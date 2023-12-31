using System;
using System.Buffers.Binary;
using System.IO;
using System.Text;

internal class EndianAwareBinaryWriter : BinaryWriter
{
    public enum Endianness
    {
        Little,
        Big,
    }

    private readonly Endianness _endianness = Endianness.Little;

    public EndianAwareBinaryWriter(Stream output) : base(output)
    {
    }

    public EndianAwareBinaryWriter(Stream output, Encoding encoding) : base(output, encoding)
    {
    }

    public EndianAwareBinaryWriter(Stream output, Encoding encoding, bool leaveOpen) : base(
        output, encoding, leaveOpen)
    {
    }

    public EndianAwareBinaryWriter(Stream output, Endianness endianness) : base(output)
    {
        _endianness = endianness;
    }

    public EndianAwareBinaryWriter(Stream output, Encoding encoding, Endianness endianness) :
        base(output, encoding)
    {
        _endianness = endianness;
    }

    public EndianAwareBinaryWriter(Stream output, Encoding encoding, bool leaveOpen,
        Endianness endianness) : base(output, encoding, leaveOpen)
    {
        _endianness = endianness;
    }

    public void WriteInt16(Int16 val) {
        var arr = new byte[sizeof(Int16)];
        switch (_endianness)
        {
            case Endianness.Little: 
                BinaryPrimitives.WriteInt16LittleEndian(arr, val);
                break;
            case Endianness.Big:
                BinaryPrimitives.WriteInt16BigEndian(arr, val);
                break;
        }
        
        Write(arr);
    }

    public void WriteInt32(Int32 val) {
        var arr = new byte[sizeof(Int32)];
        switch (_endianness)
        {
            case Endianness.Little: 
                BinaryPrimitives.WriteInt32LittleEndian(arr, val);
                break;
            case Endianness.Big:
                BinaryPrimitives.WriteInt32BigEndian(arr, val);
                break;
        }
        
        Write(arr);
    }

    public void WriteInt64(Int64 val) {
        var arr = new byte[sizeof(Int64)];
        switch (_endianness)
        {
            case Endianness.Little: 
                BinaryPrimitives.WriteInt64LittleEndian(arr, val);
                break;
            case Endianness.Big:
                BinaryPrimitives.WriteInt64BigEndian(arr, val);
                break;
        }
        
        Write(arr);
    }

    public void WriteUInt16(UInt16 val) {
        var arr = new byte[sizeof(UInt16)];
        switch (_endianness)
        {
            case Endianness.Little: 
                BinaryPrimitives.WriteUInt16LittleEndian(arr, val);
                break;
            case Endianness.Big:
                BinaryPrimitives.WriteUInt16BigEndian(arr, val);
                break;
        }
        
        Write(arr);
    }

    public void WriteUInt32(UInt32 val) {
        var arr = new byte[sizeof(UInt32)];
        switch (_endianness)
        {
            case Endianness.Little: 
                BinaryPrimitives.WriteUInt32LittleEndian(arr, val);
                break;
            case Endianness.Big:
                BinaryPrimitives.WriteUInt32BigEndian(arr, val);
                break;
        }
        
        Write(arr);
    }

    public void WriteUInt64(UInt64 val) {
        var arr = new byte[sizeof(UInt64)];
        switch (_endianness)
        {
            case Endianness.Little: 
                BinaryPrimitives.WriteUInt64LittleEndian(arr, val);
                break;
            case Endianness.Big:
                BinaryPrimitives.WriteUInt64BigEndian(arr, val);
                break;
        }
        
        Write(arr);
    }
}