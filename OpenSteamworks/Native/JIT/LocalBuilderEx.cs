using System;
using System.Reflection.Emit;

public class LocalBuilderEx
{
    private LocalBuilder builder;
    public LocalBuilderEx(LocalBuilder builder) {
        this.builder = builder;
    }

    //
    // Summary:
    //     Gets a value indicating whether the object referred to by the local variable
    //     is pinned in memory.
    //
    // Returns:
    //     true if the object referred to by the local variable is pinned in memory; otherwise,
    //     false.
    public bool IsPinned => builder.IsPinned;
    //
    // Summary:
    //     Gets the zero-based index of the local variable within the method body.
    //
    // Returns:
    //     An integer value that represents the order of declaration of the local variable
    //     within the method body.
    public int LocalIndex => builder.LocalIndex;
    //
    // Summary:
    //     Gets the type of the local variable.
    //
    // Returns:
    //     The System.Type of the local variable.
    public Type LocalType => builder.LocalType;

    public string LocalName { get; private set; } = "";

    public void SetLocalSymInfo(string name) {
        this.LocalName = name;
    }
}