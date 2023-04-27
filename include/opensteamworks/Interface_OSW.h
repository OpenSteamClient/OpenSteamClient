//==========================  Open Steamworks  ================================
//
// This file is part of the Open Steamworks project. All individuals associated
// with this project do not claim ownership of the contents
//
// The code, comments, and all related files, projects, resources,
// redistributables included with this project are Copyright Valve Corporation.
// Additionally, Valve, the Valve logo, Half-Life, the Half-Life logo, the
// Lambda logo, Steam, the Steam logo, Team Fortress, the Team Fortress logo,
// Opposing Force, Day of Defeat, the Day of Defeat logo, Counter-Strike, the
// Counter-Strike logo, Source, the Source logo, and Counter-Strike Condition
// Zero are trademarks and or registered trademarks of Valve Corporation.
// All other trademarks are property of their respective owners.
//
//=============================================================================

#if !defined(INTERFACEOSW_H) && !defined(_S4N_)
#define INTERFACEOSW_H
#ifdef _WIN32
#pragma once
#endif



#ifdef _WIN32
	#include "Win32Library.h"

	static const int k_iPathMaxSize = MAX_PATH;
	static const char* k_cszSteam2LibraryName = "steam.dll";
	static const char* k_cszSteam3LibraryName = "steamclient.dll";
#elif defined(__APPLE_CC__)
	#include "POSIXLibrary.h"
	#include <CoreServices/CoreServices.h>
	#include <sys/param.h>

	static const int k_iPathMaxSize = MAXPATHLEN;
	static const char* k_cszSteam2LibraryName = "libsteam.dylib";
	static const char* k_cszSteam3LibraryName = "steamclient.dylib";
#elif defined(__linux__)
	#include "POSIXLibrary.h"
	#include <libgen.h>
	#include <limits.h>

	static const int k_iPathMaxSize = PATH_MAX;
	static const char* k_cszSteam2LibraryName = "libsteam.so";
	static const char* k_cszSteam3LibraryName = "steamclient.so";
#else
	#error Unsupported platform
#endif


class CSteamAPILoader
{
public:
	enum ESearchOrder
	{
		k_ESearchOrderLocalFirst,
		k_ESearchOrderSteamInstallFirst,
	};

	CSteamAPILoader(ESearchOrder eSearchOrder = k_ESearchOrderLocalFirst)
	{
		m_eSearchOrder = eSearchOrder;
		m_pSteamclient = NULL;
		m_pSteam = NULL;

		TryGetSteamDir();
		TryLoadLibraries();
	}

	~CSteamAPILoader()
	{
		if(m_pSteamclient)
			delete m_pSteamclient;
		if(m_pSteam)
			delete m_pSteam;
	}

	CreateInterfaceFn GetSteam3Factory()
	{
		return (CreateInterfaceFn)m_pSteamclient->GetSymbol("CreateInterface");
	}

	FactoryFn STEAMWORKS_DEPRECATE("This function is provided for backwards compatibility only. Steam2 is deprecated. Please use Steam3 instead") GetSteam2Factory()
	{
		return (FactoryFn)m_pSteam->GetSymbol("_f");
	}
	
	const char* GetSteamDir()
	{
		return m_szSteamPath;
	}
	
	const DynamicLibrary *GetSteamClientModule()
	{
		return m_pSteamclient;
	}
	const DynamicLibrary *GetSteamModule()
	{
		return m_pSteam;
	}

	CreateInterfaceFn STEAMWORKS_DEPRECATE("This function is provided for backward compatiblity. Please use GetSteam3Factory instead") Load()
	{
		return GetSteam3Factory();
	}

	FactoryFn STEAMWORKS_DEPRECATE("This function is provided for backward compatiblity. Please use GetSteam2Factory instead") LoadFactory()
	{
#ifdef _MSC_VER
	#pragma warning(push)
	#pragma warning(disable: 4996)
#elif defined(__GNUC__)
	#pragma GCC diagnostic push
	#pragma GCC diagnostic ignored "-Wdeprecated"
	#pragma GCC diagnostic ignored "-Wdeprecated-declarations"
#endif

		return GetSteam2Factory();

#ifdef _MSC_VER
	#pragma warning(pop)
#elif defined(__GNUC__)
	#pragma GCC diagnostic pop
#endif
	}

private:

#ifdef _MSC_VER
	#pragma warning(push) 
	#pragma warning(disable: 4996) 
#endif

	void TryGetSteamDir()
	{
#ifdef _WIN32
		HKEY hRegKey;

		bool bFallback = true;
		if (RegOpenKeyExA(HKEY_LOCAL_MACHINE, "Software\\Valve\\Steam", 0, KEY_QUERY_VALUE, &hRegKey) == ERROR_SUCCESS)
		{
			DWORD dwLength = sizeof(m_szSteamPath) - 1;
			if(RegQueryValueExA(hRegKey, "InstallPath", NULL, NULL, (BYTE*)m_szSteamPath, &dwLength) == ERROR_SUCCESS)
			{
				m_szSteamPath[dwLength] = '\0';
				bFallback = false;
			}
			RegCloseKey(hRegKey);
		}

		if(bFallback)
		{
			strcpy(m_szSteamPath, ".");
		}
#elif defined(__APPLE_CC__)
		CFURLRef url;
		OSStatus err = LSFindApplicationForInfo(kLSUnknownCreator, CFSTR("com.valvesoftware.steam"), NULL, NULL, &url);
		
		bool bFallback = true;

		if(err == noErr)
		{
			if(CFURLGetFileSystemRepresentation(url, true, (UInt8*)m_szSteamPath, sizeof(m_szSteamPath)))
			{
				strncat(m_szSteamPath, "/Contents/MacOS/osx32/", sizeof(m_szSteamPath));
				bFallback = false;
			}
		}
		CFRelease(url);

		if(bFallback)
		{
			strcpy(m_szSteamPath, ".");
		}
#elif defined(__linux__)
		// We don't know where to find Steam on this platform, so we're going
		// to say it lives in the same directory as our executable
		if(readlink("/proc/self/exe", m_szSteamPath, sizeof(m_szSteamPath)) != -1)
		{
			char *pchSlash = strrchr(m_szSteamPath, '/');

			if(pchSlash)
			{
				*pchSlash = '\0';
				return;
			}
		}

		strcpy(m_szSteamPath, ".");
#endif
	}

	void TryLoadLibraries()
	{
		if(m_eSearchOrder == k_ESearchOrderLocalFirst)
		{
			m_pSteamclient = new DynamicLibrary(k_cszSteam3LibraryName);
			m_pSteam = new DynamicLibrary(k_cszSteam2LibraryName);

			if(!m_pSteamclient->IsLoaded() || !m_pSteam->IsLoaded())
			{
				delete m_pSteamclient;
				m_pSteamclient = NULL;

				delete m_pSteam;
				m_pSteam = NULL;
			}
			else
				return;
		}

#ifdef _WIN32
		// steamclient.dll expects to be able to load tier0_s without an absolute
		// path, so we'll need to add the steam dir to the search path.
		SetDllDirectoryA( m_szSteamPath );
#endif

		char szLibraryPath[k_iPathMaxSize];
		szLibraryPath[sizeof(szLibraryPath) - 1] = '\0';

		snprintf(szLibraryPath, sizeof(szLibraryPath) - 1, "%s/%s", m_szSteamPath, k_cszSteam3LibraryName);
		m_pSteamclient = new DynamicLibrary(szLibraryPath);
		
		snprintf(szLibraryPath, sizeof(szLibraryPath) - 1, "%s/%s", m_szSteamPath, k_cszSteam2LibraryName);
		m_pSteam = new DynamicLibrary(szLibraryPath);
	}

	
	char m_szSteamPath[k_iPathMaxSize];

	DynamicLibrary* m_pSteamclient;
	DynamicLibrary* m_pSteam;

	ESearchOrder m_eSearchOrder;
};

#ifdef _MSC_VER
	#pragma warning(pop) 
#endif

#endif // INTERFACEOSW_H
