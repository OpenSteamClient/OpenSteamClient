using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using OpenSteamworks.Utils;

namespace OpenSteamworks.KeyValues;

public sealed class KVBinarySerializer : IDisposable {
    private readonly Stream stream;
    private readonly EndianAwareBinaryWriter writer;

    public KVBinarySerializer(Stream stream) {
        this.stream = stream;
        this.writer = new EndianAwareBinaryWriter(stream, Encoding.Default, true, EndianAwareBinaryWriter.Endianness.Little);
    }
    
    public static byte[] SerializeToArray(KVObject toSerialize) {
        using (var serializer = new KVBinarySerializer(new MemoryStream()))
        {
            serializer.SerializeRootObject(toSerialize);
            return (serializer.stream as MemoryStream)!.ToArray();
        }
    }

    public static void Serialize(Stream stream, KVObject toSerialize) {
        using (var serializer = new KVBinarySerializer(stream))
        {
            serializer.SerializeRootObject(toSerialize);
        }
    }

    private void SerializeRootObject(KVObject obj) {
        SerializeInternal(obj);
        stream.WriteByte((byte)BType.End);
    }

    private void SerializeInternal(KVObject toSerialize) {
        var type = WriteTypeAndName(toSerialize);
        if (toSerialize.HasChildren) {
            foreach (var item in toSerialize.Children)
            {
                SerializeInternal(item);
            }

            stream.WriteByte((byte)BType.End);
            return;
        }

        switch (type) {            
            case BType.String:
                stream.Write(Encoding.UTF8.GetBytes(toSerialize.Value + "\0"));
                break;
            
            case BType.Int32:
            case BType.Color:
            case BType.Pointer:
                writer.WriteInt32(toSerialize.Value);
                break;
            
            case BType.UInt64:
                writer.WriteUInt64(toSerialize.Value);
                break;
            
            case BType.Int64:
                writer.WriteInt64(toSerialize.Value);
                break;
            
            case BType.Float32:
                writer.Write((float)toSerialize.Value);
                break;
            
            default:
                throw new Exception($"Unknown/unhandled KV type {type}");
        }
    }

    private BType WriteTypeAndName(KVObject obj) {
        BType type = GetBTypeFromType(obj.Value.GetType());
        stream.WriteByte((byte)type);
        stream.Write(Encoding.UTF8.GetBytes(obj.Name + "\0"));
        return type;
    }

    private static BType GetBTypeFromType(Type type) {
        if (type == typeof(string)) {
            return BType.String;
        } else if (type == typeof(List<KVObject>)) {
            return BType.ChildObject;
        } else if (type == typeof(int)) {
            return BType.Int32;
        } else if (type == typeof(ulong)) {
            return BType.UInt64;
        } else if (type == typeof(long)) {
            return BType.Int64;
        } else if (type == typeof(float)) {
            return BType.Float32;
        }

        throw new InvalidOperationException("Type " + type + " has no corresponding BType");
    }

    public void Dispose()
    {
        ((IDisposable)writer).Dispose();
    }
}