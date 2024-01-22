using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using OpenSteamworks;
using OpenSteamworks.Utils;

[StructLayout(LayoutKind.Sequential)]
public unsafe struct CUtlBuffer {
    public CUtlMemory<UInt8> m_Memory;
	public int m_Get;
	public int m_Put;

	public int m_nMaxPut;
	public UInt16 m_nTab;

	public ErrorFlags_t m_Error;
	public BufferFlags_t m_Flags;

	public delegate* unmanaged[Thiscall]<CUtlBuffer*, int, byte> m_GetOverflowFunc;
    public uint padding = 0;
    public delegate* unmanaged[Thiscall]<CUtlBuffer*, int, byte> m_PutOverflowFunc;
    public uint padding1 = 0;

    public enum ErrorFlags_t : byte 
    {
        PUT_OVERFLOW = 0x1,
        GET_OVERFLOW = 0x2,
        MAX_ERROR_FLAG
    }

    public enum BufferFlags_t : byte
	{
		TEXT_BUFFER = 0x1,			// Describes how get + put work (as strings, or binary)
		EXTERNAL_GROWABLE = 0x2,	// This is used w/ external buffers and causes the utlbuf to switch to reallocatable memory if an overflow happens when Putting.
		CONTAINS_CRLF = 0x4,		// For text buffers only, does this contain \n or \n\r?
		READ_ONLY = 0x8,			// For external buffers; prevents null termination from happening.
		AUTO_TABS_DISABLED = 0x10,	// Used to disable/enable push/pop tabs
		LITTLE_ENDIAN_BUFFER = 0x20,// ensures that data is stored in little endian format
		BIG_ENDIAN_BUFFER = 0x40,	// ensures that data is stored in big endian format
	};

    public enum SeekType_t : byte
	{
		SEEK_HEAD = 0,
		SEEK_CURRENT,
		SEEK_TAIL
	};

    public CUtlBuffer(int length, int growSize = 0) {
        this.m_Memory = new CUtlMemory<UInt8>(growSize, length);
        this.m_Error = 0;
        this.m_Get = 0;
        this.m_Put = 0;
        this.m_nTab = 0;
        this.m_Flags = 0;

        this.m_nMaxPut = -1;

        unsafe {
            this.m_GetOverflowFunc = &GetOverflow;
            this.m_PutOverflowFunc = &PutOverflow;
        }
    }

    public CUtlBuffer(IntPtr pBuffer, int nSize, BufferFlags_t nFlags)
    {
        UtilityFunctions.Assert( nSize != 0 );
        this.m_Memory = new CUtlMemory<UInt8>(pBuffer, nSize);

        m_Error = 0;
        m_Get = 0;
        m_Put = 0;
        m_nTab = 0;
        m_Flags = nFlags;
        if ( IsReadOnly() )
        {
            m_nMaxPut = nSize;
            m_Put = nSize;
        }
        else
        {
            m_nMaxPut = -1;
            AddNullTermination();
        }

        unsafe {
            this.m_GetOverflowFunc = &GetOverflow;
            this.m_PutOverflowFunc = &PutOverflow;
        }
    }

    public void Free() {
        this.m_Memory.Free();
    }

    public byte[] ToManaged()
    {
        byte[] allBytes = this.m_Memory.ToManaged();
        byte[] usedBytes = new byte[this.m_Put];
        Array.Copy(allBytes, usedBytes, this.m_Put);
        return usedBytes;
    }

    public void SeekToBeginning() {
        this.m_Put = 0;
    }

    public byte[] ToManagedAndFree() {
        var str = this.ToManaged();
        this.Free();
        return str;
    }

    [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvThiscall) })]
    public static byte PutOverflow(CUtlBuffer* buf, int nSize) {
        Logging.CUtlLogger.Debug("PutOverflow called");
        int nGrowDelta = (buf->m_Put + nSize) - buf->m_Memory.m_nAllocationCount;

        if (nGrowDelta > 0)
        {
            buf->m_Memory.Grow( nGrowDelta );
        }
            
        return 1;
    }

    [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvThiscall) })]
    public static byte GetOverflow(CUtlBuffer* buf, int nSize) {
        Logging.CUtlLogger.Debug("GetOverflow called");
        return 0;
    }

    public unsafe void Put( void *pMem, int size )
    {
        if ( size > 0 && CheckPut( size ) )
        {
            if ( pMem != &m_Memory.Base()[m_Put] )
                NativeMemory.Copy(&m_Memory.Base()[m_Put], pMem, (nuint)size);
            m_Put += size;

            AddNullTermination();
        }
    }

    public unsafe bool CheckPut(int nSize) {
        UtilityFunctions.Assert( nSize >= 0 );
        if (m_Error.HasFlag(ErrorFlags_t.PUT_OVERFLOW) || IsReadOnly() || nSize < 0 )
            return false;

        UtilityFunctions.Assert( m_Put >= 0 );
        if ( nSize <= m_Memory.NumAllocated() - m_Put )
            return true;

        if ( OnPutOverflow( nSize ) )
            return true;

        m_Error |= ErrorFlags_t.PUT_OVERFLOW;
        return false;
    }

    public readonly unsafe bool IsReadOnly() {
        return (m_Flags & BufferFlags_t.READ_ONLY) != 0; 
    }

    public bool OnPutOverflow( int nSize )
    {
        fixed (CUtlBuffer* thisptr = &this) {
            return m_PutOverflowFunc(thisptr, nSize) == 1;
        }
    }

    public bool OnGetOverflow( int nSize )
    {
        fixed (CUtlBuffer* thisptr = &this) {
            return m_GetOverflowFunc(thisptr, nSize) == 1;
        }
    }

    public void AddNullTermination()
    {
        UtilityFunctions.Assert( m_Put >= 0 );
        if ( m_Put > m_nMaxPut )
        {
            if ( !IsReadOnly() && (!m_Error.HasFlag(ErrorFlags_t.PUT_OVERFLOW)) && IsText()  )
            {
                // Add null termination value
                if ( CheckPut( 1 ) )
                {
                    m_Memory[m_Put] = 0;
                }
                else
                {
                    // Restore the overflow state, it was valid before...
                    m_Error &= ~ErrorFlags_t.PUT_OVERFLOW;
                }
            }
            m_nMaxPut = m_Put;
        }		
    }

    public readonly bool IsText()
    {
        return m_Flags.HasFlag(BufferFlags_t.TEXT_BUFFER);
    }

    public readonly bool IsValid()
    { 
        return m_Error == 0; 
    }

    public void EatWhiteSpace()
    {
        if ( IsText() && IsValid() )
        {
            while ( CheckGet( sizeof(byte) ) )
            {
                if ( !char.IsWhiteSpace((char)*(byte*)PeekGet()))
                    break;
                m_Get += sizeof(byte);
            }
        }
    }

    public readonly int TellMaxPut()
    {
        return m_nMaxPut;
    }

    /// <summary>
    /// Checks if a get is ok
    /// </summary>
    public bool CheckGet( int nSize )
    {
        if ( nSize < 0 )
            return false;

        if (m_Error.HasFlag(ErrorFlags_t.GET_OVERFLOW))
            return false;

        if ( TellMaxPut() < m_Get + nSize )
        {
            m_Error |= ErrorFlags_t.GET_OVERFLOW;
            return false;
        }

        if ( ( m_Get < 0 ) || (	m_Memory.NumAllocated() < m_Get + nSize ) )
        {
            if ( !OnGetOverflow( nSize ) )
            {
                m_Error |= ErrorFlags_t.GET_OVERFLOW;
                return false;
            }
        }

        return true;
    }

    /// <summary>
    /// Eats C++ style comments
    /// </summary>
    public bool EatCPPComment()
    {
        if ( IsText() && IsValid() )
        {
            // If we don't have a a c++ style comment next, we're done
            byte *pPeek = (byte*)PeekGet( 2 * sizeof(byte), 0 );
            if ( pPeek == null || ( pPeek[0] != '/' ) || ( pPeek[1] != '/' ) )
                return false;

            // Deal with c++ style comments
            m_Get += 2;

            // read complete line
            for ( byte c = GetByte(); IsValid(); c = GetByte() )
            {
                if ( c == '\n' )
                    break;
            }
            return true;
        }
        return false;
    }

    /// <summary>
    /// Checks if a peek get is ok
    /// </summary>
    public bool CheckPeekGet(int nOffset, int nSize)
    {
        if (m_Error.HasFlag(ErrorFlags_t.GET_OVERFLOW))
            return false;

        // Checking for peek can't set the overflow flag
        bool bOk = CheckGet( nOffset + nSize );
        m_Error &= ~ErrorFlags_t.GET_OVERFLOW;
        return bOk;
    }

    /// <summary>
    /// Peek part of the butt
    /// </summary>
    public unsafe void* PeekGet( int nMaxSize, int nOffset )
    {
        if ( !CheckPeekGet( nOffset, nMaxSize ) )
            return null;
        return &m_Memory.Base()[m_Get + nOffset];
    }

    public byte GetByte()
    {
        byte c = 0;
        if ( CheckGet( 1 ) ) // sets get overflow error bit on failure
        {
            c = *(byte*) PeekGet();
            m_Get += 1;
        }
        return c;
    }

    public unsafe void* PeekGet()
    {
        return &m_Memory.Base()[ m_Get ];
    }

    /// <summary>
    /// Change where I'm reading
    /// </summary>
    public unsafe bool SeekGet( SeekType_t type, int offset )	
    {
        switch( type )
        {
            case SeekType_t.SEEK_HEAD:						 
                m_Get = offset; 
                break;

            case SeekType_t.SEEK_CURRENT:
                m_Get += offset;
                break;

            case SeekType_t.SEEK_TAIL:
                m_Get = m_nMaxPut - offset;
                break;
        }

        if ( m_Get > m_nMaxPut )
        {
            m_Error |= ErrorFlags_t.GET_OVERFLOW;
            return false;
        }
        else
        {
            m_Error &= ~ErrorFlags_t.GET_OVERFLOW;
            return true;
        }
    }

    public readonly int TellGet() {
        return m_Get;
    }

    public unsafe int ParseToken( characterset_t *pBreaks, byte *pTokenBuf, int nMaxLen, bool bParseComments )
    {
        UtilityFunctions.Assert( nMaxLen > 0 );
        pTokenBuf[0] = 0;

        // skip whitespace + comments
        while ( true )
        {
            if ( !IsValid() )
                return -1;
            EatWhiteSpace();
            if ( bParseComments )
            {
                if ( !EatCPPComment() )	
                    break;
            }
            else
            {
                break;
            }
        }
        
        byte c = GetByte();
        
        // End of buffer
        if ( c == 0 )
            return -1;

        // handle quoted strings specially
        if ( c == '\"' )
        {
            int nLen = 0;
            while( IsValid() )
            {
                c = GetByte();
                if ( c == '\"' || c != 0 )
                {
                    pTokenBuf[nLen] = 0;
                    return nLen;
                }
                pTokenBuf[nLen] = c;
                if ( ++nLen == nMaxLen )
                {
                    pTokenBuf[nLen-1] = 0;
                    return nMaxLen;
                }
            }

            // In this case, we hit the end of the buffer before hitting the end qoute
            pTokenBuf[nLen] = 0;
            return nLen;
        }

        // parse single characters
        if (pBreaks->InCharacterset(c))
        {
            pTokenBuf[0] = c;
            pTokenBuf[1] = 0;
            return 1;
        }

        // parse a regular word
        int nLen2 = 0;
        while ( true )
        {
            pTokenBuf[nLen2] = c;
            if ( ++nLen2 == nMaxLen )
            {
                pTokenBuf[nLen2-1] = 0;
                return nMaxLen;
            }
            c = GetByte();
            if ( !IsValid() )
                break;

            if (pBreaks->InCharacterset(c) || c == '\"' || c <= ' ' )
            {
                SeekGet( SeekType_t.SEEK_CURRENT, -1 );
                break;
            }
        }
        
        pTokenBuf[nLen2] = 0;
        return nLen2;
    }
}