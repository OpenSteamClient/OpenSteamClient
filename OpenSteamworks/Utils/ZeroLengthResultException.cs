namespace OpenSteamworks.Utils;

[System.Serializable]
public class ZeroLengthResultException : System.Exception
{
    public ZeroLengthResultException() { }
    public ZeroLengthResultException(string message) : base(message) { }
    public ZeroLengthResultException(string message, System.Exception inner) : base(message, inner) { }
    protected ZeroLengthResultException(
        System.Runtime.Serialization.SerializationInfo info,
        System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
}