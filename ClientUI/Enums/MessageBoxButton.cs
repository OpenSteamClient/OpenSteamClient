using System;

namespace ClientUI.Enums;

[Flags]
public enum MessageBoxButton
{
    No = 1,
    Yes = 2,
    Ok = 4,
    Cancel = 8
}