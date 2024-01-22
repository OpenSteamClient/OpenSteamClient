using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using OpenSteamworks.Extensions;
using OpenSteamworks.Utils;

namespace OpenSteamworks.KeyValues;

public sealed class KVBinaryDeserializer : IDisposable {
    private readonly Stream stream;
    private readonly EndianAwareBinaryReader reader;

    private KVBinaryDeserializer(Stream stream) {
        this.stream = stream;
        this.reader = new EndianAwareBinaryReader(stream, Encoding.Default, true, EndianAwareBinaryReader.Endianness.Little);
    }

    private bool placeholderName = true;
    private KVObject Deserialize() {
        KVObject parent = new("", new List<KVObject>());
        while (true)
        {
            bool setPlaceholderName = false;

            KVObject? deserialized;
            var type = (BType)reader.ReadByte();
            if (type == BType.End) {
                break;
            }

            var name = reader.ReadNullTerminatedUTF8String();
            dynamic value;

            if (placeholderName) {
                placeholderName = false;
                setPlaceholderName = true;
                parent.Name = name;
            }

            switch (type) {
                case BType.ChildObject:
                    value = Deserialize();
                    break;
                
                case BType.String:
                    value = reader.ReadNullTerminatedUTF8String();
                    break;
                
                case BType.Int32:
                case BType.Color:
                case BType.Pointer:
                    value = reader.ReadInt32();
                    break;
                
                case BType.UInt64:
                    value = reader.ReadUInt64();
                    break;
                
                case BType.Int64:
                    value = reader.ReadInt64();
                    break;
                
                case BType.Float32:
                    value = reader.ReadSingle();
                    break;
                
                default:
                    throw new Exception($"Unknown/unhandled KV type {type} encountered at position {stream.Position}");
		    }

            if (value is KVObject asKV) {
                deserialized = new KVObject(name, asKV.Value);
            } else {
                deserialized = new KVObject(name, value);
            }

            if (setPlaceholderName) {
                UtilityFunctions.Assert(deserialized.HasChildren);
                parent.Value = deserialized.Value;
            } else {
                parent.SetChild(deserialized);
            }
        }

        return parent;
    }

    public static KVObject Deserialize(byte[] bytes) {
        using (var serializer = new KVBinaryDeserializer(new MemoryStream(bytes)))
        {
            return serializer.Deserialize();
        }
    }

    public static KVObject Deserialize(Stream stream) {
        using (var serializer = new KVBinaryDeserializer(stream))
        {
            return serializer.Deserialize();
        }
    }

    public void Dispose()
    {
        ((IDisposable)reader).Dispose();
    }
}