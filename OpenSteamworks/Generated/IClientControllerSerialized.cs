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
    public unknown_ret StartBindingVisualization();  // argc: 3, index: 0, ipc args: [bytes4, bytes4, bytes4], ipc returns: []
    // WARNING: Arguments are unknown!
    public unknown_ret StopBindingVisualization();  // argc: 2, index: 1, ipc args: [bytes4, bytes4], ipc returns: []
    public unknown_ret GetNumConnectedControllers();  // argc: 0, index: 2, ipc args: [], ipc returns: [bytes4]
    // WARNING: Arguments are unknown!
    [BlacklistedInCrossProcessIPC]
    public unknown_ret GetAllControllersStatus();  // argc: 1, index: 0, ipc args: [bytes4], ipc returns: []
    // WARNING: Arguments are unknown!
    public unknown_ret GetControllerDetails();  // argc: 1, index: 1, ipc args: [bytes4], ipc returns: [bytes8]
    // WARNING: Arguments are unknown!
    public unknown_ret SetDefaultConfig();  // argc: 1, index: 2, ipc args: [bytes4], ipc returns: [bytes1]
    // WARNING: Arguments are unknown!
    public unknown_ret CalibrateTrackpads();  // argc: 1, index: 3, ipc args: [bytes4], ipc returns: [bytes1]
    // WARNING: Arguments are unknown!
    public unknown_ret CalibrateJoystick();  // argc: 1, index: 4, ipc args: [bytes4], ipc returns: [bytes1]
    // WARNING: Arguments are unknown!
    public unknown_ret CalibrateIMU();  // argc: 1, index: 5, ipc args: [bytes4], ipc returns: [bytes1]
    // WARNING: Arguments are unknown!
    [BlacklistedInCrossProcessIPC]
    public unknown_ret SetAudioMapping();  // argc: 2, index: 6, ipc args: [bytes4, bytes4], ipc returns: [bytes1]
    // WARNING: Arguments are unknown!
    public unknown_ret PlayAudio();  // argc: 2, index: 7, ipc args: [bytes4, bytes4], ipc returns: [bytes1]
    // WARNING: Arguments are unknown!
    public unknown_ret ResetStickExtents();  // argc: 1, index: 8, ipc args: [bytes4], ipc returns: [bytes1]
    // WARNING: Arguments are unknown!
    public unknown_ret BIsStreamingController();  // argc: 1, index: 9, ipc args: [bytes4], ipc returns: [boolean]
    // WARNING: Arguments are unknown!
    public unknown_ret SetUserLedColor();  // argc: 4, index: 10, ipc args: [bytes4, bytes1, bytes1, bytes1], ipc returns: [bytes1]
    // WARNING: Arguments are unknown!
    public unknown_ret SetRumble();  // argc: 5, index: 11, ipc args: [bytes4, bytes4, bytes4, bytes2, bytes2], ipc returns: [bytes1]
    // WARNING: Arguments are unknown!
    public unknown_ret SetRumbleExtended();  // argc: 7, index: 12, ipc args: [bytes4, bytes4, bytes4, bytes2, bytes2, bytes2, bytes2], ipc returns: [bytes1]
    // WARNING: Arguments are unknown!
    public unknown_ret IdentifyControllerRumbleEffect();  // argc: 1, index: 13, ipc args: [bytes4], ipc returns: [bytes1]
    // WARNING: Arguments are unknown!
    public unknown_ret SetGyroAutoCalibrate();  // argc: 2, index: 14, ipc args: [bytes4, bytes1], ipc returns: [bytes1]
    // WARNING: Arguments are unknown!
    public unknown_ret RequestGyroActive();  // argc: 3, index: 15, ipc args: [bytes4, bytes4, bytes1], ipc returns: []
    // WARNING: Arguments are unknown!
    public unknown_ret SetGyroCalibrating();  // argc: 2, index: 16, ipc args: [bytes4, bytes1], ipc returns: []
    // WARNING: Arguments are unknown!
    [BlacklistedInCrossProcessIPC]
    public unknown_ret LoadConfigFromVDFString();  // argc: 7, index: 17, ipc args: [bytes4, bytes4, string, bytes4, bytes8, bytes4], ipc returns: []
    public unknown_ret InvalidateBindingCache();  // argc: 0, index: 18, ipc args: [], ipc returns: []
    // WARNING: Arguments are unknown!
    public unknown_ret ActivateConfig();  // argc: 2, index: 0, ipc args: [bytes4, bytes4], ipc returns: []
    // WARNING: Arguments are unknown!
    public unknown_ret WarmOptInStatus();  // argc: 2, index: 1, ipc args: [bytes4, bytes4], ipc returns: []
    // WARNING: Arguments are unknown!
    public unknown_ret GetCurrentActionSetHandleForRunningApp();  // argc: 2, index: 2, ipc args: [bytes4, bytes4], ipc returns: [bytes8]
    // WARNING: Arguments are unknown!
    public unknown_ret GetGamepadIndexForControllerIndex();  // argc: 1, index: 3, ipc args: [bytes4], ipc returns: [bytes4]
    // WARNING: Arguments are unknown!
    [BlacklistedInCrossProcessIPC]
    public unknown_ret CreateBindingInstanceFromVDFString();  // argc: 1, index: 4, ipc args: [bytes4], ipc returns: [bytes4]
    // WARNING: Arguments are unknown!
    public unknown_ret FreeBindingInstance();  // argc: 1, index: 5, ipc args: [bytes4], ipc returns: [bytes1]
    // WARNING: Arguments are unknown!
    public unknown_ret GetControllerConfiguration();  // argc: 2, index: 6, ipc args: [bytes4], ipc returns: [bytes4, unknown]
    // WARNING: Arguments are unknown!
    public unknown_ret SetControllerActionSet();  // argc: 3, index: 7, ipc args: [bytes4, protobuf], ipc returns: [bytes4, unknown]
    // WARNING: Arguments are unknown!
    public unknown_ret SetControllerSourceMode();  // argc: 3, index: 8, ipc args: [bytes4, protobuf], ipc returns: [bytes4, unknown]
    // WARNING: Arguments are unknown!
    public unknown_ret DuplicateControllerSourceMode();  // argc: 3, index: 9, ipc args: [bytes4, protobuf], ipc returns: [bytes4, unknown]
    // WARNING: Arguments are unknown!
    public unknown_ret SetControllerInputActivator();  // argc: 3, index: 10, ipc args: [bytes4, protobuf], ipc returns: [bytes4, unknown]
    // WARNING: Arguments are unknown!
    public unknown_ret SetControllerInputBinding();  // argc: 3, index: 11, ipc args: [bytes4, protobuf], ipc returns: [bytes4, unknown]
    // WARNING: Arguments are unknown!
    public unknown_ret SetControllerInputActivatorEnabled();  // argc: 3, index: 12, ipc args: [bytes4, protobuf], ipc returns: [bytes4, unknown]
    // WARNING: Arguments are unknown!
    public unknown_ret SetControllerMiscMappingSettings();  // argc: 3, index: 13, ipc args: [bytes4, protobuf], ipc returns: [bytes4, unknown]
    // WARNING: Arguments are unknown!
    public unknown_ret SwapControllerModeInputBindings();  // argc: 3, index: 14, ipc args: [bytes4, protobuf], ipc returns: [bytes4, unknown]
    // WARNING: Arguments are unknown!
    public unknown_ret SetControllerModeShiftBinding();  // argc: 3, index: 15, ipc args: [bytes4, protobuf], ipc returns: [bytes4, unknown]
    // WARNING: Arguments are unknown!
    public unknown_ret IsModified();  // argc: 1, index: 16, ipc args: [bytes4], ipc returns: [boolean]
    // WARNING: Arguments are unknown!
    public unknown_ret ClearModified();  // argc: 1, index: 17, ipc args: [bytes4], ipc returns: []
    // WARNING: Arguments are unknown!
    public unknown_ret GetLocalizationTokenCount();  // argc: 1, index: 18, ipc args: [bytes4], ipc returns: [bytes4]
    // WARNING: Arguments are unknown!
    [BlacklistedInCrossProcessIPC]
    public unknown_ret GetLocalizationToken();  // argc: 3, index: 19, ipc args: [bytes4, bytes4, bytes4], ipc returns: [bytes1]
    // WARNING: Arguments are unknown!
    public unknown_ret GetLocalizedString();  // argc: 2, index: 20, ipc args: [bytes4, string], ipc returns: [string]
    // WARNING: Arguments are unknown!
    public unknown_ret GetBindingVDFString();  // argc: 1, index: 21, ipc args: [bytes4], ipc returns: [string]
    // WARNING: Arguments are unknown!
    public unknown_ret GetBindingTitle();  // argc: 2, index: 22, ipc args: [bytes4, bytes1], ipc returns: [string]
    // WARNING: Arguments are unknown!
    public unknown_ret SetBindingTitle();  // argc: 2, index: 23, ipc args: [bytes4, string], ipc returns: []
    // WARNING: Arguments are unknown!
    public unknown_ret GetBindingDescription();  // argc: 2, index: 24, ipc args: [bytes4, bytes1], ipc returns: [string]
    // WARNING: Arguments are unknown!
    public unknown_ret GetBindingRevision();  // argc: 3, index: 25, ipc args: [bytes4], ipc returns: [bytes1, bytes4, bytes4]
    // WARNING: Arguments are unknown!
    public unknown_ret BBindingMajorRevisionMismatch();  // argc: 1, index: 26, ipc args: [bytes4], ipc returns: [boolean]
    // WARNING: Arguments are unknown!
    public unknown_ret SetBindingDescription();  // argc: 2, index: 27, ipc args: [bytes4, string], ipc returns: []
    // WARNING: Arguments are unknown!
    [BlacklistedInCrossProcessIPC]
    public unknown_ret GetConfigBindingInfo();  // argc: 2, index: 28, ipc args: [bytes4, bytes4], ipc returns: []
    // WARNING: Arguments are unknown!
    public unknown_ret SetBindingControllerType();  // argc: 2, index: 29, ipc args: [bytes4, bytes4], ipc returns: []
    // WARNING: Arguments are unknown!
    public unknown_ret GetBindingControllerType();  // argc: 1, index: 30, ipc args: [bytes4], ipc returns: [bytes4]
    // WARNING: Arguments are unknown!
    public unknown_ret SetBindingCreator();  // argc: 3, index: 31, ipc args: [bytes4, bytes8], ipc returns: []
    // WARNING: Arguments are unknown!
    public unknown_ret GetBindingCreator();  // argc: 1, index: 32, ipc args: [bytes4], ipc returns: [bytes8]
    // WARNING: Arguments are unknown!
    public unknown_ret GetBindingProgenitor();  // argc: 1, index: 33, ipc args: [bytes4], ipc returns: [string]
    // WARNING: Arguments are unknown!
    public unknown_ret SetBindingProgenitor();  // argc: 2, index: 34, ipc args: [bytes4, string], ipc returns: []
    // WARNING: Arguments are unknown!
    public unknown_ret GetBindingURL();  // argc: 1, index: 35, ipc args: [bytes4], ipc returns: [string]
    // WARNING: Arguments are unknown!
    public unknown_ret SetBindingURL();  // argc: 2, index: 36, ipc args: [bytes4, string], ipc returns: []
    // WARNING: Arguments are unknown!
    public unknown_ret GetBindingExportType();  // argc: 1, index: 37, ipc args: [bytes4], ipc returns: [bytes4]
    // WARNING: Arguments are unknown!
    public unknown_ret SetBindingExportType();  // argc: 2, index: 38, ipc args: [bytes4, bytes4], ipc returns: []
    // WARNING: Arguments are unknown!
    [BlacklistedInCrossProcessIPC]
    public unknown_ret GetConfigFeatures();  // argc: 2, index: 39, ipc args: [bytes4, bytes4], ipc returns: []
    // WARNING: Arguments are unknown!
    public unknown_ret BIsXInputActiveForController();  // argc: 1, index: 40, ipc args: [bytes4], ipc returns: [boolean]
    // WARNING: Arguments are unknown!
    public unknown_ret PS4SettingsChanged();  // argc: 1, index: 41, ipc args: [bytes1], ipc returns: []
    // WARNING: Arguments are unknown!
    public unknown_ret SwitchSettingsChanged();  // argc: 1, index: 42, ipc args: [bytes1], ipc returns: []
    // WARNING: Arguments are unknown!
    public unknown_ret ControllerSettingsChanged();  // argc: 1, index: 43, ipc args: [bytes4], ipc returns: []
    // WARNING: Arguments are unknown!
    public unknown_ret SetTrackpadPressureCurve();  // argc: 3, index: 44, ipc args: [bytes4, bytes4, bytes4], ipc returns: []
    // WARNING: Arguments are unknown!
    public unknown_ret SetDefaultNintendoButtonLayout();  // argc: 1, index: 45, ipc args: [bytes1], ipc returns: []
    // WARNING: Arguments are unknown!
    public unknown_ret IsControllerConnected();  // argc: 2, index: 46, ipc args: [bytes4, bytes1], ipc returns: [boolean]
    // WARNING: Arguments are unknown!
    public unknown_ret TriggerHapticPulse();  // argc: 6, index: 47, ipc args: [bytes4, bytes4, bytes2, bytes2, bytes2, bytes4], ipc returns: []
    // WARNING: Arguments are unknown!
    public unknown_ret TriggerSimpleHapticEvent();  // argc: 5, index: 48, ipc args: [bytes4, bytes1, bytes1, bytes1, bytes1], ipc returns: []
    // WARNING: Arguments are unknown!
    public unknown_ret TriggerVibration();  // argc: 4, index: 49, ipc args: [bytes4, bytes4, bytes2, bytes2], ipc returns: []
    // WARNING: Arguments are unknown!
    public unknown_ret TriggerVibrationExtended();  // argc: 6, index: 50, ipc args: [bytes4, bytes4, bytes2, bytes2, bytes2, bytes2], ipc returns: []
    // WARNING: Arguments are unknown!
    public unknown_ret SetLEDColor();  // argc: 5, index: 51, ipc args: [bytes4, bytes1, bytes1, bytes1, bytes4], ipc returns: []
    // WARNING: Arguments are unknown!
    public unknown_ret SetDonglePairingMode();  // argc: 2, index: 52, ipc args: [bytes1, bytes4], ipc returns: []
    public unknown_ret ReserveSteamController();  // argc: 0, index: 53, ipc args: [], ipc returns: []
    public unknown_ret CancelSteamControllerReservations();  // argc: 0, index: 0, ipc args: [], ipc returns: []
    // WARNING: Arguments are unknown!
    public unknown_ret OpenStreamingSession();  // argc: 2, index: 0, ipc args: [bytes4, bytes4], ipc returns: []
    // WARNING: Arguments are unknown!
    public unknown_ret CloseStreamingSession();  // argc: 2, index: 1, ipc args: [bytes4, bytes4], ipc returns: []
    // WARNING: Arguments are unknown!
    public unknown_ret UpdateStreamingSessionInputPermissions();  // argc: 1, index: 2, ipc args: [bytes4], ipc returns: []
    // WARNING: Arguments are unknown!
    public unknown_ret InitiateISPFirmwareUpdate();  // argc: 1, index: 3, ipc args: [bytes4], ipc returns: [bytes8]
    // WARNING: Arguments are unknown!
    public unknown_ret InitiateBootloaderFirmwareUpdate();  // argc: 1, index: 4, ipc args: [bytes4], ipc returns: [bytes8]
    // WARNING: Arguments are unknown!
    public unknown_ret FlashControllerFirmware();  // argc: 4, index: 5, ipc args: [bytes4, unknown, bytes4, string], ipc returns: [bytes8]
    // WARNING: Arguments are unknown!
    public unknown_ret TurnOffController();  // argc: 1, index: 6, ipc args: [bytes4], ipc returns: []
    public unknown_ret EnumerateControllers();  // argc: 0, index: 7, ipc args: [], ipc returns: []
    // WARNING: Arguments are unknown!
    public unknown_ret GetControllerStatusEvent();  // argc: 2, index: 0, ipc args: [bytes4], ipc returns: [bytes1, bytes12]
    // WARNING: Arguments are unknown!
    public unknown_ret GetActualControllerDetails();  // argc: 2, index: 1, ipc args: [bytes4], ipc returns: [bytes1, bytes92]
    // WARNING: Arguments are unknown!
    [BlacklistedInCrossProcessIPC]
    public unknown_ret GetControllerIdentity();  // argc: 2, index: 2, ipc args: [bytes4, bytes4], ipc returns: [bytes1]
    // WARNING: Arguments are unknown!
    [BlacklistedInCrossProcessIPC]
    public unknown_ret GetControllerPersonalization();  // argc: 2, index: 3, ipc args: [bytes4, bytes4], ipc returns: [bytes1]
    // WARNING: Arguments are unknown!
    public unknown_ret GetControllerReverseDiamondLayout();  // argc: 1, index: 4, ipc args: [bytes4], ipc returns: [bytes1]
    // WARNING: Arguments are unknown!
    public unknown_ret BRumbleEnabledByUser();  // argc: 1, index: 5, ipc args: [bytes4], ipc returns: [boolean]
    // WARNING: Arguments are unknown!
    public unknown_ret BHapticsEnabledByUser();  // argc: 1, index: 6, ipc args: [bytes4], ipc returns: [boolean]
    // WARNING: Arguments are unknown!
    public unknown_ret SetControllerPairingConnectionState();  // argc: 2, index: 7, ipc args: [bytes4, bytes1], ipc returns: []
    // WARNING: Arguments are unknown!
    public unknown_ret SetControllerKeyboardMouseState();  // argc: 2, index: 8, ipc args: [bytes4, bytes1], ipc returns: []
    // WARNING: Arguments are unknown!
    [BlacklistedInCrossProcessIPC]
    public unknown_ret GetTouchKeysForPopupMenu();  // argc: 4, index: 9, ipc args: [bytes4, bytes4, bytes4, bytes4], ipc returns: [bytes4]
    // WARNING: Arguments are unknown!
    public unknown_ret PopupMenuTouchKeyClicked();  // argc: 3, index: 10, ipc args: [bytes4, bytes4, bytes4], ipc returns: []
    // WARNING: Arguments are unknown!
    public unknown_ret AccessControllerInputGeneratorMouseButton();  // argc: 3, index: 11, ipc args: [bytes4, bytes4, bytes1], ipc returns: []
    // WARNING: Arguments are unknown!
    [BlacklistedInCrossProcessIPC]
    public unknown_ret SetControllerSetting();  // argc: 2, index: 12, ipc args: [bytes4, bytes4], ipc returns: []
    [BlacklistedInCrossProcessIPC]
    public unknown_ret GetEmulatedOutputState();  // argc: 0, index: 13, ipc args: [], ipc returns: [bytes4]
    // WARNING: Arguments are unknown!
    [BlacklistedInCrossProcessIPC]
    public unknown_ret SetSelectedConfigForApp();  // argc: 7, index: 0, ipc args: [bytes4, bytes4, bytes8, bytes4, bytes1, bytes1], ipc returns: []
    // WARNING: Arguments are unknown!
    [BlacklistedInCrossProcessIPC]
    public unknown_ret BControllerHasUniqueConfigForAppID();  // argc: 2, index: 1, ipc args: [bytes4, bytes4], ipc returns: [boolean]
    // WARNING: Arguments are unknown!
    public unknown_ret DeRegisterController();  // argc: 2, index: 2, ipc args: [bytes4, bytes4], ipc returns: []
    // WARNING: Arguments are unknown!
    public unknown_ret SendOSKeyboardEvent();  // argc: 1, index: 3, ipc args: [string], ipc returns: []
    // WARNING: Arguments are unknown!
    public unknown_ret SetOSKeyboardKey();  // argc: 2, index: 4, ipc args: [bytes4, bytes1], ipc returns: []
    // WARNING: Arguments are unknown!
    public unknown_ret SetMousePosition();  // argc: 3, index: 5, ipc args: [bytes4, bytes4, bytes4], ipc returns: []
    public unknown_ret GetGamepadIndexChangeCounter();  // argc: 0, index: 6, ipc args: [], ipc returns: [bytes4]
    // WARNING: Arguments are unknown!
    public unknown_ret BSwapGamepadIndex();  // argc: 3, index: 0, ipc args: [bytes4, bytes4, bytes4], ipc returns: [boolean]
    // WARNING: Arguments are unknown!
    public unknown_ret GetGamepadIndexForXInputIndex();  // argc: 1, index: 1, ipc args: [bytes4], ipc returns: [bytes4]
    // WARNING: Arguments are unknown!
    public unknown_ret GetControllerIndexForGamepadIndex();  // argc: 1, index: 2, ipc args: [bytes4], ipc returns: [bytes4]
    // WARNING: Arguments are unknown!
    public unknown_ret SetControllerActiveAccount();  // argc: 2, index: 3, ipc args: [bytes4, bytes4], ipc returns: []
    // WARNING: Arguments are unknown!
    public unknown_ret StartControllerRegistrationToAccount();  // argc: 2, index: 4, ipc args: [bytes4, bytes4], ipc returns: []
    // WARNING: Arguments are unknown!
    public unknown_ret CompleteControllerRegistrationToAccount();  // argc: 2, index: 5, ipc args: [bytes4, bytes4], ipc returns: []
    // WARNING: Arguments are unknown!
    public unknown_ret AutoRegisterControllerRegistrationToAccount();  // argc: 2, index: 6, ipc args: [bytes4, bytes4], ipc returns: []
    // WARNING: Arguments are unknown!
    [BlacklistedInCrossProcessIPC]
    public unknown_ret GetConfigForAppAndController();  // argc: 4, index: 7, ipc args: [bytes4, string, bytes4, bytes4], ipc returns: [bytes1]
    // WARNING: Arguments are unknown!
    [BlacklistedInCrossProcessIPC]
    public unknown_ret SetControllerPersonalization();  // argc: 3, index: 8, ipc args: [bytes4, bytes4, bytes1], ipc returns: []
    // WARNING: Arguments are unknown!
    public unknown_ret SetPersonalizationFile();  // argc: 4, index: 9, ipc args: [bytes4, bytes4, bytes8], ipc returns: []
    // WARNING: Arguments are unknown!
    public unknown_ret SetGameWindowPos();  // argc: 4, index: 10, ipc args: [bytes4, bytes4, bytes4, bytes4], ipc returns: []
    // WARNING: Arguments are unknown!
    [BlacklistedInCrossProcessIPC]
    public unknown_ret GetGameWindowPos();  // argc: 4, index: 11, ipc args: [bytes4, bytes4, bytes4, bytes4], ipc returns: []
    // WARNING: Arguments are unknown!
    public unknown_ret HasGameMapping();  // argc: 1, index: 12, ipc args: [bytes4], ipc returns: [bytes1]
    // WARNING: Arguments are unknown!
    public unknown_ret GetControllerUsageData();  // argc: 2, index: 13, ipc args: [bytes4], ipc returns: []
    // WARNING: Arguments are unknown!
    public unknown_ret BAllowAppConfigForController();  // argc: 2, index: 14, ipc args: [bytes4, bytes4], ipc returns: [boolean]
    public unknown_ret ResetControllerEnableCache();  // argc: 0, index: 15, ipc args: [], ipc returns: []
    // WARNING: Arguments are unknown!
    public unknown_ret GetControllerEnableSupport();  // argc: 1, index: 0, ipc args: [bytes4], ipc returns: [bytes4]
    public unknown_ret BInputGenerated();  // argc: 0, index: 1, ipc args: [], ipc returns: [boolean]
    // WARNING: Arguments are unknown!
    public unknown_ret GetControllerActivityByType();  // argc: 1, index: 0, ipc args: [bytes4], ipc returns: [bytes4]
    public unknown_ret GetLastActiveControllerVID();  // argc: 0, index: 1, ipc args: [], ipc returns: [bytes2]
    public unknown_ret GetLastActiveControllerPID();  // argc: 0, index: 0, ipc args: [], ipc returns: [bytes2]
    // WARNING: Arguments are unknown!
    public unknown_ret LoadControllerPersonalizationFile();  // argc: 4, index: 0, ipc args: [bytes4, string, bytes1, bytes1], ipc returns: []
    // WARNING: Arguments are unknown!
    [BlacklistedInCrossProcessIPC]
    public unknown_ret SaveControllerPersonalizationFile();  // argc: 3, index: 1, ipc args: [bytes4, bytes4, bytes4], ipc returns: [bytes1]
    // WARNING: Arguments are unknown!
    public unknown_ret LoadRemotePlayControllerPersonalizationVDF();  // argc: 2, index: 2, ipc args: [string, string], ipc returns: []
    // WARNING: Arguments are unknown!
    public unknown_ret FindControllerByPath();  // argc: 1, index: 3, ipc args: [string], ipc returns: [bytes4]
    // WARNING: Arguments are unknown!
    [BlacklistedInCrossProcessIPC]
    public unknown_ret GetControllerPath();  // argc: 2, index: 4, ipc args: [bytes4, bytes4], ipc returns: [bytes1]
    // WARNING: Arguments are unknown!
    [BlacklistedInCrossProcessIPC]
    public unknown_ret GetControllerProductName();  // argc: 2, index: 5, ipc args: [bytes4, bytes4], ipc returns: [bytes1]
    // WARNING: Arguments are unknown!
    public unknown_ret SetControllerHapticsSetting();  // argc: 2, index: 6, ipc args: [bytes4, bytes1], ipc returns: [bytes1]
    // WARNING: Arguments are unknown!
    public unknown_ret SetControllerName();  // argc: 2, index: 7, ipc args: [bytes4, string], ipc returns: [bytes1]
    // WARNING: Arguments are unknown!
    public unknown_ret SetControllerRumbleSetting();  // argc: 2, index: 8, ipc args: [bytes4, bytes4], ipc returns: [bytes1]
    // WARNING: Arguments are unknown!
    public unknown_ret SetControllerNintendoLayoutSetting();  // argc: 2, index: 9, ipc args: [bytes4, bytes1], ipc returns: [bytes1]
    // WARNING: Arguments are unknown!
    public bool BGetTouchConfigData(uint unk, uint unk1, out UInt64 unk2, out uint unk3, [IPCOut] CUtlBuffer* unk4, [IPCOut] CUtlBuffer* unk5);  // argc: 6, index: 10, ipc args: [bytes4, bytes4], ipc returns: [boolean, bytes8, bytes4, unknown, unknown]
    // WARNING: Arguments are unknown!
    [BlacklistedInCrossProcessIPC]
    public unknown_ret BSaveTouchConfigLayout();  // argc: 3, index: 11, ipc args: [bytes4, bytes4, bytes4], ipc returns: [boolean]
    // WARNING: Arguments are unknown!
    public unknown_ret SetGyroOn();  // argc: 3, index: 12, ipc args: [bytes4, bytes8], ipc returns: []
    // WARNING: Arguments are unknown!
    public unknown_ret CursorVisibilityChanged();  // argc: 1, index: 13, ipc args: [bytes1], ipc returns: []
    // WARNING: Arguments are unknown!
    public unknown_ret ForceSimpleHapticEvent();  // argc: 5, index: 14, ipc args: [bytes4, bytes1, bytes1, bytes1, bytes1], ipc returns: []
}