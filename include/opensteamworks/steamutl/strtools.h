//========= Copyright Valve Corporation, All rights reserved. ============//
//
// Misc string functions.  This is just what we need to compile SDR,
// stripped out from Steam's version that had a bunch of extra stuff in it.
//
//========================================================================//

#ifndef VSTDLIB_STRTOOLS_H
#define VSTDLIB_STRTOOLS_H
#pragma once

#include <ctype.h>
#include <string.h>

#ifdef __MINGW32__
	// for __MINGW_PRINTF_FORMAT
	#include <stdio.h>
#endif

#include <stdlib.h>
#include <limits.h>
#include <stdarg.h>
#include "miniutl.h"

template< class T > class CUtlMemory;
template< class T, class A > class CUtlVector;

#define V_strlen (int)strlen
#define V_strcmp strcmp
#define V_memcmp memcmp
#define V_memmove memmove
#define V_memset memset
#define V_memcpy memcpy
#define V_strstr strstr
#define V_strchr strchr

int	V_strncmp( const char *s1, const char *s2, int count );
int V_strnicmp( const char *s1, const char *s2, int n );
const char* V_strnchr( const char* pStr, char c, int n );
const char* V_stristr( const char* pStr, const char* pSearch );
const char* V_strnistr( const char* pStr, const char* pSearch, int n );
inline int V_stricmp( const char *s1, const char *s2 ) { return V_strnicmp( s1, s2, INT_MAX ); }

void V_strncpy( OUT_Z_CAP(maxLen) char *pDest, const char *pSrc, size_t maxLen );
int V_vsnprintf( OUT_Z_CAP(maxLen) char *pDest, int maxLen, const char *pFormat, va_list params );
int V_vsnprintfRet( OUT_Z_CAP(maxLen) char *pDest, int maxLen, const char *pFormat, va_list params, bool *pbTruncated );
int V_snprintf( OUT_Z_CAP(destLen) char *pDest, size_t destLen, PRINTF_FORMAT_STRING const char *pFormat, ... ) FMTFUNCTION( 3, 4 );

#define COPY_ALL_CHARACTERS -1
char *V_strncat(INOUT_Z_CAP(destBufferSize) char *, const char *, size_t destBufferSize, int max_chars_to_copy=COPY_ALL_CHARACTERS );

// is* helpers
inline bool V_isspace(char c) { return isspace((unsigned char)c) != 0; }

// Split the specified string on the specified separator.
// Returns a list of strings separated by pSeparator.
// You are responsible for freeing the contents of outStrings (call outStrings.PurgeAndDeleteElements).
void V_SplitString( const char *pString, const char *pSeparator, CUtlVector<char*, CUtlMemory<char*> > &outStrings, bool bIncludeEmptyStrings = false );

// Strips trailing *ASCII* whitespace characters.  (Any
// character that returns true for V_isspace returns true.)  Doesn't
// handle all unicode whitespace characters
void V_StripTrailingWhitespaceASCII( char *pch );

// trim whitespace from both ends of the string
int V_StrTrim( char *pStr );

#ifndef _WIN32
#define _atoi64 atoll
#define _wtoi(arg) wcstol(arg, NULL, 10)
#define _i64toa( num, buf, base ) sprintf( buf, "%lld", num )
#define _stricmp strcasecmp
#define _strtoi64 strtoll
#define _strtoui64 strtoull
#define _vsnprintf vsnprintf
#ifdef TEXT
#undef TEXT
#endif
#define TEXT(str) str
#endif // POSIX

#endif	// VSTDLIB_STRTOOLS_H
