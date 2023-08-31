using System;
using System.Buffers.Binary;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Text;
using Google.Protobuf;

namespace OpenSteamworks.Messaging;

public class ProtoMsg<T> where T: Google.Protobuf.IMessage<T>, new()
{
    public const uint PROTOBUF_MASK = 0x80000000;
    public CMsgProtoBufHeader header { get; private set; }
    public uint EMsg { get; set; }
    public T? body;
    public readonly Google.Protobuf.MessageParser<T> body_parser;
    public ProtoMsg()
    {
        header = new CMsgProtoBufHeader();
        body_parser = new Google.Protobuf.MessageParser<T>(() => new T());
    }

    [MemberNotNull(nameof(body))]
    public void FillFromBinary(byte[] data) {
        using (var stream = new MemoryStream(data)) {
            // The steamclient is a strange beast. A 64-bit library compiled for little endian.
            using (var reader = new EndianAwareBinaryReader(stream, Encoding.UTF8, EndianAwareBinaryReader.Endianness.Little))
            {
                this.EMsg = ~PROTOBUF_MASK & reader.ReadUInt32();

                // Read the header
                var header_size = reader.ReadUInt32();
                byte[] header_binary = reader.ReadBytes((int)header_size);

                // Parse the header
                this.header = CMsgProtoBufHeader.Parser.ParseFrom(header_binary);

                // Read the body
                var body_size = reader.ReadUInt32();
                byte[] body_binary = reader.ReadBytes((int)body_size);

                // Parse the body
                this.body = body_parser.ParseFrom(body_binary);
            }
        }
    }

    public byte[] Serialize() {
        if (this.body == null) {
            throw new InvalidOperationException("body is null");
        }

        using (var stream = new MemoryStream())
        {
            using (var writer = new EndianAwareBinaryWriter(stream, Encoding.UTF8, EndianAwareBinaryWriter.Endianness.Little))
            {
                uint header_size = (uint)this.header.CalculateSize();
                uint body_size = (uint)this.body.CalculateSize();
                writer.WriteUInt32(PROTOBUF_MASK | EMsg);

                // Serialize header
                writer.WriteUInt32(header_size);
                writer.Write(this.header.ToByteArray());

                // Serialize body
                writer.WriteUInt32(body_size);
                writer.Write(this.body.ToByteArray());
            }

            return stream.ToArray();
        }
    }

    public override string ToString()
    {
        StringBuilder builder = new StringBuilder();
        builder.AppendLine(string.Format("Printing message {0}, EMsg: {1}", typeof(T).FullName, this.EMsg));
        builder.AppendLine("Header: " + this.header.ToString());
        builder.AppendLine("Body: " + this.body == null ? "(null)" : this.body!.ToString());
        return builder.ToString();
    }
}