var rootDirectory = args[0];

Console.Title = "ClearBinObjFolders.exe";

if (!Directory.Exists(rootDirectory))
{
    Console.WriteLine($"{rootDirectory} does not exist");
    Environment.Exit(1);
}

var directories = GetDirectoriesRecursive(rootDirectory);

var directoriesToRemove = GetDirectoriesToRemove(directories);

foreach (var directory in directoriesToRemove)
{
    try
    {
        Directory.Delete(directory, true);
        Console.WriteLine($"{directory} removed");
    }
    catch (Exception)
    {
        Console.WriteLine($"Error in removing {directory}");
    }
}

IEnumerable<string> GetDirectoriesRecursive(string directory)
{
    var directories = Directory.GetDirectories(directory);
    var subDirectories = directories
        .Where(dir => !dir.Contains("node_modules"))
        .SelectMany(dir => GetDirectoriesRecursive(dir));
    return [.. directories, .. subDirectories];
}

IEnumerable<string> GetDirectoriesToRemove(IEnumerable<string> directories)
{
    return directories.Where(dir =>
    {
        var leaf = Path.GetFileName(dir);
        return leaf != null &&
            (leaf.Equals("obj", StringComparison.InvariantCultureIgnoreCase)
            ||
            leaf.Equals("bin", StringComparison.InvariantCultureIgnoreCase));
    });
}