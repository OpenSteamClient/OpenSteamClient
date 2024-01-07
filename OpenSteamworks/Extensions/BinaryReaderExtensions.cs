using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using OpenSteamworks.Utils;

namespace OpenSteamworks.Extensions;

internal static class BinaryReaderExtensions {
    internal static T ReadStruct<T>(this BinaryReader reader) where T: unmanaged {
        unsafe {
            fixed (byte* ptr = reader.ReadBytes(sizeof(T))) {
                return Marshal.PtrToStructure<T>((nint)ptr);
            }
        }
    }

    internal static object ReadStruct(this BinaryReader reader, Type type) {
        unsafe {
            fixed (byte* ptr = reader.ReadBytes(Marshal.SizeOf(type))) {
                var structt = Marshal.PtrToStructure((nint)ptr, type);
                UtilityFunctions.AssertNotNull(structt);
                return structt;
            }
        }
    }

    public static string ReadNullTerminatedString(this BinaryReader reader)
    {
        StringBuilder builder = new();
        while (true)
        {
            char c = reader.ReadChar();
            if (c == char.MinValue) {
                break;
            }
            
            builder.Append(c);
        }
        return builder.ToString();
    }
}