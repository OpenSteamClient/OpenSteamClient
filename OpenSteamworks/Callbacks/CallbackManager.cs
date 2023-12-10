using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
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

public class CallResult<T> where T: unmanaged {
    public bool failed;
    public ESteamAPICallFailure failureReason;
    public T data;

    public CallResult(bool failed, ESteamAPICallFailure failureReason, T data) {
        this.failed = failed;
        this.failureReason = failureReason;
        this.data = data;
    }
}

//TODO: The code in this class is pretty terrible
public class CallbackManager
{
    public interface ICallbackHandler {
        public int CallbackID { get; init; }
        public void Invoke(object obj);
    }

    public class CallbackHandler<T> : ICallbackHandler {
        public int CallbackID { get; init; }
        private readonly Action<CallbackHandler<T>, T> func;

        /// <summary>
        /// If the handler should be removed after executing.
        /// </summary>
        public bool OneShot { get; private set; }
        internal CallbackHandler(Action<CallbackHandler<T>, T> func, bool oneShot) {
            this.CallbackID = GetCallbackID(typeof(T));
            this.func = func;
            this.OneShot = oneShot;
        }

        private void InvokeTyped(T obj) {
            this.func(this, obj);
            if (OneShot) {
                lock (handlersLock)
                {
                    Handlers.Remove(this);
                }
            }
        }

        public void Invoke(object obj) {
            this.InvokeTyped((T)obj);
        }
    }

    private readonly SteamClient client;
    private bool poll = true;
    private bool pausePoll = false;
    private bool didPoll = true;
    private readonly Thread pollThread;
    private static readonly object handlersLock = new();
    private static readonly HashSet<ICallbackHandler> Handlers = new();

    internal CallbackManager(SteamClient client) {
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
    public async Task<CallResult<T>> WaitForAPICallResultAsync<T>(SteamAPICall_t handle, bool resumeThread = true, CancellationToken cancellationToken = default) where T: unmanaged {
        await this.PauseThreadAsync();
        
        var tcs = new TaskCompletionSource<CallResult<T>>();
        unsafe {
            int callbackID = GetCallbackID(typeof(T));
            int callbackSize = sizeof(T);

            var handler = this.RegisterHandler((CallbackHandler<SteamAPICallCompleted_t> handler, SteamAPICallCompleted_t compl) =>
            {
                if (compl.m_hAsyncCall == handle) {
                    fixed (byte* data = new byte[callbackSize]) {
                        if (!this.client.NativeClient.IClientUtils.GetAPICallResult(handle, data, callbackSize, callbackID, out bool failed)) {
                            throw new Exception("GetAPICallResult returned false with our handle after receiving SteamAPICallCompleted_t. Bugged?");
                        }

                        ESteamAPICallFailure failureReason = this.client.NativeClient.IClientUtils.GetAPICallFailureReason(handle);
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
                        if (!this.client.NativeClient.IClientUtils.GetAPICallResult(handle, data, callbackSize, callbackID, out bool failed)) {
                            throw new Exception("GetAPICallResult returned false with our handle after receiving SteamAPICallCompleted_t. Bugged?");
                        }

                        ESteamAPICallFailure failureReason = this.client.NativeClient.IClientUtils.GetAPICallFailureReason(handle);
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
                this.client.NativeClient.IClientEngine.RunFrame();
                runFrameTime.Stop();

                bGetCallbackTime.Start();
                hasCallback = this.client.NativeClient.native_Steam_BGetCallback(client.NativeClient.Pipe, (nint)(&msg));
                bGetCallbackTime.Stop();

                if (!hasCallback) {
                    // Sleep only if we have no extra messages
                    System.Threading.Thread.Sleep(1);
                    continue;
                }

                idToTypeTime.Start();
                var hasType = CallbackConstants.IDToType.TryGetValue(msg.m_iCallback, out Type? type);
                idToTypeTime.Stop();

                // Don't log HTML_NeedsPaint_t, as it'll make the browser lag. Also add other high-intensity events to this list to speed up their processing.
                bool loggable = msg.m_iCallback != 4502;

                if (loggable) {
                    logCallbackTime.Start();
                    LogCallback(msg);
                    logCallbackTime.Stop();
                }

                if (hasType && type != null) {
                    try
                    {
                        ptrToStructureTime.Start();
                        var obj = Marshal.PtrToStructure((IntPtr)msg.m_pubParam, type);
                        ptrToStructureTime.Stop();

                        if (obj != null) {
                            if (loggable) {
                                logCallbackDataTime.Start();
                                LogCallbackData(obj, type);
                                logCallbackDataTime.Stop();
                            }

                            // Send to listeners if any exist
                            getHandlersTime.Start();
                            if (GetHandlersForId(msg.m_iCallback, out List<ICallbackHandler>? handlers)) {
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
                            SteamClient.CallbackLogger.Error("PtrToStructure returned null. Message skipped.");
                        }
                    }
                    catch (System.Exception e)
                    {
                        SteamClient.CallbackLogger.Error("Message handling threw an exception. Message skipped.");
                        SteamClient.CallbackLogger.Error(e);
                    }
                }

                this.client.NativeClient.native_Steam_FreeLastCallback(client.NativeClient.Pipe);
                fullPollTime.Stop();
                SteamClient.CallbackLogger.Debug($"Callback handling took {fullPollTime.Elapsed.TotalMilliseconds}ms (RunFrame: {runFrameTime.Elapsed.TotalMilliseconds}ms, BGetCallback: {bGetCallbackTime.Elapsed.TotalMilliseconds}ms, IdToType lookup: {idToTypeTime.Elapsed.TotalMilliseconds}ms, LogCallback: {logCallbackTime.Elapsed.TotalMilliseconds}ms, PtrToStructure: {ptrToStructureTime.Elapsed.TotalMilliseconds}ms, LogCallbackData: {logCallbackDataTime.Elapsed.TotalMilliseconds}ms, GetHandlers: {getHandlersTime.Elapsed.TotalMilliseconds}ms, ExecuteHandlers: {executeHandlersTime.Elapsed.TotalMilliseconds}ms)");
            } while (poll);
        }
    }

    private void LogCallback(CallbackMsg_t msg) {
        if (SteamClient.LogIncomingCallbacks)
        {
            string callbackName = "Unknown";
            if (CallbackConstants.CallbackNames.TryGetValue(msg.m_iCallback, out string? callbackNameOut))
            {
                callbackName = callbackNameOut;
            }

            unsafe {
                lock (handlersLock)
                {
                    SteamClient.CallbackLogger.Debug($"Received callback [ID: {msg.m_iCallback}, name: {callbackName}, param length: {msg.m_cubParam}, data ptr: {string.Format("0x{0:x}", (IntPtr)msg.m_pubParam)}, has handlers: {Handlers.Any(e => e.CallbackID == msg.m_iCallback)}]");
                }
            }
        }
    }

    private void LogCallbackData(object obj, Type type) {
        if (SteamClient.LogCallbackContents) {
            var fields = type.GetFields();
            SteamClient.CallbackLogger.Debug($"Begin Message {type.Name}");
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
                        SteamClient.CallbackLogger.Debug("    " + field.Name + ": " + substituteValue);
                    } else {
                        SteamClient.CallbackLogger.Debug("    " + field.Name + ": " + value);
                    }
                    
                }
            } catch (Exception) {
                SteamClient.CallbackLogger.Debug("Encountered an error printing message.");
            }

            SteamClient.CallbackLogger.Debug($"End of Message {type.Name}");
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

    public CallbackHandler<T> RegisterHandler<T>(Action<CallbackHandler<T>, T> func, bool oneShot = false) where T : struct {
        CallbackHandler<T> _handler = new(func, oneShot);
        lock (handlersLock)
        {
            Handlers.Add(_handler);
        }
        return _handler;
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
        SteamClient.CallbackLogger.Info("Stopping CallbackThread");
        poll = false;
        do
        {
            System.Threading.Thread.Sleep(10);
        } while (this.pollThread.IsAlive);
        
        SteamClient.CallbackLogger.Info("Stopped CallbackThread");
    }
}