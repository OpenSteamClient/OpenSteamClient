using System.Runtime.InteropServices;
using System.Text;

public static class BinaryReaderExtensions {
    public static T ReadStruct<T>(this BinaryReader reader) where T: unmanaged {
        unsafe {
            fixed (byte* ptr = reader.ReadBytes(sizeof(T))) {
                return Marshal.PtrToStructure<T>((nint)ptr);
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