using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using OpenSteamworks.Utils;

namespace OpenSteamworks.ConCommands;

using CVarDLLIdentifier_t = System.Int32;

[Flags]
public enum ConCommandFlags : uint
{
    FCVAR_NEVER_AS_STRING = 1 << 0,
    UNK1 = 1 << 1,
    UNK2 = 1 << 2,
    UNK3 = 1 << 3,
    UNK4 = 1 << 4,
};

public unsafe interface ICommandCompletionCallback
{
	public int CommandCompletionCallback(string pPartial, CUtlStringList* commands);
};
 
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public unsafe struct ConCommandBase
{
    public void* vtable;
    public ConCommandBase* m_pNext; 
    public UInt64 m_bRegistered;
    public IntPtr m_pszName;
    public IntPtr m_pszHelpString; 
    public ConCommandFlags m_nFlags; 
    public uint unk;
    public void* pCommandCallback;
    public UInt64 unk1;
    public void* completionCallback;
    public UInt64 hasCompletionCallback;
    public UInt64 usingNewCommandCallback;
    public UInt32 usingCommandCallbackInterface;
    public byte padding;
    public byte padding2;
    public byte padding3;
    public byte padding4;    
    public byte padding5;
    public void* unknownPointer;
};

public unsafe struct CCommand_Funcs {
    public CCommand_Funcs(
    delegate* unmanaged[Thiscall]<CCommand*, byte*, int, characterset_t*, byte> tokenize, 
    delegate* unmanaged[Thiscall]<CCommand*, void> reset
    // delegate* unmanaged[Thiscall]<CCommand*, int> argc, 
    // delegate* unmanaged[Thiscall]<CCommand*, byte**> argv, 
    // delegate* unmanaged[Thiscall]<CCommand*, byte*> args,
    // delegate* unmanaged[Thiscall]<CCommand*, byte*> getCommandString,
    // delegate* unmanaged[Thiscall]<CCommand*, int, byte*> arg,
    // delegate* unmanaged[Thiscall]<CCommand*, int> source,
    // delegate* unmanaged[Thiscall]<CCommand*, byte*, byte*> findArg,
    // delegate* unmanaged[Thiscall]<CCommand*, byte*, int, int> findArgInt
    ) {
        this.tokenize = tokenize;
        this.reset = reset;
        // this.argc = argc;
        // this.argv = argv;
        // this.args = args;
        // this.getCommandString = getCommandString;
        // this.arrayoperator = arg;
        // this.arg = arg;
        // this.source = source;
        // this.findArg = findArg;
        // this.findArgInt = findArgInt;
    }

    public delegate* unmanaged[Thiscall]<CCommand*, byte*, int, characterset_t*, byte> tokenize;
    public delegate* unmanaged[Thiscall]<CCommand*, void> reset;
    // public delegate* unmanaged[Thiscall]<CCommand*, int> argc;
    // public delegate* unmanaged[Thiscall]<CCommand*, byte**> argv;
    // public delegate* unmanaged[Thiscall]<CCommand*, byte*> args;
    // public delegate* unmanaged[Thiscall]<CCommand*, byte*> getCommandString;
    // public delegate* unmanaged[Thiscall]<CCommand*, int, byte*> arrayoperator;
    // public delegate* unmanaged[Thiscall]<CCommand*, int, byte*> arg;
    // public delegate* unmanaged[Thiscall]<CCommand*, int> source;
    // public delegate* unmanaged[Thiscall]<CCommand*, byte*, byte*> findArg;
    // public delegate* unmanaged[Thiscall]<CCommand*, byte*, int, int> findArgInt;
    // bool Tokenize( const char *pCommand, cmd_source_t source = kCommandSrcCode, characterset_t *pBreakSet = NULL );
    // void Reset();

    // int ArgC() const;
    // const char **ArgV() const;
    // const char *ArgS() const;					// All args that occur after the 0th arg, in string form
    // const char *GetCommandString() const;		// The entire command in string form, including the 0th arg
    // const char *operator[]( int nIndex ) const;	// Gets at arguments
    // const char *Arg( int nIndex ) const;		// Gets at arguments
    // cmd_source_t Source() const;				// Find where this command was sent from

    // // Helper functions to parse arguments to commands.
    // const char* FindArg( const char *pName ) const;
    // int FindArgInt( const char *pName, int nDefaultVal ) const;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe struct CCommand
{
    static CCommand() {
        s_vtable = new CCommand_Funcs(&Tokenize, &Reset);
        // Is this bad?
        GCHandle handle = GCHandle.Alloc(s_vtable, GCHandleType.Pinned);
        s_pVtable = (CCommand_Funcs*)handle.AddrOfPinnedObject();
    }

    public const int COMMAND_MAX_LENGTH = 512;
    public const int COMMAND_MAX_ARGC = 64;

    private static characterset_t* defaultCharSet = characterset_t.AllocateCharset(new byte[] {(byte)'{', (byte)'}', (byte)'(', (byte)')', (byte)'\'', (byte)':'});
    private static CCommand_Funcs s_vtable;
    private static CCommand_Funcs* s_pVtable;
    public CCommand_Funcs* vtable = null;
    public int		m_nArgc;
	public int		m_nArgv0Size;
    // char*
	public fixed byte m_pArgSBuffer[COMMAND_MAX_LENGTH];
    // char*
    public fixed byte m_pArgvBuffer[COMMAND_MAX_LENGTH];
    // char**
	public fixed ulong m_ppArgv[COMMAND_MAX_ARGC];
	public int m_source = 0;

    public CCommand(string fullCmd)
    {
        this.vtable = s_pVtable;
        unsafe
        {
            fixed (byte* chars = Encoding.UTF8.GetBytes(fullCmd + "\0")) {
                fixed (CCommand* thiz = &this) {
                    this.vtable->tokenize(thiz, chars, 0, null);
                }
            }
        }
    }

    [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvThiscall) })]
    public static unsafe byte Tokenize(CCommand* thiz, byte* _pCommand, int source, characterset_t* charset)
    {
        string? pCommand = Marshal.PtrToStringUTF8((nint)_pCommand);

        Logging.ConCommandsLogger.Info("Tokenize called with string " + pCommand);
        
        if (charset == null) {
            charset = defaultCharSet;
        }

        thiz->vtable->reset(thiz);
        thiz->m_source = source;

        if (pCommand == null) {
            Logging.ConCommandsLogger.Error("Tokenize: pCommand is null");
            return 0;
        }

        if (pCommand.Length > COMMAND_MAX_LENGTH-1) {
            Logging.ConCommandsLogger.Error("Tokenize: Encountered command which overflows the tokenizer buffer.. Skipping!");
            return 0;
        }

        NativeMemory.Copy(_pCommand, thiz->m_pArgSBuffer, (nuint)(pCommand.Length + 1));

        CUtlBuffer bufParse = new((nint)thiz->m_pArgSBuffer, pCommand.Length, CUtlBuffer.BufferFlags_t.TEXT_BUFFER | CUtlBuffer.BufferFlags_t.READ_ONLY ); 
        int nArgvBufferSize = 0;
        while ( bufParse.IsValid() && ( thiz->m_nArgc < COMMAND_MAX_ARGC ) )
        {
            byte *pArgvBuf = &thiz->m_pArgvBuffer[nArgvBufferSize];
            int nMaxLen = COMMAND_MAX_LENGTH - nArgvBufferSize;
            int nStartGet = bufParse.TellGet();
            int nSize = bufParse.ParseToken(charset, pArgvBuf, nMaxLen, false);
            if ( nSize < 0 )
                break;

            // Check for overflow condition
            if ( nMaxLen == nSize )
            {
                thiz->vtable->reset(thiz);
                return 0;
            }

            if ( thiz->m_nArgc == 1 )
            {
                // Deal with the case where the arguments were quoted
                thiz->m_nArgv0Size = bufParse.TellGet();
                bool bFoundEndQuote = thiz->m_pArgSBuffer[thiz->m_nArgv0Size-1] == '\"';
                if ( bFoundEndQuote )
                {
                    thiz->m_nArgv0Size--;
                }
                thiz->m_nArgv0Size -= nSize;
                UtilityFunctions.Assert( thiz->m_nArgv0Size != 0 );

                // The StartGet check is to handle this case: "foo"bar
                // which will parse into 2 different args. ArgS should point to bar.
                bool bFoundStartQuote = ( thiz->m_nArgv0Size > nStartGet ) && ( thiz->m_pArgSBuffer[thiz->m_nArgv0Size-1] == '\"' );
                UtilityFunctions.Assert( bFoundEndQuote == bFoundStartQuote );
                if ( bFoundStartQuote )
                {
                    thiz->m_nArgv0Size--;
                }
            }

            thiz->m_ppArgv[thiz->m_nArgc++] = (ulong)pArgvBuf;
            if(thiz->m_nArgc >= COMMAND_MAX_ARGC)
            {
                Logging.ConCommandsLogger.Warning( "CCommand::Tokenize: Encountered command which overflows the argument buffer.. Clamped!\n" );
            }

            nArgvBufferSize += nSize + 1;
            UtilityFunctions.Assert( nArgvBufferSize <= COMMAND_MAX_LENGTH );
        }

        return 1;
    }

    [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvThiscall) })]
    public static unsafe void Reset(CCommand* thiz)
    {
        thiz->m_nArgc = 0;
        thiz->m_nArgv0Size = 0;
        thiz->m_pArgSBuffer[0] = 0;
        thiz->m_source = -1;
    }

    // [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvThiscall) })]
    // public static unsafe int ArgC(CCommand* thiz)
    // {
    //     return thiz->m_nArgc;
    // }

    // [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvThiscall) })]
    // public static unsafe byte** ArgV(CCommand* thiz)
    // {
    //     return thiz->m_nArgc != 0 ? (byte**)thiz->m_ppArgv : null;
    // }
}

//NOTE: Since we don't support inheritance in JITEngine, make sure all inheriting classes contain the same functions!
public unsafe interface ConCommandBase_Funcs {
    public void Destructor1();
    public void Destructor2();
    public unknown_ret Unk();
    public bool IsFlagSet(int flag);
    //public bool IsCommand2();
    //public bool IsCommand3();
    public void AddFlags(int flags);
    public void RemoveFlags(int flags);
    public bool IsCommand4();
    public bool IsCommand5();
    public bool IsCommand6();
    // public bool IsFlagSet(int flag);
    // public void AddFlags(int flags);
    // public void RemoveFlags(int flags);
    // public int GetFlags();
    // public string GetName();
    // public string? GetHelpText();
    public bool IsCommand();
    public ConCommandBase* GetNext();
    public bool ReturnsABool();
    public CVarDLLIdentifier_t GetDLLIdentifier();
    public void Create(string name, string? helpString = null, int flags = 0);
    public void Init();
    public void Init1();
    public void Init2();
    public void Init3();
    public void Init4();
    public bool CanAutoComplete();
    public int AutoCompleteSuggest(string partial, CUtlStringList* commands);
    public void Dispatch(CCommand* command1, CCommand* command);
}

public unsafe interface ConCommand_Funcs {
    public void Destructor();
    public void Destructor1();
    public bool IsCommand();
    public bool IsFlagSet(int flag);
    public void AddFlags(int flags);
    public void RemoveFlags(int flags);
    public int GetFlags();
    public string GetName();
    public string? GetHelpText();
    public bool IsRegistered();
    public ConCommandBase* GetNext();
    public bool ReturnsABool();
    public CVarDLLIdentifier_t GetDLLIdentifier();
    public void Create(string name, string? helpString = null, int flags = 0);
    public void Init();
    public bool IsCommand2();
    public int AutoCompleteSuggest(string partial, CUtlStringList* commands);
    public bool CanAutoComplete();
    public void Dispatch(CCommand* command);
}

public unsafe interface ConVar_Funcs {
    public void Destructor();
    public void Destructor1();
    public bool IsCommand();
    public bool IsFlagSet(int flag);
    public void AddFlags(int flags);
    public void RemoveFlags(int flags);
    public int GetFlags();
    public string GetName();
    public string? GetHelpText();
    public bool IsRegistered();
    public ConCommandBase* GetNext();
    public bool ReturnsABool();
    public CVarDLLIdentifier_t GetDLLIdentifier();
    public void Create(string name, string? helpString = null, int flags = 0);
    public void Init();
}