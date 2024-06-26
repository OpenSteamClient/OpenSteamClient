//==========================  AUTO-GENERATED FILE  ================================
//
// This file is partially auto-generated.
// If functions are removed, your changes to that function will be lost.
// Parameter types and names however are preserved if the function stays unchanged.
// Feel free to change parameters to be more accurate. 
//=============================================================================

using System;
using OpenSteamworks.Attributes;

namespace OpenSteamworks.Generated;

public unsafe interface IClientControllerSerialized
{
    // WARNING: Do not use this function! Unknown behaviour will occur!
    public unknown_ret Unknown_0_DONTUSE();  // argc: -1, index: 1, ipc args: [], ipc returns: []
    // WARNING: Do not use this function! Unknown behaviour will occur!
    public unknown_ret Unknown_1_DONTUSE();  // argc: -1, index: 2, ipc args: [], ipc returns: []
    // WARNING: Arguments are unknown!
    public unknown_ret ShowBindingPanel();  // argc: 3, index: 3, ipc args: [bytes4, bytes8], ipc returns: [bytes1]
    // WARNING: Arguments are unknown!
    public unknown_ret GetControllerTypeForHandle();  // argc: 2, index: 4, ipc args: [bytes8], ipc returns: [bytes4]
    // WARNING: Arguments are unknown!
    public unknown_ret GetGamepadIndexForHandle();  // argc: 2, index: 5, ipc args: [bytes8], ipc returns: [bytes4]
    // WARNING: Arguments are unknown!
    public unknown_ret GetHandleForGamepadIndex();  // argc: 1, index: 6, ipc args: [bytes4], ipc returns: [bytes8]
    // WARNING: Arguments are unknown!
    public unknown_ret GetActionSetHandle();  // argc: 2, index: 7, ipc args: [bytes4, string], ipc returns: [bytes8]
    // WARNING: Arguments are unknown!
    public unknown_ret GetActionSetHandleByTitle();  // argc: 2, index: 8, ipc args: [bytes4, string], ipc returns: [bytes8]
    // WARNING: Arguments are unknown!
    public unknown_ret ActivateActionSet();  // argc: 5, index: 9, ipc args: [bytes4, bytes8, bytes8], ipc returns: []
    // WARNING: Arguments are unknown!
    public unknown_ret ActivateActionSetLayer();  // argc: 5, index: 10, ipc args: [bytes4, bytes8, bytes8], ipc returns: []
    // WARNING: Arguments are unknown!
    public unknown_ret DeactivateActionSetLayer();  // argc: 5, index: 11, ipc args: [bytes4, bytes8, bytes8], ipc returns: []
    // WARNING: Arguments are unknown!
    public unknown_ret DeactivateAllActionSetLayers();  // argc: 3, index: 12, ipc args: [bytes4, bytes8], ipc returns: []
    // WARNING: Arguments are unknown!
    [BlacklistedInCrossProcessIPC]
    public unknown_ret GetActiveActionSetLayers();  // argc: 4, index: 13, ipc args: [bytes4, bytes8, bytes4], ipc returns: [bytes4]
    // WARNING: Arguments are unknown!
    public unknown_ret GetDigitalActionHandle();  // argc: 3, index: 14, ipc args: [bytes4, string, bytes1], ipc returns: [bytes8]
    // WARNING: Arguments are unknown!
    public unknown_ret GetAnalogActionHandle();  // argc: 3, index: 15, ipc args: [bytes4, string, bytes1], ipc returns: [bytes8]
    // WARNING: Arguments are unknown!
    public unknown_ret StopAnalogActionMomentum();  // argc: 4, index: 16, ipc args: [bytes8, bytes8], ipc returns: []
    // WARNING: Arguments are unknown!
    public unknown_ret EnableDeviceCallbacks();  // argc: 1, index: 17, ipc args: [bytes4], ipc returns: []
    // WARNING: Arguments are unknown!
    public unknown_ret GetStringForDigitalActionName();  // argc: 5, index: 18, ipc args: [bytes4, bytes8, bytes4], ipc returns: [bytes1, bytes_length_from_mem]
    // WARNING: Arguments are unknown!
    public unknown_ret GetStringForAnalogActionName();  // argc: 5, index: 19, ipc args: [bytes4, bytes8, bytes4], ipc returns: [bytes1, bytes_length_from_mem]
    // WARNING: Arguments are unknown!
    public unknown_ret BCheckGameDirectoryAndReloadConfigIfNecessary();  // argc: 3, index: 20, ipc args: [bytes4, bytes4, bytes_length_from_mem], ipc returns: [boolean]
    // WARNING: Arguments are unknown!
    public unknown_ret GetActionManifestPath(bool unk1);  // argc: 1, index: 21, ipc args: [bytes4], ipc returns: [string]
    // WARNING: Arguments are unknown!
    [BlacklistedInCrossProcessIPC]
    public unknown_ret GetActionManifestPath(double unk1, bool unk2);  // argc: 2, index: 22, ipc args: [bytes4, bytes4], ipc returns: [string]
    // WARNING: Arguments are unknown!
    public unknown_ret DumpConfigurationToDisk();  // argc: 1, index: 23, ipc args: [bytes4], ipc returns: [bytes1]
    public unknown_ret FlushCloudedConfigFilesToDisk();  // argc: 0, index: 24, ipc args: [], ipc returns: []
    // WARNING: Arguments are unknown!
    public unknown_ret StartBindingVisualization();  // argc: 3, index: 25, ipc args: [bytes4, bytes4, bytes4], ipc returns: []
    // WARNING: Arguments are unknown!
    public unknown_ret StopBindingVisualization();  // argc: 2, index: 26, ipc args: [bytes4, bytes4], ipc returns: []
    public unknown_ret GetNumConnectedControllers();  // argc: 0, index: 27, ipc args: [], ipc returns: [bytes4]
    // WARNING: Arguments are unknown!
    [BlacklistedInCrossProcessIPC]
    public unknown_ret GetAllControllersStatus();  // argc: 1, index: 28, ipc args: [bytes4], ipc returns: []
    // WARNING: Arguments are unknown!
    public unknown_ret GetControllerDetails();  // argc: 1, index: 29, ipc args: [bytes4], ipc returns: [bytes8]
    // WARNING: Arguments are unknown!
    public unknown_ret SetDefaultConfig();  // argc: 1, index: 30, ipc args: [bytes4], ipc returns: [bytes1]
    // WARNING: Arguments are unknown!
    public unknown_ret CalibrateTrackpads();  // argc: 1, index: 31, ipc args: [bytes4], ipc returns: [bytes1]
    // WARNING: Arguments are unknown!
    public unknown_ret CalibrateJoystick();  // argc: 1, index: 32, ipc args: [bytes4], ipc returns: [bytes1]
    // WARNING: Arguments are unknown!
    public unknown_ret CalibrateIMU();  // argc: 1, index: 33, ipc args: [bytes4], ipc returns: [bytes1]
    // WARNING: Arguments are unknown!
    [BlacklistedInCrossProcessIPC]
    public unknown_ret SetAudioMapping();  // argc: 2, index: 34, ipc args: [bytes4, bytes4], ipc returns: [bytes1]
    // WARNING: Arguments are unknown!
    public unknown_ret PlayAudio();  // argc: 2, index: 35, ipc args: [bytes4, bytes4], ipc returns: [bytes1]
    // WARNING: Arguments are unknown!
    public unknown_ret ResetStickExtents();  // argc: 1, index: 36, ipc args: [bytes4], ipc returns: [bytes1]
    // WARNING: Arguments are unknown!
    public unknown_ret BIsStreamingController();  // argc: 1, index: 37, ipc args: [bytes4], ipc returns: [boolean]
    // WARNING: Arguments are unknown!
    public unknown_ret SetUserLedColor();  // argc: 4, index: 38, ipc args: [bytes4, bytes1, bytes1, bytes1], ipc returns: [bytes1]
    // WARNING: Arguments are unknown!
    public unknown_ret SetRumble();  // argc: 5, index: 39, ipc args: [bytes4, bytes4, bytes4, bytes2, bytes2], ipc returns: [bytes1]
    // WARNING: Arguments are unknown!
    public unknown_ret SetRumbleExtended();  // argc: 7, index: 40, ipc args: [bytes4, bytes4, bytes4, bytes2, bytes2, bytes2, bytes2], ipc returns: [bytes1]
    // WARNING: Arguments are unknown!
    public unknown_ret IdentifyControllerRumbleEffect();  // argc: 1, index: 41, ipc args: [bytes4], ipc returns: [bytes1]
    // WARNING: Arguments are unknown!
    public unknown_ret SetGyroAutoCalibrate();  // argc: 2, index: 42, ipc args: [bytes4, bytes1], ipc returns: [bytes1]
    // WARNING: Arguments are unknown!
    public unknown_ret RequestGyroActive();  // argc: 3, index: 43, ipc args: [bytes4, bytes4, bytes1], ipc returns: []
    // WARNING: Arguments are unknown!
    public unknown_ret SetGyroCalibrating();  // argc: 2, index: 44, ipc args: [bytes4, bytes1], ipc returns: []
    // WARNING: Arguments are unknown!
    [BlacklistedInCrossProcessIPC]
    public unknown_ret LoadConfigFromVDFString();  // argc: 7, index: 45, ipc args: [bytes4, bytes4, string, bytes4, bytes8, bytes4], ipc returns: []
    public unknown_ret InvalidateBindingCache();  // argc: 0, index: 46, ipc args: [], ipc returns: []
    // WARNING: Arguments are unknown!
    public unknown_ret ActivateConfig();  // argc: 2, index: 47, ipc args: [bytes4, bytes4], ipc returns: []
    // WARNING: Arguments are unknown!
    public unknown_ret WarmOptInStatus();  // argc: 2, index: 48, ipc args: [bytes4, bytes4], ipc returns: []
    // WARNING: Arguments are unknown!
    public unknown_ret GetCurrentActionSetHandleForRunningApp();  // argc: 2, index: 49, ipc args: [bytes4, bytes4], ipc returns: [bytes8]
    // WARNING: Arguments are unknown!
    public unknown_ret GetGamepadIndexForControllerIndex();  // argc: 1, index: 50, ipc args: [bytes4], ipc returns: [bytes4]
    // WARNING: Arguments are unknown!
    [BlacklistedInCrossProcessIPC]
    public unknown_ret CreateBindingInstanceFromVDFString();  // argc: 1, index: 51, ipc args: [bytes4], ipc returns: [bytes4]
    // WARNING: Arguments are unknown!
    public unknown_ret FreeBindingInstance();  // argc: 1, index: 52, ipc args: [bytes4], ipc returns: [bytes1]
    // WARNING: Arguments are unknown!
    public unknown_ret GetControllerConfiguration();  // argc: 2, index: 53, ipc args: [bytes4], ipc returns: [bytes4, protobuf]
    // WARNING: Arguments are unknown!
    public unknown_ret SetControllerActionSet();  // argc: 3, index: 54, ipc args: [bytes4, protobuf], ipc returns: [bytes4, protobuf]
    // WARNING: Arguments are unknown!
    public unknown_ret SetControllerSourceMode();  // argc: 3, index: 55, ipc args: [bytes4, protobuf], ipc returns: [bytes4, protobuf]
    // WARNING: Arguments are unknown!
    public unknown_ret DuplicateControllerSourceMode();  // argc: 3, index: 56, ipc args: [bytes4, protobuf], ipc returns: [bytes4, protobuf]
    // WARNING: Arguments are unknown!
    public unknown_ret SwapControllerConfigurationSourceModes();  // argc: 3, index: 57, ipc args: [bytes4, protobuf], ipc returns: [bytes4, protobuf]
    // WARNING: Arguments are unknown!
    public unknown_ret SetControllerInputActivator();  // argc: 3, index: 58, ipc args: [bytes4, protobuf], ipc returns: [bytes4, protobuf]
    // WARNING: Arguments are unknown!
    public unknown_ret SetControllerInputBinding();  // argc: 3, index: 59, ipc args: [bytes4, protobuf], ipc returns: [bytes4, protobuf]
    // WARNING: Arguments are unknown!
    public unknown_ret SetControllerInputActivatorEnabled();  // argc: 3, index: 60, ipc args: [bytes4, protobuf], ipc returns: [bytes4, protobuf]
    // WARNING: Arguments are unknown!
    public unknown_ret SetControllerMiscMappingSettings();  // argc: 3, index: 61, ipc args: [bytes4, protobuf], ipc returns: [bytes4, protobuf]
    // WARNING: Arguments are unknown!
    public unknown_ret SwapControllerModeInputBindings();  // argc: 3, index: 62, ipc args: [bytes4, protobuf], ipc returns: [bytes4, protobuf]
    // WARNING: Arguments are unknown!
    public unknown_ret SetControllerModeShiftBinding();  // argc: 3, index: 63, ipc args: [bytes4, protobuf], ipc returns: [bytes4, protobuf]
    // WARNING: Arguments are unknown!
    public unknown_ret IsModified();  // argc: 1, index: 64, ipc args: [bytes4], ipc returns: [boolean]
    // WARNING: Arguments are unknown!
    public unknown_ret ClearModified();  // argc: 1, index: 65, ipc args: [bytes4], ipc returns: []
    // WARNING: Arguments are unknown!
    public unknown_ret GetLocalizationTokenCount();  // argc: 1, index: 66, ipc args: [bytes4], ipc returns: [bytes4]
    // WARNING: Arguments are unknown!
    [BlacklistedInCrossProcessIPC]
    public unknown_ret GetLocalizationToken();  // argc: 3, index: 67, ipc args: [bytes4, bytes4, bytes4], ipc returns: [bytes1]
    // WARNING: Arguments are unknown!
    public unknown_ret GetLocalizedString();  // argc: 2, index: 68, ipc args: [bytes4, string], ipc returns: [string]
    // WARNING: Arguments are unknown!
    public unknown_ret GetBindingVDFString();  // argc: 1, index: 69, ipc args: [bytes4], ipc returns: [string]
    // WARNING: Arguments are unknown!
    public unknown_ret GetBindingTitle();  // argc: 2, index: 70, ipc args: [bytes4, bytes1], ipc returns: [string]
    // WARNING: Arguments are unknown!
    public unknown_ret SetBindingTitle();  // argc: 2, index: 71, ipc args: [bytes4, string], ipc returns: []
    // WARNING: Arguments are unknown!
    public unknown_ret GetBindingDescription();  // argc: 2, index: 72, ipc args: [bytes4, bytes1], ipc returns: [string]
    // WARNING: Arguments are unknown!
    public unknown_ret GetBindingRevision();  // argc: 3, index: 73, ipc args: [bytes4], ipc returns: [bytes1, bytes4, bytes4]
    // WARNING: Arguments are unknown!
    public unknown_ret BBindingMajorRevisionMismatch();  // argc: 1, index: 74, ipc args: [bytes4], ipc returns: [boolean]
    // WARNING: Arguments are unknown!
    public unknown_ret SetBindingDescription();  // argc: 2, index: 75, ipc args: [bytes4, string], ipc returns: []
    // WARNING: Arguments are unknown!
    [BlacklistedInCrossProcessIPC]
    public unknown_ret GetConfigBindingInfo();  // argc: 2, index: 76, ipc args: [bytes4, bytes4], ipc returns: []
    // WARNING: Arguments are unknown!
    public unknown_ret SetBindingControllerType();  // argc: 2, index: 77, ipc args: [bytes4, bytes4], ipc returns: []
    // WARNING: Arguments are unknown!
    public unknown_ret GetBindingControllerType();  // argc: 1, index: 78, ipc args: [bytes4], ipc returns: [bytes4]
    // WARNING: Arguments are unknown!
    public unknown_ret SetBindingCreator();  // argc: 3, index: 79, ipc args: [bytes4, bytes8], ipc returns: []
    // WARNING: Arguments are unknown!
    public unknown_ret GetBindingCreator();  // argc: 1, index: 80, ipc args: [bytes4], ipc returns: [bytes8]
    // WARNING: Arguments are unknown!
    public unknown_ret GetBindingProgenitor();  // argc: 1, index: 81, ipc args: [bytes4], ipc returns: [string]
    // WARNING: Arguments are unknown!
    public unknown_ret SetBindingProgenitor();  // argc: 2, index: 82, ipc args: [bytes4, string], ipc returns: []
    // WARNING: Arguments are unknown!
    public unknown_ret GetBindingURL();  // argc: 1, index: 83, ipc args: [bytes4], ipc returns: [string]
    // WARNING: Arguments are unknown!
    public unknown_ret SetBindingURL();  // argc: 2, index: 84, ipc args: [bytes4, string], ipc returns: []
    // WARNING: Arguments are unknown!
    public unknown_ret GetBindingExportType();  // argc: 1, index: 85, ipc args: [bytes4], ipc returns: [bytes4]
    // WARNING: Arguments are unknown!
    public unknown_ret SetBindingExportType();  // argc: 2, index: 86, ipc args: [bytes4, bytes4], ipc returns: []
    // WARNING: Arguments are unknown!
    [BlacklistedInCrossProcessIPC]
    public unknown_ret GetConfigFeatures();  // argc: 2, index: 87, ipc args: [bytes4, bytes4], ipc returns: []
    // WARNING: Arguments are unknown!
    public unknown_ret BIsXInputActiveForController();  // argc: 1, index: 88, ipc args: [bytes4], ipc returns: [boolean]
    // WARNING: Arguments are unknown!
    public unknown_ret PS4SettingsChanged();  // argc: 1, index: 89, ipc args: [bytes1], ipc returns: []
    // WARNING: Arguments are unknown!
    public unknown_ret SwitchSettingsChanged();  // argc: 1, index: 90, ipc args: [bytes1], ipc returns: []
    // WARNING: Arguments are unknown!
    public unknown_ret ControllerSettingsChanged();  // argc: 1, index: 91, ipc args: [bytes4], ipc returns: []
    // WARNING: Arguments are unknown!
    public unknown_ret SetTrackpadPressureCurve();  // argc: 3, index: 92, ipc args: [bytes4, bytes4, bytes4], ipc returns: []
    // WARNING: Arguments are unknown!
    public unknown_ret SetDefaultNintendoButtonLayout();  // argc: 1, index: 93, ipc args: [bytes1], ipc returns: []
    // WARNING: Arguments are unknown!
    public unknown_ret IsControllerConnected();  // argc: 2, index: 94, ipc args: [bytes4, bytes1], ipc returns: [boolean]
    // WARNING: Arguments are unknown!
    public unknown_ret TriggerHapticPulse();  // argc: 6, index: 95, ipc args: [bytes4, bytes4, bytes2, bytes2, bytes2, bytes4], ipc returns: []
    // WARNING: Arguments are unknown!
    public unknown_ret TriggerSimpleHapticEvent();  // argc: 5, index: 96, ipc args: [bytes4, bytes1, bytes1, bytes1, bytes1], ipc returns: []
    // WARNING: Arguments are unknown!
    public unknown_ret TriggerVibration();  // argc: 4, index: 97, ipc args: [bytes4, bytes4, bytes2, bytes2], ipc returns: []
    // WARNING: Arguments are unknown!
    public unknown_ret TriggerVibrationExtended();  // argc: 6, index: 98, ipc args: [bytes4, bytes4, bytes2, bytes2, bytes2, bytes2], ipc returns: []
    // WARNING: Arguments are unknown!
    public unknown_ret SetLEDColor();  // argc: 5, index: 99, ipc args: [bytes4, bytes1, bytes1, bytes1, bytes4], ipc returns: []
    // WARNING: Arguments are unknown!
    public unknown_ret SetDonglePairingMode();  // argc: 2, index: 100, ipc args: [bytes1, bytes4], ipc returns: []
    public unknown_ret ReserveSteamController();  // argc: 0, index: 101, ipc args: [], ipc returns: []
    public unknown_ret CancelSteamControllerReservations();  // argc: 0, index: 102, ipc args: [], ipc returns: []
    // WARNING: Arguments are unknown!
    public unknown_ret OpenStreamingSession();  // argc: 2, index: 103, ipc args: [bytes4, bytes4], ipc returns: []
    // WARNING: Arguments are unknown!
    public unknown_ret CloseStreamingSession();  // argc: 2, index: 104, ipc args: [bytes4, bytes4], ipc returns: []
    // WARNING: Arguments are unknown!
    public unknown_ret UpdateStreamingSessionInputPermissions();  // argc: 1, index: 105, ipc args: [bytes4], ipc returns: []
    // WARNING: Arguments are unknown!
    public unknown_ret InitiateISPFirmwareUpdate();  // argc: 1, index: 106, ipc args: [bytes4], ipc returns: [bytes8]
    // WARNING: Arguments are unknown!
    public unknown_ret InitiateBootloaderFirmwareUpdate();  // argc: 1, index: 107, ipc args: [bytes4], ipc returns: [bytes8]
    // WARNING: Arguments are unknown!
    public unknown_ret FlashControllerFirmware();  // argc: 4, index: 108, ipc args: [bytes4, utlbuffer, bytes4, string], ipc returns: [bytes8]
    // WARNING: Arguments are unknown!
    public unknown_ret TurnOffController();  // argc: 1, index: 109, ipc args: [bytes4], ipc returns: []
    public unknown_ret EnumerateControllers();  // argc: 0, index: 110, ipc args: [], ipc returns: []
    // WARNING: Arguments are unknown!
    public unknown_ret GetControllerStatusEvent();  // argc: 2, index: 111, ipc args: [bytes4], ipc returns: [bytes1, bytes12]
    // WARNING: Arguments are unknown!
    public unknown_ret GetActualControllerDetails();  // argc: 2, index: 112, ipc args: [bytes4], ipc returns: [bytes1, bytes80]
    // WARNING: Arguments are unknown!
    [BlacklistedInCrossProcessIPC]
    public unknown_ret GetControllerIdentity();  // argc: 2, index: 113, ipc args: [bytes4, bytes4], ipc returns: [bytes1]
    // WARNING: Arguments are unknown!
    [BlacklistedInCrossProcessIPC]
    public unknown_ret GetControllerPersonalization();  // argc: 2, index: 114, ipc args: [bytes4, bytes4], ipc returns: [bytes1]
    // WARNING: Arguments are unknown!
    public unknown_ret GetControllerReverseDiamondLayout();  // argc: 1, index: 115, ipc args: [bytes4], ipc returns: [bytes1]
    // WARNING: Arguments are unknown!
    public unknown_ret BRumbleEnabledByUser();  // argc: 1, index: 116, ipc args: [bytes4], ipc returns: [boolean]
    // WARNING: Arguments are unknown!
    public unknown_ret BHapticsEnabledByUser();  // argc: 1, index: 117, ipc args: [bytes4], ipc returns: [boolean]
    // WARNING: Arguments are unknown!
    public unknown_ret SetControllerPairingConnectionState();  // argc: 2, index: 118, ipc args: [bytes4, bytes1], ipc returns: []
    // WARNING: Arguments are unknown!
    public unknown_ret SetControllerKeyboardMouseState();  // argc: 2, index: 119, ipc args: [bytes4, bytes1], ipc returns: []
    // WARNING: Arguments are unknown!
    [BlacklistedInCrossProcessIPC]
    public unknown_ret GetTouchKeysForPopupMenu();  // argc: 4, index: 120, ipc args: [bytes4, bytes4, bytes4, bytes4], ipc returns: [bytes4]
    // WARNING: Arguments are unknown!
    public unknown_ret PopupMenuTouchKeyClicked();  // argc: 3, index: 121, ipc args: [bytes4, bytes4, bytes4], ipc returns: []
    // WARNING: Arguments are unknown!
    public unknown_ret AccessControllerInputGeneratorMouseButton();  // argc: 3, index: 122, ipc args: [bytes4, bytes4, bytes1], ipc returns: []
    // WARNING: Arguments are unknown!
    [BlacklistedInCrossProcessIPC]
    public unknown_ret SetControllerSetting();  // argc: 2, index: 123, ipc args: [bytes4, bytes4], ipc returns: []
    [BlacklistedInCrossProcessIPC]
    public unknown_ret GetEmulatedOutputState();  // argc: 0, index: 124, ipc args: [], ipc returns: [bytes4]
    // WARNING: Arguments are unknown!
    [BlacklistedInCrossProcessIPC]
    public unknown_ret SetSelectedConfigForApp();  // argc: 7, index: 125, ipc args: [bytes4, bytes4, bytes8, bytes4, bytes1, bytes1], ipc returns: []
    // WARNING: Arguments are unknown!
    [BlacklistedInCrossProcessIPC]
    public unknown_ret BControllerHasUniqueConfigForAppID();  // argc: 2, index: 126, ipc args: [bytes4, bytes4], ipc returns: [boolean]
    // WARNING: Arguments are unknown!
    public unknown_ret DeRegisterController();  // argc: 2, index: 127, ipc args: [bytes4, bytes4], ipc returns: []
    // WARNING: Arguments are unknown!
    public unknown_ret SendOSKeyboardEvent();  // argc: 1, index: 128, ipc args: [string], ipc returns: []
    // WARNING: Arguments are unknown!
    public unknown_ret SetOSKeyboardKey();  // argc: 2, index: 129, ipc args: [bytes4, bytes1], ipc returns: []
    // WARNING: Arguments are unknown!
    public unknown_ret SetMousePosition();  // argc: 3, index: 130, ipc args: [bytes4, bytes4, bytes4], ipc returns: []
    public unknown_ret GetGamepadIndexChangeCounter();  // argc: 0, index: 131, ipc args: [], ipc returns: [bytes4]
    // WARNING: Arguments are unknown!
    public unknown_ret BSwapGamepadIndex();  // argc: 3, index: 132, ipc args: [bytes4, bytes4, bytes4], ipc returns: [boolean]
    // WARNING: Arguments are unknown!
    public unknown_ret GetGamepadIndexForXInputIndex();  // argc: 1, index: 133, ipc args: [bytes4], ipc returns: [bytes4]
    // WARNING: Arguments are unknown!
    public unknown_ret GetControllerIndexForGamepadIndex();  // argc: 1, index: 134, ipc args: [bytes4], ipc returns: [bytes4]
    // WARNING: Arguments are unknown!
    public unknown_ret SetControllerActiveAccount();  // argc: 2, index: 135, ipc args: [bytes4, bytes4], ipc returns: []
    // WARNING: Arguments are unknown!
    public unknown_ret StartControllerRegistrationToAccount();  // argc: 2, index: 136, ipc args: [bytes4, bytes4], ipc returns: []
    // WARNING: Arguments are unknown!
    public unknown_ret CompleteControllerRegistrationToAccount();  // argc: 2, index: 137, ipc args: [bytes4, bytes4], ipc returns: []
    // WARNING: Arguments are unknown!
    public unknown_ret AutoRegisterControllerRegistrationToAccount();  // argc: 2, index: 138, ipc args: [bytes4, bytes4], ipc returns: []
    // WARNING: Arguments are unknown!
    [BlacklistedInCrossProcessIPC]
    public unknown_ret GetConfigForAppAndController();  // argc: 4, index: 139, ipc args: [bytes4, string, bytes4, bytes4], ipc returns: [bytes1]
    // WARNING: Arguments are unknown!
    [BlacklistedInCrossProcessIPC]
    public unknown_ret SetControllerPersonalization();  // argc: 3, index: 140, ipc args: [bytes4, bytes4, bytes1], ipc returns: []
    // WARNING: Arguments are unknown!
    public unknown_ret SetPersonalizationFile();  // argc: 4, index: 141, ipc args: [bytes4, bytes4, bytes8], ipc returns: []
    // WARNING: Arguments are unknown!
    public unknown_ret SetGameWindowPos();  // argc: 4, index: 142, ipc args: [bytes4, bytes4, bytes4, bytes4], ipc returns: []
    // WARNING: Arguments are unknown!
    [BlacklistedInCrossProcessIPC]
    public unknown_ret GetGameWindowPos();  // argc: 4, index: 143, ipc args: [bytes4, bytes4, bytes4, bytes4], ipc returns: []
    // WARNING: Arguments are unknown!
    public unknown_ret HasGameMapping();  // argc: 1, index: 144, ipc args: [bytes4], ipc returns: [bytes1]
    // WARNING: Arguments are unknown!
    public unknown_ret GetControllerUsageData();  // argc: 2, index: 145, ipc args: [bytes4], ipc returns: []
    // WARNING: Arguments are unknown!
    public unknown_ret BAllowAppConfigForController();  // argc: 2, index: 146, ipc args: [bytes4, bytes4], ipc returns: [boolean]
    public unknown_ret ResetControllerEnableCache();  // argc: 0, index: 147, ipc args: [], ipc returns: []
    // WARNING: Arguments are unknown!
    public unknown_ret GetControllerEnableSupport();  // argc: 1, index: 148, ipc args: [bytes4], ipc returns: [bytes4]
    public unknown_ret BInputGenerated();  // argc: 0, index: 149, ipc args: [], ipc returns: [boolean]
    // WARNING: Arguments are unknown!
    public unknown_ret GetControllerActivityByType();  // argc: 1, index: 150, ipc args: [bytes4], ipc returns: [bytes4]
    public unknown_ret GetLastActiveControllerVID();  // argc: 0, index: 151, ipc args: [], ipc returns: [bytes2]
    public unknown_ret GetLastActiveControllerPID();  // argc: 0, index: 152, ipc args: [], ipc returns: [bytes2]
    // WARNING: Arguments are unknown!
    public unknown_ret LoadControllerPersonalizationFile();  // argc: 4, index: 153, ipc args: [bytes4, string, bytes1, bytes1], ipc returns: []
    // WARNING: Arguments are unknown!
    [BlacklistedInCrossProcessIPC]
    public unknown_ret SaveControllerPersonalizationFile();  // argc: 3, index: 154, ipc args: [bytes4, bytes4, bytes4], ipc returns: [bytes1]
    // WARNING: Arguments are unknown!
    public unknown_ret LoadRemotePlayControllerPersonalizationVDF();  // argc: 2, index: 155, ipc args: [string, string], ipc returns: []
    // WARNING: Arguments are unknown!
    public unknown_ret FindControllerByPath();  // argc: 1, index: 156, ipc args: [string], ipc returns: [bytes4]
    // WARNING: Arguments are unknown!
    [BlacklistedInCrossProcessIPC]
    public unknown_ret GetControllerPath();  // argc: 2, index: 157, ipc args: [bytes4, bytes4], ipc returns: [bytes1]
    // WARNING: Arguments are unknown!
    [BlacklistedInCrossProcessIPC]
    public unknown_ret GetControllerProductName();  // argc: 2, index: 158, ipc args: [bytes4, bytes4], ipc returns: [bytes1]
    // WARNING: Arguments are unknown!
    public unknown_ret SetControllerHapticsSetting();  // argc: 2, index: 159, ipc args: [bytes4, bytes1], ipc returns: [bytes1]
    // WARNING: Arguments are unknown!
    public unknown_ret SetControllerName();  // argc: 2, index: 160, ipc args: [bytes4, string], ipc returns: [bytes1]
    // WARNING: Arguments are unknown!
    public unknown_ret SetControllerRumbleSetting();  // argc: 2, index: 161, ipc args: [bytes4, bytes4], ipc returns: [bytes1]
    // WARNING: Arguments are unknown!
    public unknown_ret SetControllerNintendoLayoutSetting();  // argc: 2, index: 162, ipc args: [bytes4, bytes1], ipc returns: [bytes1]
    public unknown_ret SetControllerUseUniversalFaceButtonGlyphs();  // argc: 2, index: 163, ipc args: [bytes4, bytes1], ipc returns: [bytes1]
    // WARNING: Arguments are unknown!
    public bool BGetTouchConfigData(uint unk, uint unk1, out UInt64 unk2, out uint unk3, CUtlBuffer* unk4, CUtlBuffer* unk5);  // argc: 6, index: 164, ipc args: [bytes4, bytes4], ipc returns: [boolean, bytes8, bytes4, utlbuffer, utlbuffer]
    // WARNING: Arguments are unknown!
    [BlacklistedInCrossProcessIPC]
    public unknown_ret BSaveTouchConfigLayout();  // argc: 3, index: 165, ipc args: [bytes4, bytes4, bytes4], ipc returns: [boolean]
    // WARNING: Arguments are unknown!
    public unknown_ret SetGyroOn();  // argc: 3, index: 166, ipc args: [bytes4, bytes8], ipc returns: []
    // WARNING: Arguments are unknown!
    public unknown_ret CursorVisibilityChanged();  // argc: 1, index: 167, ipc args: [bytes1], ipc returns: []
    // WARNING: Arguments are unknown!
    public unknown_ret ForceSimpleHapticEvent();  // argc: 5, index: 168, ipc args: [bytes4, bytes1, bytes1, bytes1, bytes1], ipc returns: []
    public unknown_ret GetControllerMacAddr();  // argc: 3, index: 169, ipc args: [bytes4, string, string], ipc returns: [bytes1, bytes13, bytes13]
}