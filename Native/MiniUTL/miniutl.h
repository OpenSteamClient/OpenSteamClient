/* Copyright (C) 2019 Flying With Gauss */

/* Because I removed all tier0 stuff, we need to compensate it with big universal header */

#pragma once
#ifndef MINIUTL_H
#define MINIUTL_H

#include <assert.h>
#include <stdlib.h>
#include <stddef.h>
#include <stdarg.h>
#include <stdio.h>
#include <string.h>
#include <ctype.h>

#if defined(_MSC_VER)
	#define FORCEINLINE			    __forceinline
	#define FMTFUNCTION( x, y )
#else
	#if defined(__GNUC__)
		#ifdef __MINGW__
			#define FMTFUNCTION( fmtargnumber, firstvarargnumber ) __attribute__ (( format( __MINGW_PRINTF_FORMAT, fmtargnumber, firstvarargnumber )))
		#else
			#define FMTFUNCTION( fmtargnumber, firstvarargnumber ) __attribute__ (( format( __printf__, fmtargnumber, firstvarargnumber )))
		#endif
		#if __GNUC__ >= 4
			#define FORCEINLINE          inline __attribute__ ((always_inline))
		#else
			#define FORCEINLINE inline
		#endif
		#define GCC_VERSION (__GNUC__ * 10000 + __GNUC_MINOR__ * 100 + __GNUC_PATCHLEVEL__)
	#else
		#define FORCEINLINE          inline
		#define PRINTF_FORMAT_STRING
		#define FMTFUNCTION( x, y )
	#endif
	#define _vsnprintf vsnprintf
	#define __cdecl
#endif

#if _MSC_VER >= 1600 && defined(_PREFAST_)// VS 2010 and above.
	// Tag all printf-style format strings with this (consumed by MSVC).
	#define PRINTF_FORMAT_STRING _Printf_format_string_
	#define SCANF_FORMAT_STRING _Scanf_format_string_impl_

	// Various macros for specifying the capacity of the buffer pointed
	// to by a function parameter. Variations include in/out/inout,
	// CAP (elements) versus BYTECAP (bytes), and null termination or
	// not (_Z).
	#define IN_Z _In_z_
	#define IN_CAP(x) _In_count_(x)
	#define IN_BYTECAP(x) _In_bytecount_(x)
	#define OUT_Z_CAP(x) _Out_z_cap_(x)
	#define OUT_CAP(x) _Out_cap_(x)
	#define OUT_BYTECAP(x) _Out_bytecap_(x)
	#define OUT_Z_BYTECAP(x) _Out_z_bytecap_(x)
	#define INOUT_BYTECAP(x) _Inout_bytecap_(x)
	#define INOUT_Z_CAP(x) _Inout_z_cap_(x)
	#define INOUT_Z_BYTECAP(x) _Inout_z_bytecap_(x)
	// These macros are use for annotating array reference parameters, typically used in functions
	// such as V_strcpy_safe. Because they are array references the capacity is already known.
	#if _MSC_VER >= 1700
		#define IN_Z_ARRAY _Pre_z_
		#define OUT_Z_ARRAY _Post_z_
		#define INOUT_Z_ARRAY _Prepost_z_
	#else
		#define IN_Z_ARRAY _Deref_pre_z_
		#define OUT_Z_ARRAY _Deref_post_z_
		#define INOUT_Z_ARRAY _Deref_prepost_z_
	#endif // _MSC_VER >= 1700
#else
	#define PRINTF_FORMAT_STRING
	#define SCANF_FORMAT_STRING
	#define IN_Z
	#define IN_CAP(x)
	#define IN_BYTECAP(x)
	#define OUT_Z_CAP(x)
	#define OUT_CAP(x)
	#define OUT_BYTECAP(x)
	#define OUT_Z_BYTECAP(x)
	#define INOUT_BYTECAP(x)
	#define INOUT_Z_CAP(x)
	#define INOUT_Z_BYTECAP(x)
	#define OUT_Z_ARRAY
	#define INOUT_Z_ARRAY
#endif

#include "strtools.h"

#ifdef MY_COMPILER_SUCKS
	#define COMPILE_TIME_ASSERT( pred ) typedef int UNIQUE_ID[ (pred) ? 1 : -1]
#else
	#define COMPILE_TIME_ASSERT( pred ) static_assert( pred, "Compile time assert constraint is not true: " #pred )
#endif

#define Assert( x ) assert( x )
#define DbgAssert( x ) assert( x ) // a1ba: this should raise under debugger only?

#ifndef NDEBUG
inline void AssertMsg( int pred, const char *fmt, ... )
{
	char buf[1024];
	va_list va;
	va_start( va, fmt );
	_vsnprintf( buf, sizeof( buf ), fmt, va );
	va_end( va );

	assert( pred && fmt );
}

#define AssertMsg1( x, msg, msg1 ) AssertMsg( x, msg, msg1 )
#define DbgAssertMsg1( x, msg, msg1 ) AssertMsg( x, msg, msg1 )
#define AssertMsg2( x, msg, msg1, msg2 ) AssertMsg( x, msg, msg1, msg2 )
#define AssertEquals( _exp, _expectedValue ) AssertMsg2( (_exp) == (_expectedValue), "Expected %d but got %d!", (_expectedValue), (_exp) )
#else
#define AssertMsg( x, msg ) ( x )
#define AssertMsg1( x, msg, msg1 ) ( x )
#define DbgAssertMsg1( x, msg, msg1 ) ( x )
#define AssertMsg2( x, msg, msg1, msg2 ) ( x )
#define AssertEquals( x, y ) ( x )
#endif

#define VerifyEquals( x, y ) AssertEquals( x, y )

#define PvAlloc   malloc
#define PvRealloc realloc
#define FreePv    free

#ifndef _WIN32
	#define PlatformSecureZeroMemory( ptr, len ) memset( ptr, 0, len )
#else
	#define PlatformSecureZeroMemory( ptr, len ) SecureZeroMemory( ptr, len )
#endif

#define Msg       printf

inline void Error( const char *msg )
{
	puts( msg );
	abort();
}

#define MEM_ALLOC_CREDIT_CLASS()

// This is the preferred Min operator. Using the MIN macro can lead to unexpected
// side-effects or more expensive code.
template< class T >
static FORCEINLINE T const & Min( T const &val1, T const &val2 )
{
	return val1 < val2 ? val1 : val2;
}

// This is the preferred Max operator. Using the MAX macro can lead to unexpected
// side-effects or more expensive code.
template< class T >
static FORCEINLINE T const & Max( T const &val1, T const &val2 )
{
	return val1 > val2 ? val1 : val2;
}

#define V_ARRAYSIZE( arr ) ( sizeof((arr)) / sizeof((arr)[0]) )

#if _MSC_VER == 1200
// msvc6 only targets win32
typedef char int8;
typedef unsigned char uint8;

typedef short int16;
typedef unsigned short uint16;

typedef int int32;
typedef unsigned int uint32;

typedef __int64 int64;
typedef unsigned __int64 uint64;
#else
#include <stdint.h>
typedef int8_t int8;
typedef uint8_t uint8;

typedef int16_t int16;
typedef uint16_t uint16;

typedef int32_t int32;
typedef uint32_t uint32;

typedef int64_t int64;
typedef uint64_t uint64;
#endif

typedef unsigned int uint;

#ifdef _MSC_VER
#include <new.h>
#else
#include <new>
#endif

template <class T>
inline void Construct( T* pMemory )
{
	::new( pMemory ) T;
}

template <class T>
inline void CopyConstruct( T* pMemory, T const& src )
{
	::new( pMemory ) T(src);
}

template <class T>
inline void Destruct( T* pMemory )
{
	pMemory->~T();
}

namespace basetypes
{
	template <class T>
	inline bool IsPowerOf2( T n )
	{
		return n > 0 && (n & (n - 1)) == 0;
	}

	template <class T1, class T2>
	inline T2 ModPowerOf2( T1 a, T2 b )
	{
		return T2( a ) & (b - 1);
	}

	template <class T>
	inline T RoundDownToMultipleOf( T n, T m )
	{
		return n - (IsPowerOf2( m ) ? ModPowerOf2( n, m ) : (n%m));
	}

	template <class T>
	inline T RoundUpToMultipleOf( T n, T m )
	{
		if ( !n )
		{
			return m;
		}
		else
		{
			return RoundDownToMultipleOf( n + m - 1, m );
		}
	}
}

#endif // MINIUTL_H
