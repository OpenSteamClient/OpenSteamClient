//====== Copyright (c) 1996-2005, Valve Corporation, All rights reserved. =====
//
// Purpose: 
//
//=============================================================================

#ifndef UTLSTRING_H
#define UTLSTRING_H
#ifdef _WIN32
#pragma once
#endif

#include "miniutl.h"
#include "generichash.h"

class CUtlString;

//
// Maximum number of allowable characters in a CUtlString.
// V_strlen and its ilk are currently coded to return an unsigned int, but
// in many places in the code it gets treated as a signed int
//
const uint k_cchMaxString = 0x7fff0000;


//-----------------------------------------------------------------------------
// Purpose: simple wrapper class around a char *
//			relies on the small-block heap existing for efficient memory allocation
//			as compact as possible, no virtuals or extraneous data
//			to be used primarily to replace of char array buffers
//			tries to match CUtlSymbol interface wherever possible
//-----------------------------------------------------------------------------
class CUtlString
{
public:
	CUtlString();
	CUtlString( const char *pchString );
	CUtlString( CUtlString const &string );
	explicit CUtlString( size_t nPreallocateBytes );
	~CUtlString();

	// operator=
	CUtlString &operator=( CUtlString const &src );
	CUtlString &operator=( const char *pchString );

	// operator==
	bool operator==( CUtlString const &src ) const;
	bool operator==( const char *pchString ) const;
	
	// operator!=
	bool operator!=( CUtlString const &src ) const;
	bool operator!=( const char *pchString ) const;

	// operator </>, performs case sensitive comparison
	bool operator<( const CUtlString &val ) const;
	bool operator<( const char *pchString ) const;
	bool operator>( const CUtlString &val ) const;
	bool operator>( const char *pchString ) const;

	// operator+=
	CUtlString &operator+=( const char *rhs );

	// is valid?
	bool IsValid() const;

	// gets the string
	// never returns NULL, use IsValid() to see if it's never been set
	const char *String() const;
	const char *Get() const { return String(); }
	operator const char *() const { return String(); }

	// returns the string directly (could be NULL)
	// useful for doing inline operations on the string
	char *Access() { return m_pchString; }
	
	// If you want to take ownership of the ptr, you can use this.
	char *DetachRawPtr() { char *psz = m_pchString; m_pchString = NULL; return psz; }

	// append in-place, causing a re-allocation
	void Append( const char *pchAddition );
	void Append( const char *pchAddition, size_t cbLen );

	// append in-place for a single or repeated run of characters
	void AppendChar( char ch ) { Append( &ch, 1 ); }
	void AppendRepeat( char ch, int cCount );

	// sets the string
	void SetValue( const char *pchString );
	void Set( const char *pchString );
	void Clear() { SetValue( NULL ); }
	void SetPtr( char *pchString );
	void Swap( CUtlString &src );

	void ToLower();
	void ToUpper();

	void Wipe();

	// Set directly and don't look for a null terminator in pValue.
	void SetDirect( const char *pValue, size_t nChars );

	// Get the length of the string in characters.
	uint32 Length() const;
	bool IsEmpty() const;

	// Format like sprintf.
	size_t Format( PRINTF_FORMAT_STRING const char *pFormat, ... ) FMTFUNCTION( 2, 3 );

	// format, then append what we crated in the format
	size_t AppendFormat( PRINTF_FORMAT_STRING const char *pFormat, ... ) FMTFUNCTION( 2, 3 );

	// convert bytes to hex string and append
	void AppendHex( const uint8 *pbInput, size_t cubInput, bool bLowercase = true );

	// replace a single character with another, returns hit count
	size_t Replace( char chTarget, char chReplacement );

	// replace a string with another string, returns hit count
	// replacement string might be NULL or "" to remove target substring
	size_t Replace( const char *pstrTarget, const char *pstrReplacement );
	size_t ReplaceCaseless( const char *pstrTarget, const char *pstrReplacement );

	ptrdiff_t IndexOf( const char *pstrTarget ) const;
	bool BEndsWith( const char *pchString ) const;
	bool BEndsWithCaseless( const char *pchString ) const;
	bool BStartsWith( const char *pchString ) const;
	bool BStartsWithCaseless( const char *pchString ) const;

	// remove whitespace from the string; anything that is isspace()
	size_t RemoveWhitespace( );

	// trim whitespace from the beginning and end of the string
	size_t TrimWhitespace();

	// trim whitespace from the end of the string
	size_t TrimTrailingWhitespace();

	void SecureZero();

#ifdef DBGFLAG_VALIDATE
	void Validate( CValidator &validator, const char *pchName ) const;	// validate our internal structures
#endif // DBGFLAG_VALIDATE

	size_t FormatV( const char *pFormat, va_list args );
	size_t VAppendFormat( const char *pFormat, va_list args );
	void Truncate( size_t nChars );

	// Both TruncateUTF8 methods guarantee truncation of the string to a length less-than-or-equal-to the 
	// specified number of bytes or characters.  Both return false and truncate early if invalid UTF8 sequences 
	// are encountered before the cap is reached.
	// As a result, the string is guaranteed to be valid UTF-8 upon completion of the operation.
	bool TruncateUTF8Bytes( size_t unMaxBytes ) { return TruncateUTF8Internal( (size_t)-1, unMaxBytes ); }
	bool TruncateUTF8Chars( size_t unMaxChars ) { return TruncateUTF8Internal( unMaxChars, (size_t)-1 ); }

private:
	bool TruncateUTF8Internal( size_t unMaxChars, size_t unMaxBytes );
	size_t ReplaceInternal( const char *pstrTarget, const char *pstrReplacement, const char *pfnCompare(const char*, const char*) );
	static void AssertStringTooLong();
	char *m_pchString;
};


//-----------------------------------------------------------------------------
// Purpose: constructor
//-----------------------------------------------------------------------------
inline CUtlString::CUtlString() : m_pchString( NULL )
{
}


//-----------------------------------------------------------------------------
// Purpose: constructor
//-----------------------------------------------------------------------------
inline CUtlString::CUtlString( size_t nPreallocateBytes ) 
{
	if ( nPreallocateBytes > 0 )
	{
		if ( nPreallocateBytes > k_cchMaxString )
			AssertStringTooLong();
		m_pchString = (char*) PvAlloc( nPreallocateBytes );
		m_pchString[0] = 0;
	}
	else
	{
		m_pchString = NULL;
	}
}


//-----------------------------------------------------------------------------
// Purpose: constructor
//-----------------------------------------------------------------------------
inline CUtlString::CUtlString( const char *pchString ) : m_pchString( NULL )
{
	SetValue( pchString );
}


//-----------------------------------------------------------------------------
// Purpose: constructor
//-----------------------------------------------------------------------------
inline CUtlString::CUtlString( CUtlString const &string ) : m_pchString( NULL )
{
	SetValue( string.String() );
}


//-----------------------------------------------------------------------------
// Purpose: destructor
//-----------------------------------------------------------------------------
inline CUtlString::~CUtlString()
{
	FreePv( m_pchString );
}


//-----------------------------------------------------------------------------
// Purpose: ask if the string has anything in it
//-----------------------------------------------------------------------------
inline bool CUtlString::IsEmpty() const
{
	if ( m_pchString == NULL )
		return true;

	return m_pchString[0] == 0;
}


//-----------------------------------------------------------------------------
// Purpose: assignment
//-----------------------------------------------------------------------------
inline CUtlString &CUtlString::operator=( const char *pchString )
{
	SetValue( pchString );
	return *this;
}


//-----------------------------------------------------------------------------
// Purpose: assignment
//-----------------------------------------------------------------------------
inline CUtlString &CUtlString::operator=( CUtlString const &src )
{
	SetValue( src.String() );
	return *this;
}


//-----------------------------------------------------------------------------
// Purpose: comparison
//-----------------------------------------------------------------------------
inline bool CUtlString::operator==( CUtlString const &src ) const
{
	return !V_strcmp( String(), src.String() );
}


//-----------------------------------------------------------------------------
// Purpose: comparison
//-----------------------------------------------------------------------------
inline bool CUtlString::operator==( const char *pchString ) const
{
	return !V_strcmp( String(), pchString );
}

//-----------------------------------------------------------------------------
// Purpose: comparison
//-----------------------------------------------------------------------------
inline bool CUtlString::operator!=( CUtlString const &src ) const
{
	return !( *this == src );
}


//-----------------------------------------------------------------------------
// Purpose: comparison
//-----------------------------------------------------------------------------
inline bool CUtlString::operator!=( const char *pchString ) const
{
	return !( *this == pchString );
}


//-----------------------------------------------------------------------------
// Purpose: comparison
//-----------------------------------------------------------------------------
inline bool CUtlString::operator<( CUtlString const &val ) const
{
	return operator<( val.String() );
}


//-----------------------------------------------------------------------------
// Purpose: comparison
//-----------------------------------------------------------------------------
inline bool CUtlString::operator<( const char *pchString ) const
{
	return V_strcmp( String(), pchString ) < 0;
}

//-----------------------------------------------------------------------------
// Purpose: comparison
//-----------------------------------------------------------------------------
inline bool CUtlString::operator>( CUtlString const &val ) const
{
	return V_strcmp( String(), val.String() ) > 0;
}


//-----------------------------------------------------------------------------
// Purpose: comparison
//-----------------------------------------------------------------------------
inline bool CUtlString::operator>( const char *pchString ) const
{
	return V_strcmp( String(), pchString ) > 0;
}


//-----------------------------------------------------------------------------
// Return a string with this string and rhs joined together.
inline CUtlString& CUtlString::operator+=( const char *rhs )
{
	Append( rhs );
	return *this;
}


//-----------------------------------------------------------------------------
// Purpose: returns true if the string is not null
//-----------------------------------------------------------------------------
inline bool CUtlString::IsValid() const
{
	return ( m_pchString != NULL );
}


//-----------------------------------------------------------------------------
// Purpose: data accessor
//-----------------------------------------------------------------------------
inline const char *CUtlString::String() const
{
	return m_pchString ? m_pchString : "";
}


//-----------------------------------------------------------------------------
// Purpose: Sets the string to be the new value, taking a copy of it
//-----------------------------------------------------------------------------
inline void CUtlString::SetValue( const char *pchString )
{
	if ( m_pchString != pchString )
	{
		FreePv( m_pchString );

		if ( pchString && *pchString )
		{
			size_t nLength = 1 + strlen( pchString );
			if ( nLength > k_cchMaxString )
				AssertStringTooLong( );
			m_pchString = (char*)PvAlloc( nLength );
			V_memcpy( m_pchString, pchString, nLength );
		}
		else
		{
			m_pchString = NULL;
		}
	}
}

//-----------------------------------------------------------------------------
// Purpose: Converts the string to lower case in-place. Not necessarily clean
//          about all possibly localization issues.
//-----------------------------------------------------------------------------
inline void CUtlString::ToLower()
{
	if ( m_pchString != NULL )
	{
		for ( int i = 0; m_pchString[i]; i++ )
		{
			m_pchString[i] = (char)tolower( (int)(unsigned char)m_pchString[i] );
		}
	}
}


//-----------------------------------------------------------------------------
// Purpose: Converts the string to upper case in-place. Not necessarily clean
//          about all possibly localization issues.
//-----------------------------------------------------------------------------
inline void CUtlString::ToUpper()
{
	if ( m_pchString != NULL )
	{
		for ( int i = 0; m_pchString[i]; i++ )
		{
			m_pchString[i] = ( char )toupper( ( int )( unsigned char )m_pchString[i] );
		}
	}
}


//-----------------------------------------------------------------------------
// Purpose: Clear the string from memory, then free it.
//-----------------------------------------------------------------------------
inline void CUtlString::Wipe()
{
	//
	// Overwrite the current buffer
	//
	if ( m_pchString )
	{
		SecureZero();
	}
	SetValue( NULL );
}


//-----------------------------------------------------------------------------
// Purpose: Set directly and don't look for a null terminator in pValue.
//-----------------------------------------------------------------------------
inline void CUtlString::SetDirect( const char *pValue, size_t nChars )
{
	FreePv( m_pchString );
	m_pchString = NULL;

	if ( nChars > 0 )
	{
		if ( nChars + 1 > k_cchMaxString )
			AssertStringTooLong();
		m_pchString = (char*)PvAlloc( nChars + 1 );
		V_memcpy( m_pchString, pValue, nChars );
		m_pchString[nChars] = 0;
	}
}


//-----------------------------------------------------------------------------
// Purpose: Sets the string to be the new value, taking a copy of it
//-----------------------------------------------------------------------------
inline void CUtlString::Set( const char *pchString )
{
	SetValue( pchString );
}


//-----------------------------------------------------------------------------
// Purpose: Sets the string to be the new value, taking ownership of the pointer
//-----------------------------------------------------------------------------
inline void CUtlString::SetPtr( char *pchString )
{
	FreePv( m_pchString );
	m_pchString = pchString;
}


inline uint32 CUtlString::Length() const
{
	if ( !m_pchString )
		return 0;

	return (uint32) V_strlen( m_pchString );
}


//-----------------------------------------------------------------------------
// Purpose: format something sprintf() style, and take it as the new value of this CUtlString
//-----------------------------------------------------------------------------
inline size_t CUtlString::Format( const char *pFormat, ... )
{
	va_list args;
	va_start( args, pFormat );
	size_t len = FormatV( pFormat, args );
	va_end( args );
	return len;
}

//-----------------------------------------------------------------------------
// format a string and append the result to the string we hold
//-----------------------------------------------------------------------------
inline size_t CUtlString::AppendFormat( const char *pFormat, ... )
{
	va_list args;
	va_start( args, pFormat );
	size_t len = VAppendFormat( pFormat, args );
	va_end( args );
	return len;
}

//-----------------------------------------------------------------------------
// Purpose: concatenate the provided string to our current content
//-----------------------------------------------------------------------------
inline void CUtlString::Append( const char *pchAddition )
{
	if ( pchAddition && pchAddition[0] )
	{
		size_t cchLen = V_strlen( pchAddition );
		Append( pchAddition, cchLen );
	}
}


//-----------------------------------------------------------------------------
// Purpose: concatenate the provided string to our current content
//			when the additional string length is known
//-----------------------------------------------------------------------------
inline void CUtlString::Append( const char *pchAddition, size_t cbLen )
{
	if ( m_pchString == NULL )
	{
		SetDirect( pchAddition, cbLen );
	}
	else if ( pchAddition && pchAddition[0] )
	{
		size_t cbOld = V_strlen( m_pchString );
		if ( 1 + cbOld + cbLen > k_cchMaxString )
			AssertStringTooLong();
		char *pstrNew = (char *) PvAlloc( 1 + cbOld + cbLen );
		
		V_memcpy( pstrNew, m_pchString, cbOld );
		V_memcpy( pstrNew + cbOld, pchAddition, cbLen );
		pstrNew[ cbOld + cbLen ] = '\0';

		FreePv( m_pchString );
		m_pchString = pstrNew;
	}
}


//-----------------------------------------------------------------------------
// Purpose: repeat the passed character a specified number of times and
//			concatenate those characters to our existing content
//-----------------------------------------------------------------------------
inline void CUtlString::AppendRepeat( char ch, int cCount )
{
	if ( m_pchString == NULL )
	{
		if ( cCount + 1 > k_cchMaxString )
			AssertStringTooLong();
		char *pchNew = (char *) PvAlloc( cCount + 1 );
		for ( int n = 0; n < cCount; n++ )
			pchNew[n] = ch;
		pchNew[cCount] = 0;
		m_pchString = pchNew;
	}
	else
	{
		size_t cbOld = strlen( m_pchString );
		if ( 1 + cbOld + cCount > k_cchMaxString )
			AssertStringTooLong();
		char *pchNew = (char *) PvAlloc( 1 + cbOld + cCount );

		V_memcpy( pchNew, m_pchString, cbOld );
		for ( int n = 0; n < cCount; n++ )
			pchNew[n + cbOld] = ch;
		pchNew[cCount + cbOld] = 0;

		FreePv( m_pchString );
		m_pchString = pchNew;
	}
}


//-----------------------------------------------------------------------------
// Purpose: Swaps string contents
//-----------------------------------------------------------------------------
inline void CUtlString::Swap( CUtlString &src )
{
	char *tmp = src.m_pchString;
	src.m_pchString = m_pchString;
	m_pchString = tmp;
}


//-----------------------------------------------------------------------------
// Purpose: replace all occurrences of one character with another
//-----------------------------------------------------------------------------
inline size_t CUtlString::Replace( char chTarget, char chReplacement )
{
	size_t cReplacements = 0;

	if ( m_pchString != NULL )
	{
		for ( char *pstrWalker = m_pchString; *pstrWalker != 0; pstrWalker++ )
		{
			if ( *pstrWalker == chTarget )
			{
				*pstrWalker = chReplacement;
				cReplacements++;
			}
		}
	}

	return cReplacements;
}


//-----------------------------------------------------------------------------
// Purpose: Truncates the string to the specified number of characters
//-----------------------------------------------------------------------------
inline void CUtlString::Truncate( size_t nChars )
{
	if ( !m_pchString )
		return;

	size_t nLen = V_strlen( m_pchString );
	if ( nLen <= nChars )
		return;

	m_pchString[nChars] = '\0';
}


//-----------------------------------------------------------------------------
// Data and memory validation
//-----------------------------------------------------------------------------
#ifdef DBGFLAG_VALIDATE
inline void CUtlString::Validate( CValidator &validator, const char *pchName ) const
{
#ifdef _WIN32
	validator.Push( typeid(*this).raw_name(), (void*) this, pchName );
#else
	validator.Push( typeid(*this).name(), (void*) this, pchName );
#endif

	if ( NULL != m_pchString )
		validator.ClaimMemory( m_pchString );

	validator.Pop();
}
#endif // DBGFLAG_VALIDATE


//-----------------------------------------------------------------------------
// Purpose: Case insensitive CUtlString comparison
//-----------------------------------------------------------------------------
class CDefCaselessUtlStringEquals
{
public:
	CDefCaselessUtlStringEquals() {}
	CDefCaselessUtlStringEquals( int i ) {}
	inline bool operator()( const CUtlString &lhs, const CUtlString &rhs ) const
	{ 
		return ( V_stricmp( lhs.String(), rhs.String() ) == 0 );
	}
	inline bool operator!() const { return false; }
};
class CDefCaselessUtlStringLess
{
public:
	CDefCaselessUtlStringLess() {}
	CDefCaselessUtlStringLess( int i ) {}
	inline bool operator()( const CUtlString &lhs, const CUtlString &rhs ) const
	{ 
		return ( V_stricmp( lhs.String(), rhs.String() ) < 0 );
	}
	inline bool operator!() const { return false; }
};

// Hash specialization for CUtlStrings
template<>
struct HashFunctor<CUtlString>
{
	uint32 operator()(const CUtlString &strKey) const
	{
		return HashString( strKey.String() );
	}
};

struct HashFunctorUtlStringCaseless
{
	typedef uint32 TargetType;
	TargetType operator()(const CUtlString &strKey) const
	{
		return HashStringCaseless( strKey.String() );
	}
};

// HashItem() overload that works automatically with our hash containers
template<>
inline uint32 HashItem( const CUtlString &item )
{
	return HashString( item );
}

#endif // UTLSTRING_H
