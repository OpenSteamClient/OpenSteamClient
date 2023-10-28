namespace CustomBuildTask;

public static class FileEx {
    /// <summary>
    /// Checks if a file exists on PATH or the current directory.
    /// </summary>
    /// <param name="fileName"></param>
    /// <returns></returns>
    public static bool Exists(string fileName)
    {
        return GetFullPath(fileName) != null;
    }
    
    /// <summary>
    /// Gets an executable given it's name from the PATH. Returns null if not found.
    /// </summary>
    /// <param name="fileName"></param>
    /// <returns></returns>
    public static string? GetFullPath(string fileName)
    {
        if (File.Exists(fileName))
            return Path.GetFullPath(fileName);

        var pathEnv = Environment.GetEnvironmentVariable("PATH");
        if (string.IsNullOrEmpty(pathEnv)) {
            return null;
        }
        
        foreach (var path in pathEnv.Split(Path.PathSeparator))
        {
            var fullPath = Path.Combine(path, fileName);
            Console.WriteLine("trying to find at " + fullPath);
            if (File.Exists(fullPath)) {
                Console.WriteLine("found at " + fullPath);
                return fullPath;
            }

            // Also support .exe files
            var fullPathExe = Path.Combine(path, fileName + ".exe");
            
            if (File.Exists(fullPathExe)) {
                Console.WriteLine("found at " + fullPathExe);
                return fullPathExe;
            }
        }
        return null;
    }
}