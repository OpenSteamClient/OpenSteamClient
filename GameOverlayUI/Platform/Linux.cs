using System.Runtime.InteropServices;

namespace GameOverlayUI.Platform;

public unsafe static class LinuxConsts {
    public const int SYS_gettid = 186;
    public const int SYS_futex = 202;
    public const int SYS_set_robust_list = 273;
    public const int SYS_get_robust_list = 274;
    public const int FUTEX_WAIT = 0;
    public const int FUTEX_WAKE = 1;

    [DllImport("c", EntryPoint = "syscall", SetLastError = true)]
    public static extern long syscall_futex(long num, uint* uaddr, int futex_op, uint val, timespec* timeout, uint* uaddr2, uint val3);

    [DllImport("c", EntryPoint = "syscall", SetLastError = true)]
    public static extern long syscall_get_robust_list(long num, int pid, robust_list_head** head_ptr, UIntPtr* len_ptr);

    [DllImport("c", EntryPoint = "syscall", SetLastError = true)]
    public static extern long syscall_set_robust_list(long num, robust_list_head* head, UIntPtr len);

    [DllImport("c", EntryPoint = "syscall", SetLastError = true)]
    public static extern long syscall_gettid(long num);
}

public unsafe static class LinuxErrno {
    public const int EWOULDBLOCK = 11;
    public const int EDEADLK = 35;
    public const int EINTR = 4;
    public const int ENOLCK = 37;
    public const int EPERM = 1;
    public const int EOWNERDEAD = 130;
}

public struct timespec {
    public long	tv_sec;		/* seconds */
    public long tv_nsec;	/* nanoseconds */
}

public unsafe struct robust_list {
    public robust_list* next;
}

public unsafe struct robust_list_head {
    public robust_list list;
    public long futex_offset;
    public robust_list* list_op_pending;
}