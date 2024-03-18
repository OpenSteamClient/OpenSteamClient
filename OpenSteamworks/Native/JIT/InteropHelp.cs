using System;
using System.Text;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Reflection;
using Profiler;

namespace OpenSteamworks.Native.JIT
{
    public class InteropHelp
    {
        private static readonly GCHandle NullHandle = GCHandle.Alloc(new byte[0], GCHandleType.Pinned);

        /// <summary>
        /// Decodes IntPtr as if it were a UTF-8 string
        /// </summary>
        public unsafe static string? DecodeUTF8String(IntPtr ptr)
        {
            if (ptr == IntPtr.Zero)
                return null;

            int len = 0;
            while (Marshal.ReadByte(ptr, len) != 0) len++;

            if (len == 0)
                return string.Empty;

            byte[] buffer = new byte[len];
            Marshal.Copy(ptr, buffer, 0, buffer.Length);
            return Encoding.UTF8.GetString(buffer);
            //return Marshal.PtrToStringUTF8(ptr);
        }

        /// <summary>
        /// Encodes string as an IntPtr
        /// </summary>
        public static IntPtr EncodeUTF8String(string str, out GCHandle handle)
        {
            if (str == null)
            {
                handle = NullHandle;
                return IntPtr.Zero;
            }

            var length = Encoding.UTF8.GetByteCount(str);
            byte[] buffer = new byte[length + 1];

            Encoding.UTF8.GetBytes(str, 0, str.Length, buffer, 0);

            handle = GCHandle.Alloc(buffer, GCHandleType.Pinned);
            return handle.AddrOfPinnedObject();
        }

        public static void FreeString(ref GCHandle handle)
        {
            if (handle == NullHandle)
                return;

            handle.Free();
        }

        public static void ThrowIfRemotePipe() {
            if (SteamClient.instance != null && SteamClient.IsIPCCrossProcess) {
                throw new InvalidOperationException("This function cannot be called in cross-process contexts.");
            }
        }

        public static CProfiler.INodeLifetime? StartProfile(string name) {
            return CProfiler.CurrentProfiler?.EnterScope(name);
        }

        public static void EndProfile(CProfiler.INodeLifetime? lifetime) {
            lifetime?.Dispose();
        }
    }
}
