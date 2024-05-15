using System.Diagnostics;
using OpenSteamworks;
using OpenSteamworks.Structs;

public class Program
{
    public static void Main(string[] args)
    {
        Console.WriteLine("Start dumping callbacks");

        // Replace the DLL path here to a place where the correctly versioned steamclient is stored.
        var client = new SteamClient("C:\\Program Files (x86)\\Steam\\steamclient64.dll", ConnectionType.ExistingClient, true, true);
        client.IClientSharedConnection.AllocateSharedConnection();

        Stopwatch bGetCallback = new();
        Stopwatch ipcCallback = new();
        int currentCB = 0;

        var hanlde = client.IClientNetworkingSocketsSerialized.GetCertAsync();
        while (true)
        {
            bGetCallback.Reset();
            ipcCallback.Reset();

            client.IClientEngine.RunFrame();

            //ipcCallback.Start();
            // if (client.IPCClient.BGetCallback(out CallbackMsg_t ipcCB)) {
            //     ipcCallback.Stop();
            //     Console.WriteLine("IPCClient callback " + ipcCB.callbackID + ", user: " + ipcCB.steamUser + ", len: " + ipcCB.callbackData.Length + " in " + ipcCallback.ElapsedMilliseconds + "ms");
            // }


            bGetCallback.Start();
            if (client.BGetCallback(out CallbackMsg_t cb)) {
                bGetCallback.Stop();
                Console.WriteLine("Got callback " + cb.callbackID + ", user: " + cb.steamUser + ", len: " + cb.callbackData.Length + " in: " + bGetCallback.ElapsedMilliseconds + "ms");
                if (cb.callbackID == 703) {
                    if (client.IClientUtils.IsAPICallCompleted(hanlde, out bool failed) && !failed) {
                        
                        byte[] buf = new byte[4096 * 8];
                        unsafe
                        {
                            fixed (byte* ptr = buf) {
                                if (client.IClientUtils.GetAPICallResult(hanlde, (void*)ptr, buf.Length, 1296, out bool failed2) && !failed2) {
                                    File.WriteAllBytes("certdatacb.bin", buf);
                                    Console.WriteLine("Got apicall data, exiting");
                                    return;
                                } else {
                                    Console.WriteLine("Failed2: " + client.IClientUtils.GetAPICallFailureReason(hanlde));
                                }
                            }
                        }
                        
                    } else {
                        if (failed) {
                            Console.WriteLine("Failed: " + client.IClientUtils.GetAPICallFailureReason(hanlde));
                        }
                    }
                }

                currentCB++;
                File.WriteAllBytes($"{currentCB}_{cb.callbackID}.bin", cb.callbackData);
                client.FreeLastCallback();
            } else {
                //Console.WriteLine("No cb");
            }

            System.Threading.Thread.Sleep(2);
        }
    }
}
