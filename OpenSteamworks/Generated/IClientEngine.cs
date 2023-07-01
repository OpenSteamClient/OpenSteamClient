using System;
using OpenSteamworks.Enums;
using OpenSteamworks.Native;

namespace OpenSteamworks.Generated;

public interface IClientEngine {
    public Int32 CreateSteamPipe();
	public bool BReleaseSteamPipe( Int32 hSteamPipe );
	public Int32 CreateGlobalUser( ref Int32 phSteamPipe );
	public Int32 ConnectToGlobalUser( Int32 hSteamPipe );
	public Int32 CreateLocalUser( ref Int32 phSteamPipe, EAccountType eAccountType );
	public void CreatePipeToLocalUser( Int32 hSteamUser, ref Int32 phSteamPipe );
	public void ReleaseUser( Int32 hSteamPipe, Int32 hUser );
    public bool IsValidHSteamUserPipe(Int32 hSteamPipe, Int32 hUser);
	public TClass GetIClientUser<TClass>(Int32 hSteamUser, Int32 hSteamPipe) where TClass : class;
}