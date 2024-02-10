using System;
using OpenSteamworks.Converters;
using System.Runtime.InteropServices;
using System.Text.Json.Serialization;
using OpenSteamworks.Enums;
using System.Globalization;

namespace OpenSteamworks.Structs;

[StructLayout(LayoutKind.Sequential, Pack = 1)]
[JsonConverter(typeof(CSteamIDJsonConverter))]
public struct CSteamID : System.IEquatable<CSteamID>, System.IComparable<CSteamID> {
    public const int k_unSteamAccountInstanceMask = 0x000FFFFF;
    public static readonly CSteamID Zero = new(0);
    public static readonly CSteamID OutofDateGS = new(0, 0, EUniverse.Invalid, EAccountType.Invalid);
    public static readonly CSteamID LanModeGS = new(0, 0, EUniverse.Public, EAccountType.Invalid);
    public static readonly CSteamID NotInitYetGS = new(1, 0, EUniverse.Invalid, EAccountType.Invalid);
    public static readonly CSteamID NonSteamGS = new(2, 0, EUniverse.Invalid, EAccountType.Invalid);
    public const int DefaultInstance = 1;


    [field: MarshalAs(UnmanagedType.U8)]
    public ulong SteamID64 { get; set; }

    public CSteamID(AccountID_t unAccountID, EUniverse eUniverse, EAccountType eAccountType) {
        SteamID64 = 0;
        Set(unAccountID, eUniverse, eAccountType);
    }

    public CSteamID(AccountID_t unAccountID, uint unAccountInstance, EUniverse eUniverse, EAccountType eAccountType) {
        SteamID64 = 0;
        InstancedSet(unAccountID, unAccountInstance, eUniverse, eAccountType);
    }

    public CSteamID(ulong ulSteamID) {
        SteamID64 = ulSteamID;
    }

    public void Set(AccountID_t unAccountID, EUniverse eUniverse, EAccountType eAccountType) {
        AccountID = unAccountID;
        Universe = eUniverse;
        AccountType = eAccountType;

        if (eAccountType == EAccountType.Clan || eAccountType == EAccountType.GameServer) {
            AccountInstance = 0;
        }
        else {
            AccountInstance = DefaultInstance;
        }
    }

    public void InstancedSet(AccountID_t unAccountID, uint unInstance, EUniverse eUniverse, EAccountType eAccountType) {
        AccountID = unAccountID;
        Universe = eUniverse;
        AccountType = eAccountType;
        AccountInstance = unInstance;
    }

    public void Clear() {
        SteamID64 = 0;
    }

    public void CreateBlankAnonGSLogon(EUniverse eUniverse) {
        AccountID = 0;
        Universe = eUniverse;
        AccountType = EAccountType.AnonGameServer;
        AccountInstance = 0;
    }

    public void CreateBlankAnonUserLogon(EUniverse eUniverse) {
        AccountID = new AccountID_t(0);
        Universe = eUniverse;
        AccountType = EAccountType.AnonUser;
        AccountInstance = 0;
    }

    /// <summary>
    ///  Is this an anonymous game server login that will be filled in?
    /// </summary>
    public bool BBlankAnonAccount() {
        return AccountID == 0 && BAnonAccount() && AccountInstance == 0;
    }

    /// <summary>
    /// Is this a game server account id?  (Either persistent or anonymous)
    /// </summary>
    public bool BGameServerAccount() {
        return AccountType == EAccountType.GameServer || AccountType == EAccountType.AnonGameServer;
    }

    /// <summary>
    /// Is this a persistent (not anonymous) game server account id?
    /// </summary>
    public bool BPersistentGameServerAccount() {
        return AccountType == EAccountType.GameServer;
    }

    /// <summary>
    /// Is this an anonymous game server account id?
    /// </summary>
    public bool BAnonGameServerAccount() {
        return AccountType == EAccountType.AnonGameServer;
    }

    /// <summary>
    /// Is this a content server account id?
    /// </summary>
    public bool BContentServerAccount() {
        return AccountType == EAccountType.ContentServer;
    }


    /// <summary>
    /// Is this a clan account id?
    /// </summary>
    public bool BClanAccount() {
        return AccountType == EAccountType.Clan;
    }


    /// <summary>
    /// Is this a chat account id?
    /// </summary>
    public bool BChatAccount() {
        return AccountType == EAccountType.Chat;
    }

    /// <summary>
    /// Is this a (matchmaking?) lobby?
    /// </summary>
    public bool IsLobby() {
        return (AccountType == EAccountType.Chat)
            && (AccountInstance & (int)EChatSteamIDInstanceFlags.k_EChatInstanceFlagLobby) != 0;
    }


    /// <summary>
    /// Is this an individual user account id?
    /// </summary>
    public bool BIndividualAccount() {
        return AccountType == EAccountType.Individual || AccountType == EAccountType.ConsoleUser;
    }


    /// <summary>
    /// Is this an anonymous account?
    /// </summary>
    public bool BAnonAccount() {
        return AccountType == EAccountType.AnonUser || AccountType == EAccountType.AnonGameServer;
    }

    /// <summary>
    /// Is this an anonymous user account? ( used to create an account or reset a password )
    /// </summary>
    public bool BAnonUserAccount() {
        return AccountType == EAccountType.AnonUser;
    }

    public AccountID_t AccountID {
        get {
            return (uint)(SteamID64 & 0xFFFFFFFFul);
        }

        set {
            SteamID64 = (SteamID64 & ~(0xFFFFFFFFul << (ushort)0)) | (((ulong)(value) & 0xFFFFFFFFul) << (ushort)0);
        }
    }

    public uint AccountInstance {
        get {
            return (uint)((SteamID64 >> 32) & 0xFFFFFul);
        }

        set {
            SteamID64 = (SteamID64 & ~(0xFFFFFul << (ushort)32)) | (((ulong)(value) & 0xFFFFFul) << (ushort)32);
        }
    }

    public EAccountType AccountType {
        get {
            return (EAccountType)((SteamID64 >> 52) & 0xFul);
        }

        set {
            SteamID64 = (SteamID64 & ~(0xFul << (ushort)52)) | (((ulong)(value) & 0xFul) << (ushort)52);
        }
    }

    public EUniverse Universe {
        get {
            return (EUniverse)((SteamID64 >> 56) & 0xFFul);
        }

        set {
            SteamID64 = (SteamID64 & ~(0xFFul << (ushort)56)) | (((ulong)(value) & 0xFFul) << (ushort)56);
        }
    }

    public bool IsValid() {
        if (AccountType <= EAccountType.Invalid || AccountType >= EAccountType.Max)
            return false;

        if (Universe <= EUniverse.Invalid || Universe >= EUniverse.Max)
            return false;

        if (AccountType == EAccountType.Individual) {
            if (AccountID == new AccountID_t(0) || AccountInstance > DefaultInstance)
                return false;
        }

        if (AccountType == EAccountType.Clan) {
            if (AccountID == new AccountID_t(0) || AccountInstance != 0)
                return false;
        }

        if (AccountType == EAccountType.GameServer) {
            if (AccountID == new AccountID_t(0))
                return false;
            // Any limit on instances?  We use them for local users and bots
        }
        return true;
    }

    public override string ToString() {
        return SteamID64.ToString();
    }

    public override bool Equals(object? other) {
        return other is not null && other is CSteamID id && this.SteamID64 == id.SteamID64;
    }

    public override int GetHashCode() {
        return SteamID64.GetHashCode();
    }

    public static bool operator ==(CSteamID x, CSteamID y) {
        return x.SteamID64 == y.SteamID64;
    }

    public static bool operator !=(CSteamID x, CSteamID y) {
        return !(x == y);
    }

    public static implicit operator CSteamID(ulong value) {
        return new CSteamID(value);
    }
    public static implicit operator ulong(CSteamID that) {
        return that.SteamID64;
    }

    public bool Equals(CSteamID other) {
        return SteamID64 == other.SteamID64;
    }

    public int CompareTo(CSteamID other) {
        return SteamID64.CompareTo(other.SteamID64);
    }

    /// <summary>
    /// Format: 
    /// A (anonymous user)
    /// S (anonymous server)
    /// I:3040000 (from accountid, assume default universe)
    /// F:765000000000000 (full set)
    /// </summary>
    /// <param name="dbgStr"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
	private static CSteamID InterfaceDebuggerSupport(string dbgStr) {
        var steamid = new CSteamID();
		switch (dbgStr[0])
		{
			case 'I':
                return new CSteamID(uint.Parse(dbgStr[1..], CultureInfo.InvariantCulture.NumberFormat), EUniverse.Public, EAccountType.Individual);
			case 'F':
				return new CSteamID(ulong.Parse(dbgStr[1..], CultureInfo.InvariantCulture.NumberFormat));
			case 'A':
                steamid.CreateBlankAnonUserLogon(EUniverse.Public);
                return steamid;
            case 'S':
                steamid.CreateBlankAnonGSLogon(EUniverse.Public);
                return steamid;
        }

        throw new ArgumentOutOfRangeException(nameof(dbgStr), "unknown dbg string type");
    }
}