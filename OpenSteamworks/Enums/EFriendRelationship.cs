namespace OpenSteamworks.Enums;

public enum EFriendRelationship
{
    None = 0,
    
    /// <summary>
    /// this doesn't get stored; the user has just done an Ignore on an friendship invite
    /// </summary>
	Blocked = 1,
	RequestRecipient = 2,
	Friend = 3,
	RequestInitiator = 4,

    /// <summary>
    /// this is stored; the user has explicit blocked this other user from comments/chat/etc
    /// </summary>
	Ignored = 5,		
	IgnoredFriend = 6,

    /// <summary>
    /// was used by the original implementation of the facebook linking feature, but now unused.
    /// </summary>
	Suggested_DEPRECATED = 7,

	// keep this updated
	Max = 8,
};