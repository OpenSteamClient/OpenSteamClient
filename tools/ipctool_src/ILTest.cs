using System;
using System.Text;

public class TestClass {
    private uint steamuser;
    private IPCClient client;
    public bool SetLanguage(string language) {
        return client.CallIPCFunction<bool>(this.steamuser, 1, 1453699815, 1455458003, new object[] { language });
    }

    public int GetAccountName(StringBuilder nameOut, int nameMax) {
        return client.CallIPCFunction<int>(this.steamuser, 1, 1453699815, 1455458003, new object[] { nameOut, nameMax });
    }

    public bool BLoggedOn() {
        return client.CallIPCFunction<bool>(this.steamuser, 1, 1453699815, 1455458003, Array.Empty<object>());
    }

    public int GetSubscribedApps(uint[] arr, uint lengthOfArr, bool unk) {
        return client.CallIPCFunction<int>(this.steamuser, 1, 1453699815, 1455458003, new object[] { arr, lengthOfArr, unk });
    }
}