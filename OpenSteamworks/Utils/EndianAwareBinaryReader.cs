using System.Buffers.Binary;
using System.IO;
using System.Text;

namespace OpenSteamworks.Utils;

public class EndianAwareBinaryReader : BinaryReader
{
    public enum Endianness
    {
        Little,
        Big,
    }

    private readonly Endianness _endianness = Endianness.Little;

    public EndianAwareBinaryReader(Stream input) : base(input)
    {
    }

    public EndianAwareBinaryReader(Stream input, Encoding encoding) : base(input, encoding)
    {
    }

    public EndianAwareBinaryReader(Stream input, Encoding encoding, bool leaveOpen) : base(
        input, encoding, leaveOpen)
    {
    }

    public EndianAwareBinaryReader(Stream input, Endianness endianness) : base(input)
    {
        _endianness = endianness;
    }

    public EndianAwareBinaryReader(Stream input, Encoding encoding, Endianness endianness) :
        base(input, encoding)
    {
        _endianness = endianness;
    }

    public EndianAwareBinaryReader(Stream input, Encoding encoding, bool leaveOpen,
        Endianness endianness) : base(input, encoding, leaveOpen)
    {
        _endianness = endianness;
    }

    public override short ReadInt16() => ReadInt16(_endianness);

    public override int ReadInt32() => ReadInt32(_endianness);

    public override long ReadInt64() => ReadInt64(_endianness);

    public override ushort ReadUInt16() => ReadUInt16(_endianness);

    public override uint ReadUInt32() => ReadUInt32(_endianness);

    public override ulong ReadUInt64() => ReadUInt64(_endianness);

    public short ReadInt16(Endianness endianness) => endianness == Endianness.Little
        ? BinaryPrimitives.ReadInt16LittleEndian(ReadBytes(sizeof(short)))
        : BinaryPrimitives.ReadInt16BigEndian(ReadBytes(sizeof(short)));

    public int ReadInt32(Endianness endianness) => endianness == Endianness.Little
        ? BinaryPrimitives.ReadInt32LittleEndian(ReadBytes(sizeof(int)))
        : BinaryPrimitives.ReadInt32BigEndian(ReadBytes(sizeof(int)));

    public long ReadInt64(Endianness endianness) => endianness == Endianness.Little
        ? BinaryPrimitives.ReadInt64LittleEndian(ReadBytes(sizeof(long)))
        : BinaryPrimitives.ReadInt64BigEndian(ReadBytes(sizeof(long)));

    public ushort ReadUInt16(Endianness endianness) => endianness == Endianness.Little
        ? BinaryPrimitives.ReadUInt16LittleEndian(ReadBytes(sizeof(ushort)))
        : BinaryPrimitives.ReadUInt16BigEndian(ReadBytes(sizeof(ushort)));

    public uint ReadUInt32(Endianness endianness) => endianness == Endianness.Little
        ? BinaryPrimitives.ReadUInt32LittleEndian(ReadBytes(sizeof(uint)))
        : BinaryPrimitives.ReadUInt32BigEndian(ReadBytes(sizeof(uint)));

    public ulong ReadUInt64(Endianness endianness) => endianness == Endianness.Little
        ? BinaryPrimitives.ReadUInt64LittleEndian(ReadBytes(sizeof(ulong)))
        : BinaryPrimitives.ReadUInt64BigEndian(ReadBytes(sizeof(ulong)));
}