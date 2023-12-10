using System.Runtime.InteropServices;
using OpenSteamworks.Enums;

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct CAmount
{
	public int m_nAmount;
	public ECurrencyCode m_eCurrencyCode;
};