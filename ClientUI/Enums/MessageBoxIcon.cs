using System;

namespace ClientUI.Enums;

[Flags]
public enum MessageBoxIcon
{
    QUESTION = 1,
    INFORMATION = 2,
    WARNING = 4,
    ERROR = 8
}