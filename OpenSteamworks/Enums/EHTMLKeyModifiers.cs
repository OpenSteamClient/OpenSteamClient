using System;

namespace OpenSteamworks.Enums;

[Flags]
public enum EHTMLKeyModifiers : uint
{
	k_EHTMLKeyModifier_None = 0,
	k_EHTMLKeyModifier_AltDown = 1 << 0,
	k_EHTMLKeyModifier_CtrlDown = 1 << 1,
    k_EHTMLKeyModifier_ShiftDown = 1 << 2,
};