using System;
using System.Runtime.InteropServices;
using OpenSteamworks.Generated;

namespace OpenSteamworks;
public class Client
{
    Native.ClientNative native;
    public Client()
    {
        this.native = new Native.ClientNative("/home/onni/.steam/steam/linux64/steamclient.so");
        var iface = this.native.CreateInterface<IClientEngine>("CLIENTENGINE_INTERFACE_VERSION005", out int retCode);
        if (iface == null || retCode != 0) {
            throw new Exception("Initializing SteamClient failed.");
        }

        var pipe = iface.CreateSteamPipe();
        Console.WriteLine("pipe: " + pipe);

        var user = iface.ConnectToGlobalUser(pipe);
        Console.WriteLine("user: " + user);

        var clientUser = iface.GetIClientUser<IClientUser>(user, pipe);
        Console.WriteLine("Logged on: " + clientUser.BConnected());

        unsafe {
            uint bufSize = 256;
            void* ptr = NativeMemory.AllocZeroed(bufSize);
            clientUser.GetAccountName((IntPtr)ptr, bufSize);
            Console.WriteLine(Marshal.PtrToStringAuto((IntPtr)ptr));
            NativeMemory.Free(ptr);
        }
        
    }
}