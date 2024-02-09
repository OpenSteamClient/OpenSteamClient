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
    }

    public async Task RunStartup()
    {
        HadShadersEnabled = shader.BIsShaderManagementEnabled();
        shader.EnableShaderManagement(false);
        shader.EnableShaderBackgroundProcessing(false);
        shader.EnableShaderManagementSystem(false);

        // Steam on Windows does not persist shader settings for some reason, so don't bother restarting as we'll get stuck in an infinite loop
        if (HadShadersEnabled && OperatingSystem.IsLinux()) {
            Console.WriteLine("Forcing restart, had shaders enabled");
            await bootstrapper.Restart();
        }
    }
}