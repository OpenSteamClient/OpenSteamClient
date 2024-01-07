using System;

namespace OpenSteamworks.Attributes;

[System.AttributeUsage(System.AttributeTargets.Parameter, Inherited = false, AllowMultiple = false)]
sealed class ProtobufPtrTypeAttribute : System.Attribute
{    
    public Type ProtobufType { get; init; }
    public ProtobufPtrTypeAttribute(Type type) { ProtobufType = type; }
}