using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using OpenSteamworks.Utils;

namespace OpenSteamworks.Structs;

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct CGameID : System.IEquatable<CGameID>, System.IComparable<CGameID> {
	public ulong m_GameID;

	public enum EGameIDType {
		k_EGameIDTypeApp = 0,
		k_EGameIDTypeGameMod = 1,
		k_EGameIDTypeShortcut = 2,
		k_EGameIDTypeP2P = 3,
	};

	public CGameID(ulong GameID) {
		m_GameID = GameID;
	}

	public CGameID(AppId_t nAppID) {
		m_GameID = 0;
		SetAppID(nAppID);
	}

	public CGameID(AppId_t nAppID, uint nModID) {
		m_GameID = 0;
		SetAppID(nAppID);
		SetType(EGameIDType.k_EGameIDTypeGameMod);
		SetModID(nModID);
	}

	/// <summary>
    /// Format: 
    /// A:730 (as appid)
    /// G:730 (as gameid)
    /// M:730:2 (appid:modid pair)
    /// </summary>
    /// <param name="dbgStr"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
	private static CGameID InterfaceDebuggerSupport(string dbgStr) {
		switch (dbgStr[0])
		{
			case 'A':
                return new CGameID(UInt32.Parse(dbgStr[1..]));
			case 'G':
				return new CGameID(ulong.Parse(dbgStr[1..]));
			case 'M':
                var appidStr = dbgStr[1..].Split(':')[0];
				var modidStr = dbgStr[1..].Split(':')[1];
                return new CGameID(UInt32.Parse(appidStr), uint.Parse(modidStr));
        }

        throw new ArgumentOutOfRangeException(nameof(dbgStr), "unknown dbg string type");
    }

	/// <summary>
    /// Constructor for a mod.
    /// Path does not need to point to a valid place on the filesystem.
    /// The given values are used for hashing via CRC32 (should be compatible with the way steam does it) to generate a proper gameid
    /// </summary>
	public CGameID(AppId_t nAppID, string modPath) {
		m_GameID = 0;
		SetAppID(nAppID);
		SetType(EGameIDType.k_EGameIDTypeGameMod);

        CRC32 crc32 = new();
        string? toCrc = Path.GetDirectoryName(modPath);
		if (toCrc == null) {
            toCrc = modPath;
        }

        byte[] pathBytes = Encoding.UTF8.GetBytes(toCrc);
        crc32.AddBytes(pathBytes);
        SetModID(crc32.GetHash() | (0x80000000));
	}


	/// <summary>
    /// Constructor for a non-steam game (shortcut).
    /// Path does not need to point to a valid place on the filesystem.
    /// The given values are used for hashing via CRC32 (should be compatible with the way steam does it) to generate a proper gameid
    /// </summary>
	public CGameID(string nonSteamAppPath, string nonSteamGameName) {
		m_GameID = 0;
		SetAppID(0);
		SetType(EGameIDType.k_EGameIDTypeShortcut);
        CRC32 crc32 = new();
		byte[] pathBytes = Encoding.UTF8.GetBytes(nonSteamAppPath);
		byte[] nameBytes = Encoding.UTF8.GetBytes(nonSteamGameName);
        crc32.AddBytes(pathBytes);
		crc32.AddBytes(nameBytes);
        SetModID(crc32.GetHash() | (0x80000000));
	}

	public bool IsSteamApp() {
		return Type() == EGameIDType.k_EGameIDTypeApp;
	}

	public bool IsMod() {
		return Type() == EGameIDType.k_EGameIDTypeGameMod;
	}

	public bool IsShortcut() {
		return Type() == EGameIDType.k_EGameIDTypeShortcut;
	}

	public bool IsP2PFile() {
		return Type() == EGameIDType.k_EGameIDTypeP2P;
	}

	public AppId_t AppID() {
		return new AppId_t((uint)(m_GameID & 0xFFFFFFul));
	}

	public EGameIDType Type() {
		return (EGameIDType)((m_GameID >> 24) & 0xFFul);
	}

	public uint ModID() {
		return (uint)((m_GameID >> 32) & 0xFFFFFFFFul);
	}

	public bool IsValid() {
		// Each type has it's own invalid fixed point:
		switch (Type()) {
			case EGameIDType.k_EGameIDTypeApp:
				return AppID() != 0;

			case EGameIDType.k_EGameIDTypeGameMod:
				return AppID() != 0 && (ModID() & 0x80000000) != 0;

			case EGameIDType.k_EGameIDTypeShortcut:
				return (ModID() & 0x80000000) != 0;

			case EGameIDType.k_EGameIDTypeP2P:
				return AppID() == 0 && (ModID() & 0x80000000) != 0;

			default:
				return false;
		}
	}

	public void Reset() {
		m_GameID = 0;
	}

	public void Set(ulong GameID) {
		m_GameID = GameID;
	}

	private void SetAppID(AppId_t other) {
		m_GameID = (m_GameID & ~(0xFFFFFFul << (ushort)0)) | (((ulong)(other) & 0xFFFFFFul) << (ushort)0);
	}

	private void SetType(EGameIDType other) {
		m_GameID = (m_GameID & ~(0xFFul << (ushort)24)) | (((ulong)(other) & 0xFFul) << (ushort)24);
	}

	private void SetModID(uint other) {
		m_GameID = (m_GameID & ~(0xFFFFFFFFul << (ushort)32)) | (((ulong)(other) & 0xFFFFFFFFul) << (ushort)32);
	}

	public override string ToString() {
		return m_GameID.ToString();
	}

	public override bool Equals(object? other) {
		return other != null && other is CGameID && this == (CGameID)other;
	}

	public override int GetHashCode() {
		return m_GameID.GetHashCode();
	}

	public static bool operator ==(CGameID x, CGameID y) {
		return x.m_GameID == y.m_GameID;
	}

	public static bool operator !=(CGameID x, CGameID y) {
		return !(x == y);
	}

	public static explicit operator CGameID(ulong value) {
		return new CGameID(value);
	}
	public static explicit operator ulong(CGameID that) {
		return that.m_GameID;
	}

	public bool Equals(CGameID other) {
		return m_GameID == other.m_GameID;
	}

	public int CompareTo(CGameID other) {
		return m_GameID.CompareTo(other.m_GameID);
	}
}