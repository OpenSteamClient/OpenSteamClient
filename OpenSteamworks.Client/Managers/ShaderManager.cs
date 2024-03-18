using OpenSteamworks.Client.Startup;
using OpenSteamworks.Client.Utils.DI;
using OpenSteamworks.Generated;

namespace OpenSteamworks.Client.Managers;

public class ShaderManager : IClientLifetime
{
    private readonly IClientShader shader;
    private readonly Bootstrapper bootstrapper;

    public bool HadShadersEnabled { get; private set; }

    public ShaderManager(Bootstrapper bootstrapper, IClientShader shader) {
        this.bootstrapper = bootstrapper;
        this.shader = shader;
    }

    public async Task RunShutdown()
    {
        shader.EnableShaderManagement(false);
        shader.EnableShaderBackgroundProcessing(false);
        shader.EnableShaderManagementSystem(false);
        await Task.CompletedTask;
    }

    public async Task RunStartup()
    {
        HadShadersEnabled = shader.BIsShaderManagementEnabled();
        shader.EnableShaderManagement(false);
        shader.EnableShaderBackgroundProcessing(false);
        shader.EnableShaderManagementSystem(false);

        // Steam no longer persists shader management settings, so the disablement is only for this session. 
        // Valve, wtf?
    }
}