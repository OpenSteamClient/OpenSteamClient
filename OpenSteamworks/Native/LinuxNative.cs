using System;
using System.Runtime.InteropServices;

// typedef uint16_t Elf64_Half;
// typedef int16_t Elf64_SHalf;
// typedef uint32_t Elf64_Word;
// typedef int32_t Elf64_Sword;
// typedef uint64_t Elf64_Xword;
// typedef int64_t Elf64_Sxword;

// typedef uint64_t Elf64_Off;
// typedef uint64_t Elf64_Addr;
// typedef uint16_t Elf64_Section;


using Elf64_Half = System.UInt16;
using Elf64_SHalf = System.Int16;
using Elf64_Word = System.UInt32;
using Elf64_Sword = System.Int32;
using Elf64_Xword = System.UInt64;
using Elf64_Sxword = System.Int64;

using Elf64_Off = System.UInt64;
using Elf64_Addr = System.UInt64;
using Elf64_Section = System.UInt16;
using System.Runtime.Versioning;

namespace OpenSteamworks.Native;

[SupportedOSPlatform("linux")]
internal static class LinuxNative {
    /// <summary>
    /// Page can be read.
    /// </summary>
    public const int PROT_READ = 0x1;   
    /// <summary>
    /// Page can be written.
    /// </summary>
    public const int PROT_WRITE	= 0x2;
    /// <summary>
    /// Page can be executed.
    /// </summary>
    public const int PROT_EXEC = 0x4;
    /// <summary>
    /// Page can not be accessed.
    /// </summary>
    public const int PROT_NONE = 0x0;
    /// <summary>
    /// Treat ARG as `struct link_map **'; 
    /// store the `struct link_map *' for HANDLE there.
    /// </summary>
    public const int RTLD_DI_LINKMAP = 2;

    [DllImport("libc", SetLastError = true)]
    public static extern int setenv([MarshalAs(UnmanagedType.LPUTF8Str)] string name, [MarshalAs(UnmanagedType.LPUTF8Str)] string value, int overwrite);

    [DllImport("libc")]
    public static unsafe extern int dladdr(IntPtr handle, Dl_info* info);

    [DllImport("libc")]
    public static unsafe extern int dlinfo(IntPtr handle, int request, void* info);

    [DllImport("libc")]
    public static unsafe extern int dl_iterate_phdr(void* callback, void* data);

    [DllImport("libc", SetLastError = true)]
    public static extern int mprotect(IntPtr addr, UIntPtr size, int prot);

    [DllImport("libc")]
    public static extern string dlerror();
}

[StructLayout(LayoutKind.Sequential)]
internal unsafe struct Dl_info
{
  /// <summary>
  /// File name of defining object.
  /// </summary>
  public void *dli_fname;	
  /// <summary>
  /// Load address of that object.
  /// </summary>
  public void *dli_fbase;
  /// <summary>
  /// Name of nearest symbol.
  /// </summary>
  public void *dli_sname;
  /// <summary>
  /// Exact value of nearest symbol.
  /// </summary>
  public void* dli_saddr;
}

[StructLayout(LayoutKind.Sequential)]
internal unsafe struct link_map
{
  /// <summary>
  /// Difference between the address in the ELF file and the addresses in memory.
  /// </summary>
  public UInt64 l_addr;    
  /// <summary>
  /// Absolute file name object was found in.
  /// </summary>
  public void *l_name;    

  /// <summary>
  /// Dynamic section of the shared object.
  /// </summary>   
  public Elf64_Dyn *l_ld;
  public link_map* l_next;
  public link_map* l_prev;
};

[StructLayout(LayoutKind.Sequential)]
internal unsafe struct Elf64_Dyn {
  public Int64 d_tag;
	public UInt64 d_ptr;
};

[StructLayout(LayoutKind.Sequential)]
internal unsafe struct dl_phdr_info {
  /// <summary>
  /// Base address of object
  /// </summary>
  public UInt64        dlpi_addr;
  /// <summary>
  /// (Null-terminated) name of object
  /// </summary>
  public void *dlpi_name;
  /// <summary>
  /// Pointer to array of ELF program headers for this object
  /// </summary>
  public Elf64_Phdr *dlpi_phdr;
  /// <summary>
  /// # of items in dlpi_phdr
  /// </summary>
  public  UInt16        dlpi_phnum;
};

[StructLayout(LayoutKind.Sequential)]
internal unsafe struct Elf64_Phdr {
    public UInt32 p_type;
    public UInt32 p_flags;
    public UInt64 p_offset;
    public UInt64 p_vaddr;
    public UInt64 p_paddr;
    public UInt64 p_filesz;
    public UInt64 p_memsz;
    public UInt64 p_align;
};

[StructLayout(LayoutKind.Sequential)]
internal unsafe struct Elf64_Ehdr {
  public fixed byte e_ident[16];
  public UInt16 e_type;
  public UInt16 e_machine;
  public UInt32 e_version;
  public UInt64 e_entry;
  public UInt64 e_phoff;
  public UInt64 e_shoff;
  public UInt32 e_flags;
  public UInt16 e_ehsize;
  public UInt16 e_phentsize;
  public UInt16 e_phnum;
  public UInt16 e_shentsize;
  public UInt16 e_shnum;
  public UInt16 e_shstrndx;
};

[StructLayout(LayoutKind.Sequential)]
internal unsafe struct Elf64_Shdr {
  public Elf64_Word sh_name;
  public Elf64_Word sh_type;
  public Elf64_Xword sh_flags;
  public Elf64_Addr sh_addr;
  public Elf64_Off sh_offset;
  public Elf64_Xword sh_size;
  public Elf64_Word sh_link;
  public Elf64_Word sh_info;
  public Elf64_Xword sh_addralign;
  public Elf64_Xword sh_entsize;
};