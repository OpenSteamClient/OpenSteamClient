.globl stdcall_func_GetAppIDForGameID
.type stdcall_func_GetAppIDForGameID, @function

stdcall_func_GetAppIDForGameID:
    PUSH %rsi;
    PUSH %rdx;
    CALL *%rdi;