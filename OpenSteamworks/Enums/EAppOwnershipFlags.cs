using System;

namespace OpenSteamworks.Enums;

[Flags]
public enum EAppOwnershipFlags : uint
{
	None = 0,
	OwnsLicense = 1,
	FreeLicense = 2,
	RegionRestricted = 4,
	LowViolence = 8,
	InvalidPlatform = 16,
	SharedLicense = 32,
	FreeWeekend = 64,
	RetailLicense = 128,
	LicenseLocked = 256,
	LicensePending = 512,
	LicenseExpired = 1024,
	LicensePermanent = 2048,
	LicenseRecurring = 4096,
	LicenseCanceled = 8192,
	AutoGrant = 16384,
	PendingGift = 32768,
	RentalNotActivated = 65536,
	Rental = 131072,
	SiteLicense = 262144,
	LegacyFreeSub = 524288,
	InvalidOSType = 1048576,
	TimedTrial = 2097152,
}