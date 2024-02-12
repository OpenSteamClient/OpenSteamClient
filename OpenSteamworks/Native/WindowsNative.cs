using System;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;

namespace OpenSteamworks.Native;

using SIZE_T = System.UIntPtr;
using unsafe PVOID = void*;
using ULONGLONG = System.UInt64;
using BYTE = System.Byte;

[SupportedOSPlatform("windows")]
internal static class WindowsNative {
    public const uint PAGE_READWRITE = 0x04;
    /// <summary>
    /// The section can be executed as code. 
    /// </summary>
    public const uint IMAGE_SCN_MEM_EXECUTE = 0x20000000;


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

[StructLayout(LayoutKind.Sequential)]
internal unsafe struct IMAGE_DOS_HEADER {
  public WORD  e_magic;      /* 00: MZ Header signature */
  public WORD  e_cblp;       /* 02: Bytes on last page of file */
  public WORD  e_cp;         /* 04: Pages in file */
  public WORD  e_crlc;       /* 06: Relocations */
  public WORD  e_cparhdr;    /* 08: Size of header in paragraphs */
  public WORD  e_minalloc;   /* 0a: Minimum extra paragraphs needed */
  public WORD  e_maxalloc;   /* 0c: Maximum extra paragraphs needed */
  public WORD  e_ss;         /* 0e: Initial (relative) SS value */
  public WORD  e_sp;         /* 10: Initial SP value */
  public WORD  e_csum;       /* 12: Checksum */
  public WORD  e_ip;         /* 14: Initial IP value */
  public WORD  e_cs;         /* 16: Initial (relative) CS value */
  public WORD  e_lfarlc;     /* 18: File address of relocation table */
  public WORD  e_ovno;       /* 1a: Overlay number */
  public fixed WORD  e_res[4];     /* 1c: Reserved words */
  public WORD  e_oemid;      /* 24: OEM identifier (for e_oeminfo) */
  public WORD  e_oeminfo;    /* 26: OEM information; e_oemid specific */
  public fixed WORD  e_res2[10];   /* 28: Reserved words */
  public DWORD e_lfanew;     /* 3c: Offset to extended header */
}

[StructLayout(LayoutKind.Sequential)]
internal unsafe struct IMAGE_DATA_DIRECTORY {
  public DWORD VirtualAddress;
  public DWORD Size;
}

[StructLayout(LayoutKind.Sequential)]
internal unsafe struct IMAGE_OPTIONAL_HEADER64 {
  public WORD  Magic; /* 0x20b */
  public BYTE MajorLinkerVersion;
  public BYTE MinorLinkerVersion;
  public DWORD SizeOfCode;
  public DWORD SizeOfInitializedData;
  public DWORD SizeOfUninitializedData;
  public DWORD AddressOfEntryPoint;
  public DWORD BaseOfCode;
  public ULONGLONG ImageBase;
  public DWORD SectionAlignment;
  public DWORD FileAlignment;
  public WORD MajorOperatingSystemVersion;
  public WORD MinorOperatingSystemVersion;
  public WORD MajorImageVersion;
  public WORD MinorImageVersion;
  public WORD MajorSubsystemVersion;
  public WORD MinorSubsystemVersion;
  public DWORD Win32VersionValue;
  public DWORD SizeOfImage;
  public DWORD SizeOfHeaders;
  public DWORD CheckSum;
  public WORD Subsystem;
  public WORD DllCharacteristics;
  public ULONGLONG SizeOfStackReserve;
  public ULONGLONG SizeOfStackCommit;
  public ULONGLONG SizeOfHeapReserve;
  public ULONGLONG SizeOfHeapCommit;
  public DWORD LoaderFlags;
  public DWORD NumberOfRvaAndSizes;
  public IMAGE_DATA_DIRECTORY ExportTable;
  public IMAGE_DATA_DIRECTORY ImportTable;
  public IMAGE_DATA_DIRECTORY ResourceTable;
  public IMAGE_DATA_DIRECTORY ExceptionTable;
  public IMAGE_DATA_DIRECTORY CertificateTable;
  public IMAGE_DATA_DIRECTORY BaseRelocationTable;
  public IMAGE_DATA_DIRECTORY Debug;
  public IMAGE_DATA_DIRECTORY Architecture;
  public IMAGE_DATA_DIRECTORY GlobalPtr;
  public IMAGE_DATA_DIRECTORY TLSTable;
  public IMAGE_DATA_DIRECTORY LoadConfigTable;
  public IMAGE_DATA_DIRECTORY BoundImport;
  public IMAGE_DATA_DIRECTORY IAT;
  public IMAGE_DATA_DIRECTORY DelayImportDescriptor;
  public IMAGE_DATA_DIRECTORY CLRRuntimeHeader;
  public IMAGE_DATA_DIRECTORY Reserved;
}

[StructLayout(LayoutKind.Sequential)]
internal unsafe struct IMAGE_NT_HEADERS64 {
  public DWORD Signature;
  public IMAGE_FILE_HEADER FileHeader;
  public IMAGE_OPTIONAL_HEADER64 OptionalHeader;
}

[StructLayout(LayoutKind.Sequential)]
internal unsafe struct IMAGE_OPTIONAL_HEADER32 {

  /* Standard fields */

  public WORD  Magic; /* 0x10b or 0x107 */	/* 0x00 */
  public BYTE  MajorLinkerVersion;
  public BYTE  MinorLinkerVersion;
  public DWORD SizeOfCode;
  public DWORD SizeOfInitializedData;
  public DWORD SizeOfUninitializedData;
  public DWORD AddressOfEntryPoint;		/* 0x10 */
  public DWORD BaseOfCode;
  public DWORD BaseOfData;

  /* NT additional fields */

  public DWORD ImageBase;
  public DWORD SectionAlignment;		/* 0x20 */
  public DWORD FileAlignment;
  public WORD  MajorOperatingSystemVersion;
  public WORD  MinorOperatingSystemVersion;
  public WORD  MajorImageVersion;
  public WORD  MinorImageVersion;
  public WORD  MajorSubsystemVersion;		/* 0x30 */
  public WORD  MinorSubsystemVersion;
  public DWORD Win32VersionValue;
  public DWORD SizeOfImage;
  public DWORD SizeOfHeaders;
  public DWORD CheckSum;			/* 0x40 */
  public WORD  Subsystem;
  public WORD  DllCharacteristics;
  public DWORD SizeOfStackReserve;
  public DWORD SizeOfStackCommit;
  public DWORD SizeOfHeapReserve;		/* 0x50 */
  public DWORD SizeOfHeapCommit;
  public DWORD LoaderFlags;
  public DWORD NumberOfRvaAndSizes;

  public IMAGE_DATA_DIRECTORY ExportTable;
  public IMAGE_DATA_DIRECTORY ImportTable;
  public IMAGE_DATA_DIRECTORY ResourceTable;
  public IMAGE_DATA_DIRECTORY ExceptionTable;
  public IMAGE_DATA_DIRECTORY CertificateTable;
  public IMAGE_DATA_DIRECTORY BaseRelocationTable;
  public IMAGE_DATA_DIRECTORY Debug;
  public IMAGE_DATA_DIRECTORY Architecture;
  public IMAGE_DATA_DIRECTORY GlobalPtr;
  public IMAGE_DATA_DIRECTORY TLSTable;
  public IMAGE_DATA_DIRECTORY LoadConfigTable;
  public IMAGE_DATA_DIRECTORY BoundImport;
  public IMAGE_DATA_DIRECTORY IAT;
  public IMAGE_DATA_DIRECTORY DelayImportDescriptor;
  public IMAGE_DATA_DIRECTORY CLRRuntimeHeader;
  public IMAGE_DATA_DIRECTORY Reserved;
}

[StructLayout(LayoutKind.Sequential)]
internal unsafe struct IMAGE_NT_HEADERS {
  public DWORD Signature; /* "PE"\0\0 */	/* 0x00 */
  public IMAGE_FILE_HEADER FileHeader;		/* 0x04 */
  public IMAGE_OPTIONAL_HEADER32 OptionalHeader;	/* 0x18 */
}

[StructLayout(LayoutKind.Sequential)]
internal unsafe struct IMAGE_FILE_HEADER {
  public WORD  Machine;
  public WORD  NumberOfSections;
  public DWORD TimeDateStamp;
  public DWORD PointerToSymbolTable;
  public DWORD NumberOfSymbols;
  public WORD  SizeOfOptionalHeader;
  public WORD  Characteristics;
}

[StructLayout(LayoutKind.Sequential)]
internal unsafe struct IMAGE_SECTION_HEADER {
  public fixed byte Name[8];
  public DWORD PhysicalAddressOrVirtualSize;
  public DWORD VirtualAddress;
  public DWORD SizeOfRawData;
  public DWORD PointerToRawData;
  public DWORD PointerToRelocations;
  public DWORD PointerToLinenumbers;
  public WORD  NumberOfRelocations;
  public WORD  NumberOfLinenumbers;
  public DWORD Characteristics;
}