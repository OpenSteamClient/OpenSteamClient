using System;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;

namespace OpenSteamworks.Native;

using SIZE_T = System.UIntPtr;
using DWORD = System.UInt32;
using WORD = System.UInt16;
//TODO: once we have net8.0, we can typedef void*
using PVOID = System.IntPtr;

[SupportedOSPlatform("windows")]
internal static class WindowsNative {
    public const uint PAGE_READWRITE = 0x04;

    [DllImport("kernel32.dll", SetLastError = true)]
    public static unsafe extern bool VirtualProtect(void* lpAddress, SIZE_T dwSize, uint flNewProtect, uint* lpflOldProtect);

    [DllImport("kernel32.dll", SetLastError = true)]
    public static unsafe extern SIZE_T VirtualQuery(void* lpAddress, MEMORY_BASIC_INFORMATION* lpBuffer, SIZE_T dwLength);
}

[StructLayout(LayoutKind.Sequential)]
internal unsafe struct MEMORY_BASIC_INFORMATION {
  public PVOID  BaseAddress;
  public PVOID  AllocationBase;
  public DWORD  AllocationProtect;
  public WORD   PartitionId;
  public SIZE_T RegionSize;
  public DWORD  State;
  public DWORD  Protect;
  public DWORD  Type;
}