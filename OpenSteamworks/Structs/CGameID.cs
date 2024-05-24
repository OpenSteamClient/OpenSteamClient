using System;
using System.Globalization;
using System.IO;
using System.IO.Hashing;
using System.Runtime.InteropServices;
using System.Text;
using OpenSteamworks.Utils;

namespace OpenSteamworks.Structs;

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct CGameID : IEquatable<CGameID>, IComparable<CGameID> {
    public static readonly CGameID Zero = new((ulong)0);

	[field: MarshalAs(UnmanagedType.U8)]
    public ulong GameID { get; set; }

    public enum EGameIDType {
		App = 0,
		GameMod = 1,
		Shortcut = 2,
		P2P = 3,
		Invalid
	};

	public CGameID(ulong gameID) {
		GameID = gameID;
	}

	public CGameID(AppId_t nAppID) {
		GameID = 0;
        AppID = nAppID;
    }

	public CGameID(AppId_t nAppID, uint nModID) {
		GameID = 0;
		AppID = nAppID;
		Type = EGameIDType.GameMod;
		ModID = nModID;
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
                return new CGameID(UInt32.Parse(appidStr), uint.Parse(modidStr, CultureInfo.InvariantCulture.NumberFormat));
        }

        throw new ArgumentOutOfRangeException(nameof(dbgStr), "unknown dbg string type");
    }

	/// <summary>
    /// Constructor for a mod.
    /// Path does not need to point to a valid place on the filesystem.
    /// The given values are used for hashing via CRC32 (should be compatible with the way steam does it) to generate a proper gameid
    /// </summary>
	public CGameID(AppId_t nAppID, string modPath) {
		GameID = 0;
		AppID = nAppID;
		Type = EGameIDType.GameMod;

        string toCrc = Path.GetDirectoryName(modPath) ?? modPath;

        byte[] pathBytes = Encoding.UTF8.GetBytes(toCrc);
        ModID = Crc32.HashToUInt32(pathBytes) | (0x80000000);
	}


	/// <summary>
    /// Constructor for a non-steam game (shortcut).
    /// Path does not need to point to a valid place on the filesystem.
    /// The given values are used for hashing via CRC32 (should be compatible with the way steam does it) to generate a proper gameid
    /// </summary>
	public CGameID(string nonSteamAppPath, string nonSteamGameName) {
		GameID = 0;
		AppID = 0;
		Type = EGameIDType.Shortcut;
        Crc32 crc32 = new();
		byte[] pathBytes = Encoding.UTF8.GetBytes(nonSteamAppPath);
		byte[] nameBytes = Encoding.UTF8.GetBytes(nonSteamGameName);
        crc32.Append(pathBytes);
		crc32.Append(nameBytes);
        ModID = crc32.GetCurrentHashAsUInt32() | (0x80000000);
	}

	public bool IsSteamApp() {
		return Type == EGameIDType.App;
	}

	public bool IsMod() {
		return Type == EGameIDType.GameMod;
	}

	public bool IsShortcut() {
		return Type == EGameIDType.Shortcut;
	}

	public bool IsP2PFile() {
		return Type == EGameIDType.P2P;
	}

	public bool IsValid() {
		if (GameID == 0) {
            return false;
        }

		// Each type has it's own invalid fixed point:
		switch (Type) {
			case EGameIDType.App:
				return AppID != 0;

			case EGameIDType.GameMod:
				return AppID != 0 && (ModID & 0x80000000) != 0;

			case EGameIDType.Shortcut:
				return (ModID & 0x80000000) != 0;

			case EGameIDType.P2P:
				return AppID == 0 && (ModID & 0x80000000) != 0;

			default:
				return false;
		}
	}

	public void Reset() {
		GameID = 0;
	}

	public void Set(ulong gameID) {
		GameID = gameID;
	}

	public AppId_t AppID {
		get {
			if (GameID == 0) {
				return 0;
			}

			return new AppId_t((uint)(GameID & 0xFFFFFFul));
		}

		set {
			GameID = (GameID & ~(0xFFFFFFul << (ushort)0)) | (((ulong)(value) & 0xFFFFFFul) << (ushort)0);
		}
	}

	public EGameIDType Type {
		get {
			if (GameID == 0) {
				return EGameIDType.Invalid;
			}

			return (EGameIDType)((GameID >> 24) & 0xFFul);
		}

		set {
			GameID = (GameID & ~(0xFFul << (ushort)24)) | (((ulong)(value) & 0xFFul) << (ushort)24);
		}
	}
	
	public uint ModID {
		get {
			return (uint)((GameID >> 32) & 0xFFFFFFFFul);
		}

		set {
			GameID = (GameID & ~(0xFFFFFFFFul << (ushort)32)) | (((ulong)(value) & 0xFFFFFFFFul) << (ushort)32);
		}
	}

	public override string ToString() {
        char type;
        string value;
        switch (Type)
        {
            case EGameIDType.App:
				type = 'A';
                value = AppID.ToString();
                break;
            case EGameIDType.GameMod:
				type = 'M';
                value = AppID + ":" + ModID;
                break;
            case EGameIDType.Shortcut:
				type = 'S';
                value = AppID + ":" + ModID;
                break;
            case EGameIDType.P2P:
				type = 'P';
                value = GameID.ToString();
                break;
            case EGameIDType.Invalid:
            default:
                type = 'I';
				value = GameID.ToString();
                break;
        }

        return "[" + type + ":" + value + "]";
    }

    public override bool Equals(object? other) {
		return other != null && other is CGameID && this == (CGameID)other;
	}

	public override int GetHashCode() {
		return GameID.GetHashCode();
	}

	public static bool operator ==(CGameID x, CGameID y) {
		return x.GameID == y.GameID;
	}

	public static bool operator !=(CGameID x, CGameID y) {
		return !(x == y);
	}

	public static explicit operator CGameID(ulong value) {
		return new CGameID(value);
	}
	public static explicit operator ulong(CGameID that) {
		return that.GameID;
	}

	public bool Equals(CGameID other) {
		return GameID == other.GameID;
	}

	public int CompareTo(CGameID other) {
		return GameID.CompareTo(other.GameID);
	}
}