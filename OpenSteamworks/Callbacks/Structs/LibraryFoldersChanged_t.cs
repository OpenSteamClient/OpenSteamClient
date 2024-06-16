using System.Runtime.InteropServices;

namespace OpenSteamworks.Callbacks.Structs;

[StructLayout(LayoutKind.Sequential, Pack = SteamClient.Pack)]
public struct LibraryFoldersChanged_t {
    public int libraryFolder;
}