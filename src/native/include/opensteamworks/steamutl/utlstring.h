#pragma once

#include "miniutl.h"

const uint k_cchMaxString = 0x7fff0000;

class CUtlString {
public:
	CUtlString() {
		str = NULL;
	}
	~CUtlString() {
		FreePv(str);
	}
	void AssertStringTooLong();
	void SetDirect(const char *pValue, size_t nChars);
	char *str;
};

inline void CUtlString::SetDirect( const char *pValue, size_t nChars )
{
	FreePv( str );
	str = NULL;

	if ( nChars > 0 )
	{
		if ( nChars + 1 > k_cchMaxString )
			AssertStringTooLong();
		str = (char*)PvAlloc( nChars + 1 );
		V_memcpy( str, pValue, nChars );
		str[nChars] = 0;
	}
}

inline void CUtlString::AssertStringTooLong()
{
	AssertMsg( false, "Assertion failed: length > k_cchMaxString" );
}