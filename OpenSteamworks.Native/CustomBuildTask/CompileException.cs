namespace CustomBuildTask;

[System.Serializable]
public class CompileException : System.Exception
{
    public CompileException() { }
    public CompileException(string message) : base(message) { }
    public CompileException(string message, System.Exception inner) : base(message, inner) { }
    protected CompileException(
        System.Runtime.Serialization.SerializationInfo info,
        System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
}