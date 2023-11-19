namespace OpenSteamworks.Client.Utils;

public class TemporaryEnvVars : IDisposable
{
    private Dictionary<string, string?> ChangedVars = new();
    public void Dispose()
    {
        foreach (var item in ChangedVars)
        {
            UtilityFunctions.SetEnvironmentVariable(item.Key, item.Value);
        }
    }

    public void SetEnvironmentVariable(string variable, string value) {
        string? prevValue = UtilityFunctions.GetEnvironmentVariable(variable);
        ChangedVars[variable] = prevValue;
        UtilityFunctions.SetEnvironmentVariable(variable, value);
    }
}