#pragma once

typedef int HSteamPipe;
typedef int HSteamUser;
typedef int EAccountType;
typedef int EUniverse;
typedef int ENotificationPosition;
typedef int unknown_ret;

typedef unsigned int UInt32;
typedef unsigned short UInt16;
typedef int Int32;
typedef short Int16;
typedef unsigned long UInt64;
typedef UInt64 SteamAPICall_t;

#include <gameid.h>


#if defined(_WIN32) && defined(__GNUC__) && !defined(_S4N_)
	#define STEAMWORKS_STRUCT_RETURN_0(returnType, functionName)	\
		virtual void functionName( returnType& ret ) = 0;			\
		inline returnType functionName()							\
		{															\
			returnType ret;											\
			this->functionName(ret);								\
			return ret;												\
		}
	#define STEAMWORKS_STRUCT_RETURN_1(returnType, functionName, arg1Type, arg1Name)	\
		virtual void functionName( returnType& ret, arg1Type arg1Name ) = 0;			\
		inline returnType functionName( arg1Type arg1Name )								\
		{																				\
			returnType ret;																\
			this->functionName(ret, arg1Name);											\
			return ret;																	\
		}
	#define STEAMWORKS_STRUCT_RETURN_2(returnType, functionName, arg1Type, arg1Name, arg2Type, arg2Name)	\
		virtual void functionName( returnType& ret, arg1Type arg1Name, arg2Type arg2Name ) = 0;				\
		inline returnType functionName( arg1Type arg1Name, arg2Type arg2Name )								\
		{																									\
			returnType ret;																					\
			this->functionName(ret, arg1Name, arg2Name);													\
			return ret;																						\
		}
	#define STEAMWORKS_STRUCT_RETURN_3(returnType, functionName, arg1Type, arg1Name, arg2Type, arg2Name, arg3Type, arg3Name)	\
		virtual void functionName( returnType& ret, arg1Type arg1Name, arg2Type arg2Name, arg3Type arg3Name ) = 0;				\
		inline returnType functionName( arg1Type arg1Name, arg2Type arg2Name, arg3Type arg3Name )								\
		{																														\
			returnType ret;																										\
			this->functionName(ret, arg1Name, arg2Name, arg3Name);																\
			return ret;																											\
		}
#else
	#define STEAMWORKS_STRUCT_RETURN_0(returnType, functionName) virtual returnType functionName() = 0;
	#define STEAMWORKS_STRUCT_RETURN_1(returnType, functionName, arg1Type, arg1Name) virtual returnType functionName( arg1Type arg1Name ) = 0;
	#define STEAMWORKS_STRUCT_RETURN_2(returnType, functionName, arg1Type, arg1Name, arg2Type, arg2Name) virtual returnType functionName( arg1Type arg1Name, arg2Type arg2Name ) = 0;
	#define STEAMWORKS_STRUCT_RETURN_3(returnType, functionName, arg1Type, arg1Name, arg2Type, arg2Name, arg3Type, arg3Name) virtual returnType functionName( arg1Type arg1Name, arg2Type arg2Name, arg3Type arg3Name ) = 0;
#endif