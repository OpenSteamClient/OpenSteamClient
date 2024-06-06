using System.Runtime.InteropServices;
using GameOverlayUI.IPC;

namespace GameOverlayUI.Platform;

public unsafe static class LinuxFutex {
    public static int OverlayMutexUnlock(OverlayMutex* mutex) {
        return Interlocked.Exchange(ref mutex->ThreadID, 0);
    }

    public static int OverlayMutexLock(OverlayMutex* mutex, int timeoutSeconds) {
        timespec* timeout = null;
        timespec actualTimeout = new();
        if (timeoutSeconds > 0) {
            actualTimeout.tv_sec = timeoutSeconds;
            timeout = &actualTimeout;
        }

        int err = 0;

        robust_list_head* head = null;
        UIntPtr len = 0;
        var ret = LinuxConsts.syscall_get_robust_list(LinuxConsts.SYS_get_robust_list, 0, &head, &len);
        Console.WriteLine("r: " + ret + ", len: " + len);
        if (head == null || len != 24) {
            throw new Exception("Fatal error: futex robust_list not initialized by pthreads");
        }

        if (head->futex_offset != -32) {
            throw new Exception("Fatal error: futex robust_list not pthreads-compatible");
        }

        // mutex->threadID = 0;
        Console.WriteLine("mutex: " + mutex->ThreadID);
        
        head->list_op_pending = &mutex->robust_list;
        int threadid = GetMaskedThreadID();
        int ownerThread = Interlocked.CompareExchange(ref mutex->ThreadID, threadid, 0);
        if (ownerThread == 0) {
            // Fast acquire path
            mutex->ThreadID = threadid;
            mutex->robust_list.next = &head->list;
        } else if (threadid != (ownerThread & 0x1fffffff) || (ownerThread & 0x40000000) != 0) {
            // slow acquire path
            while (ownerThread != 0)
            {
                // Wait for the futex to change so that we can try acquiring again.
                // We don't care about the return value here as we'll just blindly
                // try the reacquire.
                if (linux_futex(&mutex->ThreadID, LinuxConsts.FUTEX_WAIT, threadid, timeout, null, 0) == -1)
                {
                    var errno = Marshal.GetLastWin32Error();
                    if (errno == LinuxErrno.EWOULDBLOCK)
                    {
                        // Futex has already changed state, see if we can acquire.
                    }
                    else if (errno == LinuxErrno.EINTR)
                    {
                        // Ignore signals, loop and wait again.
                    }
                    else
                    {
                        Console.WriteLine("error with lock: " + errno);
                        return errno;
                    }
                }
                
                // Try to acquire again.
                ownerThread = Interlocked.CompareExchange(ref mutex->ThreadID, threadid, ownerThread);
            }

        } else {
            err = LinuxErrno.EDEADLK;
        }

        head->list_op_pending = null;
        return err;
    }

    private static int GetMaskedThreadID() {
        return (int)LinuxConsts.syscall_gettid(LinuxConsts.SYS_gettid) & 0x1fffffff;
    }

    private static int linux_futex(int* uaddr, int op, int val, timespec* timeout, int* uaddr2, int val3)
    {
        return (int)LinuxConsts.syscall_futex(LinuxConsts.SYS_futex, (uint*)uaddr, op, (uint)val, timeout, (uint*)uaddr2, (uint)val3);
    }
}