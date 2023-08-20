//======= Copyright (C) 2005-2011, Valve Corporation, All rights reserved. =========
//
// Public domain MurmurHash3 by Austin Appleby is a very solid general-purpose
// hash with a 32-bit output. References:
// http://code.google.com/p/smhasher/ (home of MurmurHash3)
// https://sites.google.com/site/murmurhash/avalanche
// http://www.strchr.com/hash_functions 
//
// Variant Pearson Hash general purpose hashing algorithm described
// by Cargill in C++ Report 1994. Generates a 16-bit result.
// Now relegated to PearsonHash namespace, not recommended for use
//
//=============================================================================

#include <stdlib.h>
#include <generichash.h>
#include <strtools.h>
#include "minbase_endian.h"

#if defined(_MSC_VER) && _MSC_VER > 1200
#define ROTL32(x,y)	_rotl(x,y)
#define ROTL64(x,y)	_rotl64(x,y)
#else // defined(_MSC_VER) && _MSC_VER > 1200
static inline uint32 rotl32( uint32 x, int8 r )
{
	return ( x << r ) | ( x >> ( 32 - r ) );
}
static inline uint64 rotl64( uint64 x, int8 r )
{
	return ( x << r ) | ( x >> ( 64 - r ) );
}
#define	ROTL32(x,y)	rotl32(x,y)
#define ROTL64(x,y)	rotl64(x,y)

#endif // !defined(_MSC_VER)

#ifdef _MSC_VER
#define BIG_CONSTANT(x) (x)
#else	// defined(_MSC_VER)
#define BIG_CONSTANT(x) (x##LLU)
#endif // !defined(_MSC_VER)

//-----------------------------------------------------------------------------

uint32 MurmurHash3_32( const void * key, size_t len, uint32 seed, bool bCaselessStringVariant )
{
	const uint8 * data = (const uint8*)key;
	const ptrdiff_t nblocks = len / 4;
	uint32 uSourceBitwiseAndMask = 0xDFDFDFDF | ((uint32)bCaselessStringVariant - 1);

	uint32 h1 = seed;

	//----------
	// body

	const uint32 * blocks = (const uint32 *)(data + nblocks*4);

	for(ptrdiff_t i = -nblocks; i; i++)
	{
		uint32 k1 = LittleDWord(blocks[i]);
		k1 &= uSourceBitwiseAndMask;

		k1 *= 0xcc9e2d51;
		k1 = (k1 << 15) | (k1 >> 17);
		k1 *= 0x1b873593;

		h1 ^= k1;
		h1 = (h1 << 13) | (h1 >> 19);
		h1 = h1*5+0xe6546b64;
	}

	//----------
	// tail

	const uint8 * tail = (const uint8*)(data + nblocks*4);

	uint32 k1 = 0;

	switch(len & 3)
	{
	case 3: k1 ^= tail[2] << 16; // fallthrough
	case 2: k1 ^= tail[1] << 8; // fallthrough
	case 1: k1 ^= tail[0];
			k1 &= uSourceBitwiseAndMask;
			k1 *= 0xcc9e2d51;
			k1 = (k1 << 15) | (k1 >> 17);
			k1 *= 0x1b873593;
			h1 ^= k1;
	};

	//----------
	// finalization

	h1 ^= len;

	h1 ^= h1 >> 16;
	h1 *= 0x85ebca6b;
	h1 ^= h1 >> 13;
	h1 *= 0xc2b2ae35;
	h1 ^= h1 >> 16;

	return h1;
}

static inline uint64 fmix64( uint64 k )
{
	k ^= k >> 33;
	k *= BIG_CONSTANT( 0xff51afd7ed558ccd );
	k ^= k >> 33;
	k *= BIG_CONSTANT( 0xc4ceb9fe1a85ec53 );
	k ^= k >> 33;

	return k;
}

void MurmurHash3_128( const void * key, const int len, const uint32 seed, void * out )
{
	const uint8 * data = ( const uint8* )key;
	const int nblocks = len / 16;

	uint64 h1 = seed;
	uint64 h2 = seed;

	const uint64 c1 = BIG_CONSTANT( 0x87c37b91114253d5 );
	const uint64 c2 = BIG_CONSTANT( 0x4cf5ad432745937f );

	//----------
	// body

	const uint64 * blocks = ( const uint64 * )( data );

	for ( int i = 0; i < nblocks; i++ )
	{
		uint64 k1 = blocks[i * 2 + 0];
		uint64 k2 = blocks[i * 2 + 1];

		k1 *= c1; k1 = ROTL64( k1, 31 ); k1 *= c2; h1 ^= k1;

		h1 = ROTL64( h1, 27 ); h1 += h2; h1 = h1 * 5 + 0x52dce729;

		k2 *= c2; k2 = ROTL64( k2, 33 ); k2 *= c1; h2 ^= k2;

		h2 = ROTL64( h2, 31 ); h2 += h1; h2 = h2 * 5 + 0x38495ab5;
	}

	//----------
	// tail

	const uint8 * tail = ( const uint8* )( data + nblocks * 16 );

	uint64 k1 = 0;
	uint64 k2 = 0;

	switch ( len & 15 )
	{
	case 15: k2 ^= ( ( uint64 )tail[14] ) << 48; // fallthrough
	case 14: k2 ^= ( ( uint64 )tail[13] ) << 40; // fallthrough
	case 13: k2 ^= ( ( uint64 )tail[12] ) << 32; // fallthrough
	case 12: k2 ^= ( ( uint64 )tail[11] ) << 24; // fallthrough
	case 11: k2 ^= ( ( uint64 )tail[10] ) << 16; // fallthrough
	case 10: k2 ^= ( ( uint64 )tail[9] ) << 8; // fallthrough
	case  9: k2 ^= ( ( uint64 )tail[8] ) << 0; // fallthrough
		k2 *= c2; k2 = ROTL64( k2, 33 ); k2 *= c1; h2 ^= k2;
		// fallthrough
	case  8: k1 ^= ( ( uint64 )tail[7] ) << 56; // fallthrough
	case  7: k1 ^= ( ( uint64 )tail[6] ) << 48; // fallthrough
	case  6: k1 ^= ( ( uint64 )tail[5] ) << 40; // fallthrough
	case  5: k1 ^= ( ( uint64 )tail[4] ) << 32; // fallthrough
	case  4: k1 ^= ( ( uint64 )tail[3] ) << 24; // fallthrough
	case  3: k1 ^= ( ( uint64 )tail[2] ) << 16; // fallthrough
	case  2: k1 ^= ( ( uint64 )tail[1] ) << 8; // fallthrough
	case  1: k1 ^= ( ( uint64 )tail[0] ) << 0; // fallthrough
		k1 *= c1; k1 = ROTL64( k1, 31 ); k1 *= c2; h1 ^= k1;
	};

	//----------
	// finalization

	h1 ^= len; h2 ^= len;

	h1 += h2;
	h2 += h1;

	h1 = fmix64( h1 );
	h2 = fmix64( h2 );

	h1 += h2;
	h2 += h1;

	( ( uint64* )out )[0] = h1;
	( ( uint64* )out )[1] = h2;
}
