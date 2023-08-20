namespace OpenSteamworks.Client.Extensions;

public static class DirectoryInfoExtensions
{
    public static void CopyFilesRecursively(this DirectoryInfo source, DirectoryInfo target, bool allowOverwrite) {
        foreach (DirectoryInfo dir in source.GetDirectories())
            CopyFilesRecursively(dir, target.CreateSubdirectory(dir.Name), allowOverwrite);
        foreach (FileInfo file in source.GetFiles())
            file.CopyTo(Path.Combine(target.FullName, file.Name), allowOverwrite);
    }
    public static IEnumerable<FileInfo> EnumerateFilesRecursively(this DirectoryInfo di, int maxDepth = 10) {
        return di.EnumerateFiles("*", new EnumerationOptions {
                RecurseSubdirectories = true,
                MaxRecursionDepth = maxDepth,
        });
    }
}
