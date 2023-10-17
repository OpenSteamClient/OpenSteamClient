using System;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;

namespace CustomBuildTask;

public class RunNativeCompilationTask : Task
{
    public override bool Execute()
    {
        return true;
    }
}