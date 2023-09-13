using System;
using System.Buffers.Binary;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Text;
using Google.Protobuf;
using OpenSteamworks.Enums;

namespace OpenSteamworks.Messaging;

public interface IMessage {
    byte[] Serialize();
    void FillFromBinary(byte[] data);
}
public class ProtoMsg<T> : IMessage where T: Google.Protobuf.IMessage<T>, new()
{
    public const uint PROTOBUF_MASK = 0x80000000;
    public CMsgProtoBufHeader header { get; private set; }
    public EMsg EMsg { get; set; } = 0;
    public string JobName { get; set; } = "";
    public T body;
    public readonly Google.Protobuf.MessageParser<T> body_parser;

    /// <summary>
    /// Sets whether OpenSteamworks is allowed to rewrite the message to add info to the headers.
    /// If this message is sent with a SharedConnection, the SteamClient will rewrite it regardless and that behaviour cannot be controlled
    /// </summary>
    public bool AllowRewrite { get; set; }
    /// <summary>
    /// Constructs a new ProtoMsg.
    /// </summary>
    /// <param name="jobName">The JobName of the message (leave empty if not a job)</param>
    /// <param name="unauthenticated">If this message is a job AND this argument is set, send this as a NonAuthed service call, otherwise it is sent as a regular service call</param>
    public ProtoMsg(string jobName = "", bool unauthenticated = false)
    {
        header = new CMsgProtoBufHeader();
        header.Steamid = 0;
        body = new T();
        body_parser = new Google.Protobuf.MessageParser<T>(() => body);
        if (!string.IsNullOrEmpty(jobName)) {
            header.TargetJobName = jobName;
            if (unauthenticated) {
                this.EMsg = EMsg.KEmsgServiceMethodCallFromClientNonAuthed;
            } else {
                this.EMsg = EMsg.KEmsgServiceMethodCallFromClient;
            }
        }

        this.JobName = jobName;
    }
    
    public void FillFromBinary(byte[] data) {
        using (var stream = new MemoryStream(data)) {
            // The steamclient is a strange beast. A 64-bit library compiled for little endian.
            using (var reader = new EndianAwareBinaryReader(stream, Encoding.UTF8, EndianAwareBinaryReader.Endianness.Little))
            {
                this.EMsg = (EMsg)(~PROTOBUF_MASK & reader.ReadUInt32());

                // Read the header
                var header_size = reader.ReadUInt32();
                Console.WriteLine("header_size: " + header_size);
                byte[] header_binary = reader.ReadBytes((int)header_size);

                // Parse the header
                this.header = CMsgProtoBufHeader.Parser.ParseFrom(header_binary);

                // Read the body
                var body_size = stream.Length - stream.Position;
                byte[] body_binary = reader.ReadBytes((int)body_size);

                // Parse the body
                this.body = body_parser.ParseFrom(body_binary);
            }
        }
    }

    public void ThrowIfErrored() {
        if (this.header.Eresult != (int)EResult.k_EResultOK) {
            throw new Exception($"Message {typeof(T).Name} failed with eResult {this.header.Eresult}: {this.header.ErrorMessage}");
        }
    }

    public byte[] Serialize() {
        using (var stream = new MemoryStream())
        {
            using (var writer = new EndianAwareBinaryWriter(stream, Encoding.UTF8, EndianAwareBinaryWriter.Endianness.Little))
            {
                uint header_size = (uint)this.header.CalculateSize();
                writer.WriteUInt32(PROTOBUF_MASK | (uint)EMsg);

                // Serialize header
                writer.WriteUInt32(header_size);
                writer.Write(this.header.ToByteArray());

                // Serialize body
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
        builder.AppendLine("Body: " + this.body.ToString());
        return builder.ToString();
    }
}