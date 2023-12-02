using System.Runtime.InteropServices;
using OpenSteamworks.Protobuf;

namespace OpenSteamworks.Client.Utils;

/// <summary>
/// This is terrible. Absolutely nothing about this should work.
/// A class for marshalling managed protobuf objects to unmanaged pointers for the sake of interoperability with native binaries.
/// </summary>
public unsafe static class ProtobufHack {
    public class CMsgCellList_Disposable : IDisposable
    {
        public IntPtr ptr;
        private bool disposed = false;
        public CMsgCellList GetManaged() {
            if (disposed) {
                throw new ObjectDisposedException("");
            }

            var length = ProtobufHack.Protobuf_ByteSizeLong(ptr);
            var bytes = new byte[length];
            fixed (byte* bptr = bytes) {
                if (!ProtobufHack.Protobuf_SerializeToArray(ptr, bptr, length)) {
                    throw new Exception("Failed to serialize in native code!");
                }
            }

            return CMsgCellList.Parser.ParseFrom(bytes);
        }

        internal CMsgCellList_Disposable() {
            this.ptr = CMsgCellList_Construct();
        }

        public void Dispose()
        {
            disposed = true;
            CMsgCellList_Delete(ptr);
            ptr = 0;
        }
    }

    //TODO: auto generate allllll of these
    [DllImport("protobufhack")]
    public static extern IntPtr CMsgCellList_Construct();

    [DllImport("protobufhack")]
    public static extern size_t Protobuf_ByteSizeLong(IntPtr ptr);

    [DllImport("protobufhack")]
    public static extern void CMsgCellList_Delete(IntPtr ptr);

    [DllImport("protobufhack")]
    public static extern bool Protobuf_SerializeToArray(IntPtr ptr, void* buffer, size_t maxLen);

    [DllImport("protobufhack")]
    public static extern IntPtr CMsgCellList_Deserialize(void* buffer, int len);

    public static CMsgCellList_Disposable Create_CMsgCellList() {
        return new CMsgCellList_Disposable();
    }
}