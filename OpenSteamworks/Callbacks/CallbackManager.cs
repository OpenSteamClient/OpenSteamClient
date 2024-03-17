using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using OpenSteamworks;
using OpenSteamworks.Attributes;
using OpenSteamworks.Callbacks.Structs;
using OpenSteamworks.Enums;
using OpenSteamworks.Structs;

namespace OpenSteamworks.Callbacks;

public class CallResult<T> where T: struct {
    public bool failed;
    public ESteamAPICallFailure failureReason;
    public T data;

    public CallResult(bool failed, ESteamAPICallFailure failureReason, T data) {
        this.failed = failed;
        this.failureReason = failureReason;
        this.data = data;
    }
}

//TODO: The code in this class is (still) pretty terrible
public class CallbackManager
{
    public interface ICallbackHandler {
        /// <summary>
        /// The callback id's this handler should be invoked for
        /// </summary>
        public int CallbackID { get; init; }
        public void Invoke(object obj);

        /// <summary>
        /// If the handler should be removed after executing.
        /// </summary>
        public bool OneShot { get; }
    }

    public class CallbackHandler : ICallbackHandler {
        /// <inheritdoc/>
        public int CallbackID { get; init; }

        /// <inheritdoc/>
        public bool OneShot { get; private set; }

        private readonly Action<CallbackHandler, byte[]> func;
        private readonly CallbackManager manager;

        internal CallbackHandler(CallbackManager manager, int callbackID, Action<CallbackHandler, byte[]> func, bool oneShot) {
            this.manager = manager;
            this.CallbackID = callbackID;
            this.func = func;
            this.OneShot = oneShot;
        }

        /// <inheritdoc/>
        public void Invoke(object obj) {
            this.func(this, (byte[])obj);
            if (OneShot) {
                lock (manager.handlersLock)
                {
                    manager.Handlers.Remove(this);
                }
            }
        }
    }

    public class CallbackHandler<T> : ICallbackHandler {
        /// <inheritdoc/>
        public int CallbackID { get; init; }

        /// <inheritdoc/>
        public bool OneShot { get; private set; }

        private readonly Action<CallbackHandler<T>, T> func;
        private readonly CallbackManager manager;

        internal CallbackHandler(CallbackManager manager, Action<CallbackHandler<T>, T> func, bool oneShot) {
            this.manager = manager;
            this.CallbackID = GetCallbackID(typeof(T));
            this.func = func;
            this.OneShot = oneShot;
        }

        private void InvokeTyped(T obj) {
            this.func(this, obj);
            if (OneShot) {
                lock (manager.handlersLock)
                {
                    manager.Handlers.Remove(this);
                }
            }
        }

        /// <inheritdoc/>
        public void Invoke(object obj) {
            this.InvokeTyped((T)obj);
        }
    }

    private readonly ISteamClient client;
    private bool poll = true;
    private bool pausePoll = false;
    private bool didPoll = true;
    private readonly Thread pollThread;
    private readonly object handlersLock = new();
    private readonly HashSet<ICallbackHandler> Handlers = new();

    internal CallbackManager(ISteamClient client) {
        this.client = client;

        // Create the thread here, but don't start it immediately
        pollThread = new(this.NativePollThread)
        {
            Name = "CallbackPollThread"
        };
    }

    public void StartThread() {
        if (!pollThread.IsAlive) {
            pollThread.Start();
        }
    }

    public void PauseThreadSync() {
        pausePoll = true;
        while (didPoll)
        {
            System.Threading.Thread.Sleep(50);
        }
    }

    public void ContinueThreadSync() {
        pausePoll = false;
        while (!didPoll)
        {
            System.Threading.Thread.Sleep(50);
        }
    }

    public async Task PauseThreadAsync() {
        await Task.Run(PauseThreadSync);
    }

    public async Task ContinueThreadAsync() {
        await Task.Run(ContinueThreadSync);
    }

    /// <summary>
    /// Waits for an api call with the specified type to complete. Might block forever if given an invalid handle or if the call creating function succeeds too quickly before the handler gets registered.
    /// </summary>
    /// <typeparam name="T">Type of the call</typeparam>
    /// <param name="handle">Handle to the call</param>
    /// <param name="resumeThread">Whether to continue callbackthread after the handler is registered internally</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    public async Task<CallResult<T>> WaitForAPICallResultAsync<T>(SteamAPICall_t handle, bool resumeThread = true, CancellationToken cancellationToken = default) where T: struct {
        await this.PauseThreadAsync();
        
        var tcs = new TaskCompletionSource<CallResult<T>>();
        unsafe {
            int callbackID = GetCallbackID(typeof(T));
            int callbackSize = Marshal.SizeOf<T>();

            var handler = this.RegisterHandler((CallbackHandler<SteamAPICallCompleted_t> handler, SteamAPICallCompleted_t compl) =>
            {
                if (compl.m_hAsyncCall == handle) {
                    fixed (byte* data = new byte[callbackSize]) {
                        if (!this.client.IClientUtils.GetAPICallResult(handle, data, callbackSize, callbackID, out bool failed)) {
                            throw new Exception("GetAPICallResult returned false with our handle after receiving SteamAPICallCompleted_t. Bugged?");
                        }

                        ESteamAPICallFailure failureReason = this.client.IClientUtils.GetAPICallFailureReason(handle);
                        if (failed) {
                            tcs.TrySetResult(new CallResult<T>(failed, failureReason, default));
                        } else {
                            if (data == null) {
                                failed = true;
                                tcs.TrySetResult(new CallResult<T>(failed, failureReason, default));
                                return;
                            }

                            T val = Marshal.PtrToStructure<T>((IntPtr)data);
                            tcs.TrySetResult(new CallResult<T>(failed, failureReason, val));
                        }

                        this.DeregisterHandler(handler);
                    }
                }
            }, false);

            cancellationToken.Register(() =>
            {
                this.DeregisterHandler(handler);
                tcs.TrySetCanceled();
            });
        }

        if (resumeThread) {
            await this.ContinueThreadAsync();
        }

        return await tcs.Task;
    }

    /// <summary>
    /// Waits for an api call with the specified type to complete. Might block forever if given an invalid handle or if the call creating function succeeds too quickly before the handler gets registered.
    /// </summary>
    /// <typeparam name="T">Type of the call</typeparam>
    /// <param name="handle">Handle to the call</param>
    /// <param name="resumeThread">Whether to continue callbackthread after the handler is registered internally</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    public CallResult<T> WaitForAPICallResultSync<T>(SteamAPICall_t handle, bool resumeThread = true) where T: unmanaged {
        this.PauseThreadSync();
        
        var tcs = new TaskCompletionSource<CallResult<T>>();
        unsafe {
            int callbackID = GetCallbackID(typeof(T));
            int callbackSize = sizeof(T);

            var handler = this.RegisterHandler((CallbackHandler<SteamAPICallCompleted_t> handler, SteamAPICallCompleted_t compl) =>
            {
                if (compl.m_hAsyncCall == handle) {
                    fixed (byte* data = new byte[callbackSize]) {
                        if (!this.client.IClientUtils.GetAPICallResult(handle, data, callbackSize, callbackID, out bool failed)) {
                            throw new Exception("GetAPICallResult returned false with our handle after receiving SteamAPICallCompleted_t. Bugged?");
                        }

                        ESteamAPICallFailure failureReason = this.client.IClientUtils.GetAPICallFailureReason(handle);
                        if (failed) {
                            tcs.TrySetResult(new CallResult<T>(failed, failureReason, default));
                        } else {
                            if (data == null) {
                                failed = true;
                                tcs.TrySetResult(new CallResult<T>(failed, failureReason, default));
                                return;
                            }

                            T val = Marshal.PtrToStructure<T>((IntPtr)data);
                            tcs.TrySetResult(new CallResult<T>(failed, failureReason, val));
                        }

                        this.DeregisterHandler(handler);
                    }
                }
            }, false);
        }

        if (resumeThread) {
            this.ContinueThreadSync();
        }

        tcs.Task.Wait();
        return tcs.Task.Result;
    }
    
    private void NativePollThread() {
        bool hasCallback = false;
        CallbackMsg_t msg = new();
        Stopwatch fullPollTime = new();
        Stopwatch runFrameTime = new();
        Stopwatch bGetCallbackTime = new();
        Stopwatch idToTypeTime = new();
        Stopwatch logCallbackTime = new();
        Stopwatch ptrToStructureTime = new();
        Stopwatch logCallbackDataTime = new();
        Stopwatch getHandlersTime = new();
        Stopwatch executeHandlersTime = new();

        unsafe {
            do
            {                
                if (pausePoll) {
                    didPoll = false;
                    do
                    {
                        System.Threading.Thread.Sleep(50);
                    } while (pausePoll);
                    didPoll = true;
                }

                fullPollTime.Reset();
                runFrameTime.Reset();
                bGetCallbackTime.Reset();
                idToTypeTime.Reset();
                logCallbackTime.Reset();
                ptrToStructureTime.Reset();
                logCallbackDataTime.Reset();
                getHandlersTime.Reset();
                executeHandlersTime.Reset();

                fullPollTime.Start();

                runFrameTime.Start();
                this.client.IClientEngine.RunFrame();
                runFrameTime.Stop();

                bGetCallbackTime.Start();
                hasCallback = this.client.BGetCallback(out msg);
                bGetCallbackTime.Stop();

                if (!hasCallback) {
                    // Sleep only if we have no extra messages
                    System.Threading.Thread.Sleep(1);
                    continue;
                }

                idToTypeTime.Start();
                var hasType = CallbackConstants.IDToType.TryGetValue(msg.callbackID, out Type? type);
                idToTypeTime.Stop();

                // Don't log HTML_NeedsPaint_t, as it'll make the browser lag. Also add other high-intensity events to this list to speed up their processing.
                bool loggable = msg.callbackID != 4502;

                if (loggable) {
                    logCallbackTime.Start();
                    LogCallback(msg);
                    logCallbackTime.Stop();
                }

                try
                {
                    ptrToStructureTime.Start();
                    object? obj;
                    if (!hasType) {
                        obj = msg.callbackData;
                    } else {
                        fixed (byte* ptr = msg.callbackData) {
                            obj = Marshal.PtrToStructure((IntPtr)ptr, type!);
                        }
                    }

                    ptrToStructureTime.Stop();

                    if (obj != null) {
                        if (hasType && loggable) {
                            logCallbackDataTime.Start();
                            LogCallbackData(obj, type!);
                            logCallbackDataTime.Stop();
                        }

                        // Send to listeners if any exist
                        getHandlersTime.Start();
                        if (GetHandlersForId(msg.callbackID, out List<ICallbackHandler>? handlers)) {
                            getHandlersTime.Stop();
                            executeHandlersTime.Start();
                            foreach (var handler in handlers)
                            {
                                handler.Invoke(obj);
                            }
                            executeHandlersTime.Stop();
                        }
                        
                        getHandlersTime.Stop();

                    } else {
                        Logging.CallbackLogger.Error("PtrToStructure returned null. Message skipped.");
                    }
                }
                catch (System.Exception e)
                {
                    Logging.CallbackLogger.Error("Message handling threw an exception. Message skipped.");
                    Logging.CallbackLogger.Error(e);
                }

                this.client.FreeLastCallback();
                fullPollTime.Stop();
                Logging.CallbackLogger.Debug($"Callback handling took {fullPollTime.Elapsed.TotalMilliseconds}ms (RunFrame: {runFrameTime.Elapsed.TotalMilliseconds}ms, BGetCallback: {bGetCallbackTime.Elapsed.TotalMilliseconds}ms, IdToType lookup: {idToTypeTime.Elapsed.TotalMilliseconds}ms, LogCallback: {logCallbackTime.Elapsed.TotalMilliseconds}ms, PtrToStructure: {ptrToStructureTime.Elapsed.TotalMilliseconds}ms, LogCallbackData: {logCallbackDataTime.Elapsed.TotalMilliseconds}ms, GetHandlers: {getHandlersTime.Elapsed.TotalMilliseconds}ms, ExecuteHandlers: {executeHandlersTime.Elapsed.TotalMilliseconds}ms)");
            } while (poll);
        }
    }

    private void LogCallback(CallbackMsg_t msg) {
        if (Logging.LogIncomingCallbacks)
        {
            string callbackName = "Unknown";
            if (CallbackConstants.CallbackNames.TryGetValue(msg.callbackID, out string? callbackNameOut))
            {
                callbackName = callbackNameOut;
            }

            unsafe {
                lock (handlersLock)
                {
                    Logging.CallbackLogger.Debug($"Received callback [ID: {msg.callbackID}, name: {callbackName}, param length: {msg.callbackData.Length}, data: {string.Join(" ", msg.callbackData)}, has handlers: {Handlers.Any(e => e.CallbackID == msg.callbackID)}]");
                }
            }
        }
    }

    private void LogCallbackData(object obj, Type type) {
        if (Logging.LogCallbackContents) {
            var fields = type.GetFields();
            Logging.CallbackLogger.Debug($"Begin Message {type.Name}");
            try {
                foreach (var field in fields)
                {
                    dynamic? value = field.GetValue(obj);
                    string? substituteValue = null;
                    if (value is not null) {
                        try
                        {
                            var valueType = value.GetType();
                            if (valueType.IsArray) {
                                List<string> strings = new();
                                foreach (var item in value)
                                {
                                    strings.Add(item.ToString());
                                }

                                substituteValue = $"[{string.Join(",", strings)}]";
                            }
                        }
                        catch (System.Exception)
                        {
                            substituteValue = "(threw an exception)";
                        }
                    } else {
                        substituteValue = "null";
                    }
                    
                    if (substituteValue != null) {
                        Logging.CallbackLogger.Debug("    " + field.Name + ": " + substituteValue);
                    } else {
                        Logging.CallbackLogger.Debug("    " + field.Name + ": " + value);
                    }
                    
                }
            } catch (Exception) {
                Logging.CallbackLogger.Debug("Encountered an error printing message.");
            }

            Logging.CallbackLogger.Debug($"End of Message {type.Name}");
        }
    }

    private bool GetHandlersForId(int id, [NotNullWhen(true)] out List<ICallbackHandler> handlersOut) {
        handlersOut = new List<ICallbackHandler>();

        lock (handlersLock)
        {
            foreach (var handler in Handlers.Where(handler => handler.CallbackID == id))
            {
                handlersOut.Add(handler);
            }
        }

        if (handlersOut.Count > 0) {
            return true;
        }

        return false;
    }

    public static int GetCallbackID(Type callbackType) {
        if (!CallbackConstants.TypeToID.TryGetValue(callbackType, out int id)) {
            throw new ArgumentException(callbackType + " was not defined in TypeToID");
        }

        return id;
    }

    public CallbackHandler RegisterHandler(int callbackID, Action<CallbackHandler, byte[]> func, bool oneShot = false) {
        CallbackHandler _handler = new(this, callbackID, func, oneShot);
        lock (handlersLock)
        {
            Handlers.Add(_handler);
        }
        return _handler;
    }

    public CallbackHandler<T> RegisterHandler<T>(Action<CallbackHandler<T>, T> func, bool oneShot = false) where T : struct {
        CallbackHandler<T> _handler = new(this, func, oneShot);
        lock (handlersLock)
        {
            Handlers.Add(_handler);
        }
        return _handler;
    }

    public CallbackHandler<T> RegisterHandler<T>(TaskCompletionSource<T> tcs) where T : struct {
        return this.RegisterHandler<T>((handler, data) =>
        {
            tcs.SetResult(data);
        }, true);
    }

    public async Task<T> WaitForCallback<T>() where T: struct {
        var tcs = new TaskCompletionSource<T>();

        RegisterHandler<T>((CallbackHandler<T> handler, T callbackResult) => {
            tcs.TrySetResult(callbackResult);
        }, true);

        return await tcs.Task;
    }

    public async Task<T> WaitForCallback<T>(Func<T, bool> checkMethod) where T: struct {
        var tcs = new TaskCompletionSource<T>();

        RegisterHandler<T>((CallbackHandler<T> handler, T callbackResult) => {
            if (checkMethod(callbackResult)) {
                tcs.TrySetResult(callbackResult);
            }
        }, true);

        return await tcs.Task;
    }

    public void DeregisterHandler<T>(CallbackHandler<T> handler) {
        lock (handlersLock)
        {
            Handlers.Remove(handler);
        }
    }

    public void RequestStopAndWaitForExit() {
        Logging.CallbackLogger.Info("Stopping CallbackThread");
        poll = false;
        do
        {
            System.Threading.Thread.Sleep(10);
        } while (this.pollThread.IsAlive);
        
        Logging.CallbackLogger.Info("Stopped CallbackThread");
    }
}