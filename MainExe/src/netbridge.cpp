#include <netbridge.h>

const auto libname = STR("managed");
typedef int (CORECLR_DELEGATE_CALLTYPE *pMain_fn)();

int CNetBridge::Run() {
    // Get the current executable's directory
    // This sample assumes the managed assembly to load and its runtime configuration file are next to the host
    char_t host_path[MAX_PATH];
#if _WIN32
    auto size = ::GetFullPathNameW(libname, sizeof(host_path) / sizeof(char_t), host_path, nullptr);
    assert(size != 0);
#else
    auto resolved = realpath(libname, host_path);
    assert(resolved != nullptr);
#endif

    string_t root_path = host_path;
    auto pos = root_path.find_last_of(DIR_SEPARATOR);
    assert(pos != string_t::npos);
    root_path = root_path.substr(0, pos + 1);

    return this->run_component(root_path);
}

int CNetBridge::run_component(const string_t& root_path)
{
    //
    // STEP 1: Load HostFxr and get exported hosting functions
    //
    if (!load_hostfxr(nullptr))
    {
        assert(false && "Failure: load_hostfxr()");
        return EXIT_FAILURE;
    }

    std::cout << "Loaded hostfxr" << std::endl;

    //
    // STEP 2: Initialize and start the .NET Core runtime
    //
    const string_t config_path = root_path + STR("managed.runtimeconfig.json");
    this->managed_path = root_path + STR("managed.dll");
    this->load_assembly_and_get_function_pointer = get_dotnet_load_assembly(config_path.c_str());
    assert(this->load_assembly_and_get_function_pointer != nullptr && "Failure: get_dotnet_load_assembly()");

    std::cout << "Started .NET Core runtime" << std::endl;

    //
    // STEP 3: Load managed assembly and get function pointer to a managed method
    //
    const char_t *dotnet_type = STR("managed.Entry, managed");
    pMain_fn managedMain = (pMain_fn)this->GetFunction(STR("managed.Entry, managed"), STR("Main"));
    if (managedMain == nullptr)
    {
        std::cout << "Failure: managedMain == nullptr" << std::endl;
        return EXIT_FAILURE;
    }

    std::cout << "Got main method, running" << std::endl;
    return managedMain();
}

CNetBridge::CNetBridge(int argc, char_t **argv)
{
    assert(netbridge == nullptr);
    netbridge = this;
    this->argc = argc;
    this->argv = argv;
}

CNetBridge::~CNetBridge()
{
    if (netbridge == this) {
        netbridge = nullptr;
    }
}

// <SnippetLoadHostFxr>
// Using the nethost library, discover the location of hostfxr and get exports
bool CNetBridge::load_hostfxr(const char_t *assembly_path)
{
    get_hostfxr_parameters params { sizeof(get_hostfxr_parameters), assembly_path, nullptr };
    // Pre-allocate a large buffer for the path to hostfxr
    char_t buffer[MAX_PATH];
    size_t buffer_size = sizeof(buffer) / sizeof(char_t);
    int rc = get_hostfxr_path(buffer, &buffer_size, &params);
    if (rc != 0)
        return false;

    // Load hostfxr and get desired exports
    void *lib = load_library(buffer);
    init_for_cmd_line_fptr = (hostfxr_initialize_for_dotnet_command_line_fn)get_export(lib, "hostfxr_initialize_for_dotnet_command_line");
    init_for_config_fptr = (hostfxr_initialize_for_runtime_config_fn)get_export(lib, "hostfxr_initialize_for_runtime_config");
    get_delegate_fptr = (hostfxr_get_runtime_delegate_fn)get_export(lib, "hostfxr_get_runtime_delegate");
    run_app_fptr = (hostfxr_run_app_fn)get_export(lib, "hostfxr_run_app");
    close_fptr = (hostfxr_close_fn)get_export(lib, "hostfxr_close");

    return (init_for_config_fptr && get_delegate_fptr && close_fptr);
}

// Load and initialize .NET Core and get desired function pointer for scenario
load_assembly_and_get_function_pointer_fn CNetBridge::get_dotnet_load_assembly(const char_t *config_path)
{
    // Load .NET Core
    void *load_assembly_and_get_function_pointer = nullptr;
    hostfxr_handle cxt = nullptr;
    int rc = init_for_config_fptr(config_path, nullptr, &cxt);
    if (rc != 0 || cxt == nullptr)
    {
        std::cerr << "Init failed: " << std::hex << std::showbase << rc << std::endl;
        close_fptr(cxt);
        return nullptr;
    }

    // Get the load assembly function pointer
    rc = get_delegate_fptr(
        cxt,
        hdt_load_assembly_and_get_function_pointer,
        &load_assembly_and_get_function_pointer);
    if (rc != 0 || load_assembly_and_get_function_pointer == nullptr)
        std::cerr << "Get delegate failed: " << std::hex << std::showbase << rc << std::endl;

    close_fptr(cxt);
    return (load_assembly_and_get_function_pointer_fn)load_assembly_and_get_function_pointer;
}

void *CNetBridge::GetFunction(const string_t &className, const string_t &funcName, char_t *delegate_type_name) {
    assert(this->load_assembly_and_get_function_pointer != nullptr && "Failure: get_dotnet_load_assembly()");
    void *func = nullptr;

    int rc = this->load_assembly_and_get_function_pointer(
        this->managed_path.c_str(),
        className.c_str(),
        funcName.c_str(),
        delegate_type_name,
        nullptr,
        (void**)&func);

    assert(rc == 0 && func != nullptr && "Failure: load_assembly_and_get_function_pointer()");
    return func;
}