using System.IO;
using System.Runtime.InteropServices;

namespace OpenSteamworks.Extensions;

internal static class BinaryReaderExtensions {
    internal static T ReadStruct<T>(this BinaryReader reader) where T: unmanaged {
        unsafe {
            fixed (byte* ptr = reader.ReadBytes(sizeof(T))) {
                return Marshal.PtrToStructure<T>((nint)ptr);
            }
        }
    }
}