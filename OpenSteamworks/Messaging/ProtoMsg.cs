using System;

namespace OpenSteamworks.Messaging;

public class ProtoMsg<T>
{
    private CMsgProtoBufHeader header;
    private T msg;
    /// <summary>
    /// Initializes a new instance of the <see cref="Message"/> class.
    /// </summary>
    protected ProtoMsg()
    {
        JobID = UInt64.MaxValue;
        JobName = "";
        header = new CMsgProtoBufHeader();
        header.
    }

    /// <summary>
    /// Gets or sets the job ID this callback refers to. If it is not a job callback, it will be <see cref="UInt64.MaxValue" />.
    /// </summary>
    public UInt64 JobID { get; set; }
    public string JobName { get; set; }
}