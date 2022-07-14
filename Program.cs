using System.Security.Cryptography;
using CommandLine;

namespace Nightfall.UpdatePusher;

public static class Program
{
    public static void Main(string[] args)
    {
        for (var i = 0; i < args.Length; i++)
        {
            // Command line arguments are terribly handled by .NET and so they require a little bit of trimming
            args[i] = args[i].Trim('"').Trim();
        }

        Parser.Default.ParseArguments<Options>(args).WithParsed(Run);
    }

    private static void Run(Options options)
    {
        Utils.Configure(options.IsVerbose, options.Quiet);
        using var hashAlgorithm = SHA256.Create();
        
        //TODO: parse main .info file
        foreach (string filePath in GetFiles(options.Source))
        {
            string fileName = Path.GetFileName(filePath);
            string fileVersionsDirectory = Path.Combine(options.Destination, fileName);

            if (IsFileNew(fileVersionsDirectory))
            {
                // File was never part of any build. It will the first version of it.
                bool success = CreateVersioningDirectory(fileVersionsDirectory);
                if (!success) return;

                using var infoFile = new InfoFile(Path.Combine(fileVersionsDirectory, ".info"));
                success = infoFile.CreateNew();
                if (!success) return;

                string? hash = hashAlgorithm.ComputeHashFileS(filePath);
                if (hash is null) return;
                infoFile.WritePair("latest", hash);
                
                // The copied file is renamed to latest to indicate it's the latest file.
                File.Copy(filePath, Path.Combine(fileVersionsDirectory, "latest"));
                continue;
            }

            // Seems like the file has appeared before in some build(s)
            
        }
    }

    private static bool CreateVersioningDirectory(string path)
    {
        DirectoryInfo newFileDirectory;

        try
        {
            newFileDirectory = Directory.CreateDirectory(path);
        }
        catch (Exception e)
        {
            Utils.LogError(e);
            return false;
        }

        try
        {
            newFileDirectory.CreateSubdirectory("patches");
        }
        catch (Exception e)
        {
            Utils.LogError(e);
            return false;
        }

        return true;
    }

    private static bool IsFileNew(string path)
    {
        return !Directory.Exists(path);
    }

    private static string[] GetFiles(string path)
    {
        string[]? files = Array.Empty<string>();

        try
        {
            files = Directory.GetFiles(path);
        }
        catch (Exception e)
        {
            Utils.LogError(e);
        }

        return files;
    }
}