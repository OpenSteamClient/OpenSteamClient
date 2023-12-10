using System.Runtime.InteropServices;
using Google.Protobuf;
using OpenSteamworks.Native;
using OpenSteamworks.Protobuf;

namespace OpenSteamworks.Client.Utils;

/// <summary>
/// This is terrible. Absolutely nothing about this should work.
/// A class for marshalling managed protobuf objects to unmanaged pointers for the sake of interoperability with native binaries.
/// </summary>
public unsafe static class ProtobufHack {
    public class Proto_Disposable<T> : IDisposable where T: IMessage<T>, new()
    {
        private readonly delegate* unmanaged[Cdecl]<IntPtr, void> deletor;
        private readonly delegate* unmanaged[Cdecl]<IntPtr> constructor;
        private readonly delegate* unmanaged[Cdecl]<void*, int, IntPtr> deserializer;

        public IntPtr ptr;
        private bool disposed = false;
        public T GetManaged() {
            if (disposed) {
                throw new ObjectDisposedException("");
            }

            var parser = new MessageParser<T>(() => new T());
            var length = ProtobufHack.Protobuf_ByteSizeLong(ptr);
            var bytes = new byte[length];
            fixed (byte* bptr = bytes) {
                if (!ProtobufHack.Protobuf_SerializeToArray(ptr, bptr, length)) {
                    throw new Exception("Failed to serialize in native code!");
                }
            }
            
            return parser.ParseFrom(bytes);
        }

        private Proto_Disposable(string nativename) {
            var lib = GetProtobufHackLib();
            this.constructor = (delegate* unmanaged[Cdecl]<IntPtr>)lib.GetExport(nativename + "_Construct");
            this.deletor = (delegate* unmanaged[Cdecl]<IntPtr, void>)lib.GetExport(nativename + "_Delete");
            this.deserializer = (delegate* unmanaged[Cdecl]<void*, int, IntPtr>)lib.GetExport(nativename + "_Deserialize");
            
            if (this.constructor == null || this.deletor == null | this.deserializer == null) {
                throw new InvalidOperationException("This type is not supported in protobuf hack native lib");
            }

            this.ptr = constructor();
        }

        internal static Proto_Disposable<T> Create() {
            return new Proto_Disposable<T>(typeof(T).Name);
        }

        internal static Proto_Disposable<T> CreateWithName(string nativename) {
            return new Proto_Disposable<T>(nativename);
        }

        /// <summary>
        /// Copies the managed protobuf object into the unmanaged protobuf pointer
        /// </summary>
        public void CopyFrom(T managed) {
            var bytes = managed.ToByteArray();
            fixed (byte* bptr = bytes) {
                this.ptr = deserializer(bptr, bytes.Length);
            }
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
            disposed = true;
            deletor(ptr);
            ptr = 0;
        }
    }

    [DllImport("protobufhack")]
    public static extern size_t Protobuf_ByteSizeLong(IntPtr ptr);

    [DllImport("protobufhack")]
    public static extern bool Protobuf_SerializeToArray(IntPtr ptr, void* buffer, size_t maxLen);

    public static Proto_Disposable<T> Create<T>() where T: IMessage<T>, new() {
        return Proto_Disposable<T>.Create();
    }

    public static Proto_Disposable<T> CreateWithName<T>(string name) where T: IMessage<T>, new() {
        return Proto_Disposable<T>.CreateWithName(name);
    }

    private static NativeLibraryEx? loadedLibrary;
    internal static NativeLibraryEx GetProtobufHackLib() {
        if (loadedLibrary != null) {
            return loadedLibrary;
        }

        loadedLibrary = NativeLibraryEx.Load("protobufhack", false, true);
        return loadedLibrary;
    }
}