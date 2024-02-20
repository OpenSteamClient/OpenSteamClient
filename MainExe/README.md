# MainExe
Custom .NET host that exports native symbols for `steamclient.so` to use.

## TODO
Investigate making this a custom nethost so that managed DLLs can just set a custom apphost path and have themselves exported as a regular singlefile/host executable or library.

## Why not NativeAOT
NativeAOT does not support:
- cross compile
- codegen via Reflection.Emit