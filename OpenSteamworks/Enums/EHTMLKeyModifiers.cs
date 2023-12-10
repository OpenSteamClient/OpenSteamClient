using System;

namespace OpenSteamworks.Enums;

[Flags]
public enum EHTMLKeyModifiers : uint
{
	None = 0,
	AltDown = 1 << 0,
	CtrlDown = 1 << 1,
    ShiftDown = 1 << 2,
};