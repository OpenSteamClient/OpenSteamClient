namespace Startup
{
  void InitQT();
  void SetLaunchArgs(int argc, char *argv[]);
  void InitGlobals();
  void MaxOpenDescriptorsCheck();
  void SetUpdaterFilesDir();
  void SingleInstanceChecks();
  void RunBootstrapper();
  int UIMain();
  void InitApplication();
} // namespace Startup
