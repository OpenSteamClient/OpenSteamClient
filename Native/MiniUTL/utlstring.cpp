//========= Copyright 1996-2005, Valve Corporation, All rights reserved. ============//
//
// Purpose: Larger string functions go here.
//
// $Header: $
// $NoKeywords: $
//=============================================================================//

#ifndef _XBOX
#pragma warning (disable:4514)
#endif

#include "utlstring.h"
#include "utlvector.h"
#include "winlite.h"

//-----------------------------------------------------------------------------
// Purpose: Helper: Find s substring
//-----------------------------------------------------------------------------
static ptrdiff_t IndexOf( const char *pstrToSearch, const char *pstrTarget )
{
	const char *pstrHit = V_strstr( pstrToSearch, pstrTarget );
	if ( pstrHit == NULL )
	{
		return -1;	// Not found.
	}
	return ( pstrHit - pstrToSearch );
}


//-----------------------------------------------------------------------------
// Purpose: returns true if the string ends with the string passed in
//-----------------------------------------------------------------------------
static bool BEndsWith( const char *pstrToSearch, const char *pstrToFind, bool bCaseless )
{
	if ( !pstrToSearch )
		return false;

	if ( !pstrToFind )
		return true;

	size_t nThisLength = V_strlen( pstrToSearch );
	size_t nThatLength = V_strlen( pstrToFind );

	if ( nThatLength == 0 )
		return true;

	if ( nThatLength > nThisLength )
		return false;

	size_t nIndex = nThisLength - nThatLength;

	if ( bCaseless )
		return V_stricmp( pstrToSearch + nIndex, pstrToFind ) == 0;
	else
		return V_strcmp( pstrToSearch + nIndex, pstrToFind ) == 0;
}


//-----------------------------------------------------------------------------
// Purpose: returns true if the string starts with the string passed in
//-----------------------------------------------------------------------------
static bool BStartsWith( const char *pstrToSearch, const char *pstrToFind, bool bCaseless )
{
	if ( !pstrToSearch )
		return false;

	if ( !pstrToFind )
		return true;

	if ( bCaseless )
	{
		int nThatLength = V_strlen( pstrToFind );

		if ( nThatLength == 0 )
			return true;

		return V_strnicmp( pstrToSearch, pstrToFind, nThatLength ) == 0;
	}
	else
		return V_strstr( pstrToSearch, pstrToFind ) == pstrToSearch;
}


//-----------------------------------------------------------------------------
// Purpose: Helper: kill all whitespace.
//-----------------------------------------------------------------------------
static size_t RemoveWhitespace( char *pszString )
{
	if ( pszString == NULL )
		return 0;

	char *pstrDest = pszString;
	size_t cRemoved = 0;
	for ( char *pstrWalker = pszString; *pstrWalker != 0; pstrWalker++ )
	{
		if ( !isspace( (unsigned char)*pstrWalker ) ) 
		{
			*pstrDest = *pstrWalker;
			pstrDest++;
		}
		else
			cRemoved += 1;
	}
	*pstrDest = 0;

	return cRemoved;
}

//-----------------------------------------------------------------------------
// Purpose: Helper for Format() method
//-----------------------------------------------------------------------------
size_t CUtlString::FormatV( const char *pFormat, va_list args )
{
	size_t len = 0;
#if defined _WIN32 || defined __WATCOMC__
	char buf[4096];

	len = _vsnprintf( buf, sizeof( buf ), pFormat, args );

	Assert( len >= 0 );
	Assert( len < sizeof( buf ));

	// get it
	FreePv( m_pchString );
	m_pchString = (char *)PvAlloc( len + 1 );
	strcpy( m_pchString, buf );
#elif defined ( _PS3 )

	// ignore the PS3 documentation about vsnprintf returning -1 when the string is too small. vsprintf seems to do the right thing (least at time of
	// implementation) and returns the number of characters needed when you pass in a buffer that is too small

	FreePv( m_pchString );
	m_pchString = NULL;	

	len = vsnprintf( NULL, 0, pFormat, args );
	if ( len > 0 )
	{
		m_pchString = (char*) PvAlloc( len + 1 );
		len = vsnprintf( m_pchString, len + 1, pFormat, args );
	}

#else

	char *buf = NULL;
	len = vasprintf( &buf, pFormat, args );

	// Len < 0 represents an overflow
	if( buf )
	{
		// We need to get the string into PvFree-compatible memory, which
		// we can't assume is directly interoperable with the malloc memory
		// that vasprintf returned (definitely not compatible with a debug
		// allocator, for example).
		Set( buf );
		free( buf );
	}
#endif
	return len;
}


//-----------------------------------------------------------------------------
// Purpose: implementation helper for AppendFormat()
//-----------------------------------------------------------------------------
size_t CUtlString::VAppendFormat( const char *pFormat, va_list args )
{
	size_t len = 0;
#if defined _WIN32 || defined __WATCOMC__
	char pstrFormatted[4096];

	// format into that space, which is certainly enough
	len = _vsnprintf( pstrFormatted, sizeof(pstrFormatted), pFormat, args );

	Assert( len >= 0 );
	Assert( len < sizeof( pstrFormatted ));

#elif defined ( _PS3 )
	char *pstrFormatted = NULL;

	// ignore the PS3 documentation about vsnprintf returning -1 when the string is too small. vsprintf seems to do the right thing (least at time of
	// implementation) and returns the number of characters needed when you pass in a buffer that is too small

	len = vsnprintf( NULL, 0, pFormat, args );
	if ( len > 0 )
	{
		pstrFormatted = (char*) PvAlloc( len + 1 );
		len = vsnprintf( pstrFormatted, len + 1, pFormat, args );
	}

#else
	char *pstrFormatted = NULL;
	len = vasprintf( &pstrFormatted, pFormat, args );
#endif

	// if we ended with a formatted string, append and free it
	if ( pstrFormatted != NULL )
	{
		Append( pstrFormatted, len );
#if defined( _WIN32 ) || defined __WATCOMC__
		// no need to free a buffer on stack
#elif defined( _PS3 )
		FreePv( pstrFormatted );
#else
		free( pstrFormatted );
#endif
	}

	return len;
}


//-----------------------------------------------------------------------------
// Purpose: replace all occurrences of one string with another
//			replacement string may be NULL or "" to remove target string
//-----------------------------------------------------------------------------
size_t CUtlString::Replace( const char *pstrTarget, const char *pstrReplacement )
{
	return ReplaceInternal( pstrTarget, pstrReplacement, 
		   (const char *(*)(const char *,const char *))V_strstr );
}


//-----------------------------------------------------------------------------
// Purpose: replace all occurrences of one string with another
//			replacement string may be NULL or "" to remove target string
//-----------------------------------------------------------------------------
size_t CUtlString::ReplaceCaseless( const char *pstrTarget, const char *pstrReplacement )
{
	return ReplaceInternal( pstrTarget, pstrReplacement, V_stristr );
}

//-----------------------------------------------------------------------------
// Purpose: replace all occurrences of one string with another
//			replacement string may be NULL or "" to remove target string
//-----------------------------------------------------------------------------
size_t CUtlString::ReplaceInternal( const char *pstrTarget, const char *pstrReplacement, const char *pfnCompare(const char*, const char*) )
{
	size_t cReplacements = 0;
	if ( pstrReplacement == NULL )
		pstrReplacement = "";

	size_t nTargetLength = V_strlen( pstrTarget );
	size_t nReplacementLength = V_strlen( pstrReplacement );

	if ( m_pchString != NULL && pstrTarget != NULL  )
	{
		// walk the string counting hits
		const char *pstrHit = m_pchString;
		for ( pstrHit = pfnCompare( pstrHit, pstrTarget ); pstrHit != NULL && *pstrHit != 0; /* inside */ )
		{
			cReplacements++;
			// look for the next target and keep looping
			pstrHit = pfnCompare( pstrHit + nTargetLength, pstrTarget );
		}

		// if we didn't miss, get to work
		if ( cReplacements > 0 )
		{
			// reallocate only once; how big will we need?
			size_t nNewLength = 1 + V_strlen( m_pchString ) + cReplacements * ( nReplacementLength - nTargetLength );

			char *pstrNew = (char*) PvAlloc( nNewLength );
			if ( nNewLength == 1 )
			{
				// shortcut simple case, even if rare
				*pstrNew = 0;
			}
			else
			{
				const char *pstrPreviousHit = NULL;
				char *pstrDestination = pstrNew;
				pstrHit = m_pchString;
				size_t cActualReplacements = 0;
				for ( pstrHit = pfnCompare( m_pchString, pstrTarget ); pstrHit != NULL && *pstrHit != 0; /* inside */ )
				{
					cActualReplacements++;

					// copy from the previous hit to the match
					if ( pstrPreviousHit == NULL )
						pstrPreviousHit = m_pchString;
					memcpy( pstrDestination, pstrPreviousHit, pstrHit - pstrPreviousHit );
					pstrDestination += ( pstrHit - pstrPreviousHit );

					// push the replacement string in
					memcpy( pstrDestination, pstrReplacement, nReplacementLength );
					pstrDestination += nReplacementLength;

					pstrPreviousHit = pstrHit + nTargetLength;
					pstrHit = pfnCompare( pstrPreviousHit, pstrTarget );
				}

				while ( pstrPreviousHit != NULL && *pstrPreviousHit != 0 )
				{
					*pstrDestination = *pstrPreviousHit;
					pstrDestination++;
					pstrPreviousHit++;
				}
				*pstrDestination = 0;

				Assert( pstrNew + nNewLength == pstrDestination + 1);
				Assert( cActualReplacements == cReplacements );
			}

			// release the old string, set the new one
			FreePv( m_pchString );
			m_pchString = pstrNew;

		}
	}

	return cReplacements;
}

//-----------------------------------------------------------------------------
// Purpose: Indicates if the target string exists in this instance.
//			The index is negative if the target string is not found, otherwise it is the index in the string.
//-----------------------------------------------------------------------------
ptrdiff_t CUtlString::IndexOf( const char *pstrTarget ) const
{
	return ::IndexOf( String(), pstrTarget );
}


//-----------------------------------------------------------------------------
// Purpose: returns true if the string ends with the string passed in
//-----------------------------------------------------------------------------
bool CUtlString::BEndsWith( const char *pchString ) const
{
	return ::BEndsWith( String(), pchString, false );
}


//-----------------------------------------------------------------------------
// Purpose: returns true if the string ends with the string passed in (caseless)
//-----------------------------------------------------------------------------
bool CUtlString::BEndsWithCaseless( const char *pchString ) const
{
	return ::BEndsWith( String(), pchString, true );
}


//-----------------------------------------------------------------------------
// Purpose: returns true if the string starts with the string passed in
//-----------------------------------------------------------------------------
bool CUtlString::BStartsWith( const char *pchString ) const
{
	return ::BStartsWith( String(), pchString, false );
}


//-----------------------------------------------------------------------------
// Purpose: returns true if the string ends with the string passed in (caseless)
//-----------------------------------------------------------------------------
bool CUtlString::BStartsWithCaseless( const char *pchString ) const
{
	return ::BStartsWith( String(), pchString, true );
}


//-----------------------------------------------------------------------------
// Purpose: 
//			remove whitespace -- anything that is isspace() -- from the string
//-----------------------------------------------------------------------------
size_t CUtlString::RemoveWhitespace()
{
	return ::RemoveWhitespace( m_pchString );
}


//-----------------------------------------------------------------------------
// Purpose: 
//			trim whitespace from front and back of string
//-----------------------------------------------------------------------------
size_t CUtlString::TrimWhitespace()
{
	if ( m_pchString == NULL )
		return 0;

	int cChars = V_StrTrim( m_pchString );
	return cChars;
}


//-----------------------------------------------------------------------------
// Purpose: 
//			trim whitespace from back of string
//-----------------------------------------------------------------------------
size_t CUtlString::TrimTrailingWhitespace()
{
	if ( m_pchString == NULL )
		return 0;

	uint32 cChars = Length();
	if ( cChars == 0 )
		return 0;

	char *pCur = &m_pchString[cChars - 1];
	while ( pCur >= m_pchString && isspace( *pCur ) )
	{
		*pCur = '\0';
		pCur--;
	}

	return pCur - m_pchString + 1;
}


//-----------------------------------------------------------------------------
// Purpose: out-of-line assertion to keep code generation size down
//-----------------------------------------------------------------------------
void CUtlString::AssertStringTooLong()
{
	AssertMsg( false, "Assertion failed: length > k_cchMaxString" );
}


//-----------------------------------------------------------------------------
// Purpose: format binary data as hex characters, appending to existing data
//-----------------------------------------------------------------------------
void CUtlString::AppendHex( const uint8 *pbInput, size_t cubInput, bool bLowercase /*= true*/ )
{
	if ( !cubInput )
		return;

	size_t existingLen = Length();
	if ( existingLen >= k_cchMaxString || cubInput*2 >= k_cchMaxString - existingLen )
	{
		Assert( existingLen < k_cchMaxString && cubInput * 2 < k_cchMaxString - existingLen );
		return;
	}

	const char *pchHexLookup = bLowercase ? "0123456789abcdef" : "0123456789ABCDEF";
	CUtlString newValue( existingLen + cubInput * 2 + 1 );
	V_memcpy( newValue.Access(), Access(), existingLen );
	char *pOut = newValue.Access() + existingLen;
	for ( ; cubInput; --cubInput, ++pbInput )
	{
		uint8 val = *pbInput;
		*pOut++ = pchHexLookup[val >> 4];
		*pOut++ = pchHexLookup[val & 15];
	}
	*pOut = '\0';
	Swap( newValue );
}


// Catch invalid UTF-8 sequences and return false if found, or true if the sequence is correct
static bool BVerifyValidUTF8Continuation( size_t unStart, size_t unContinuationLength, const uint8 *pbCharacters )
{
	for ( size_t i = 0; i < unContinuationLength; ++ i )
	{
		// Make sure byte is of the form 10xxxxxx
		// Note: this also catches an unexpected NULL terminator and prevents us from overrunning the string
		if ( ( pbCharacters[i + unStart] & 0xC0 ) != 0x80 )
			return false;
	}
	return true;
}

//-----------------------------------------------------------------------------
// Purpose: Caps the string to the specified number of bytes/chars,
// while respecting UTF-8 character blocks. Resulting string will be
// strictly less than unMaxChars and unMaxBytes.
//-----------------------------------------------------------------------------
bool CUtlString::TruncateUTF8Internal( size_t unMaxChars, size_t unMaxBytes )
{
	if ( !m_pchString )
		return false;

	const uint8 *pbCharacters = ( const uint8 * )m_pchString;
	size_t unBytes = 0;
	size_t unChars = 0;
	bool bSuccess = true;
	
	while ( unBytes < unMaxBytes && unChars < unMaxChars && pbCharacters[unBytes] != '\0' )
	{
		if ( ( pbCharacters[unBytes] & 0x80 ) == 0 )
		{
			// standard ASCII
			unBytes ++;
		}
		else if ( ( pbCharacters[unBytes] & 0xE0 ) == 0xC0 ) // check for 110xxxxx bit pattern, indicates 2 byte character
		{
			if ( !BVerifyValidUTF8Continuation( unBytes, 1, pbCharacters + 1 ) )
			{
				bSuccess = false;
				break;
			}

			unBytes += 2;
		}
		else if ( ( pbCharacters[unBytes] & 0xF0 ) == 0xE0 ) // check for 1110xxxx bit pattern, indicates 3 byte character
		{
			if ( !BVerifyValidUTF8Continuation( unBytes, 2, pbCharacters + 1 ) )
			{
				bSuccess = false;
				break;
			}

			unBytes += 3;
		}
		else if ( ( pbCharacters[unBytes] & 0xF8 ) == 0xF0 ) // check for 11110xxx bit pattern, indicates 4 byte character
		{
			if ( !BVerifyValidUTF8Continuation( unBytes, 3, pbCharacters + 1 ) )
			{
				bSuccess = false;
				break;
			}

			unBytes += 4;
		}
		else if ( ( pbCharacters[unBytes] & 0xFC ) == 0xF8 ) // check for 111110xx bit pattern, indicates 5 byte character
		{
			if ( !BVerifyValidUTF8Continuation( unBytes, 4, pbCharacters + 1 ) )
			{
				bSuccess = false;
				break;
			}

			unBytes += 5;
		}
		else if ( ( pbCharacters[unBytes] & 0xFE ) == 0xFC ) // check for 1111110x bit pattern, indicates 6 byte character
		{
			if ( !BVerifyValidUTF8Continuation( unBytes, 5, pbCharacters + 1 ) )
			{
				bSuccess = false;
				break;
			}

			unBytes += 6;
		}
		else
		{
			// Unexpected character
			bSuccess = false;
			break;
		}

		unChars ++;
	}

	m_pchString[unBytes] = '\0';

	return bSuccess;
}

void CUtlString::SecureZero()
{
	PlatformSecureZeroMemory( m_pchString, V_strlen( m_pchString ));
}
