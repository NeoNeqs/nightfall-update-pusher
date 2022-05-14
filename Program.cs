using CommandLine;
using System.Security.Cryptography;
using System.Text;

namespace Nightfall.UpdatePusher;

public static class Program
{
    public static void Main(string[] args)
    {
        for (var i = 0; i < args.Length; i++)
        {
            // Command line arguments are terribly handled by .NET and so they require a bit of trimming :)
            args[i] = args[i].Trim('"').Trim();
        }

        Parser.Default.ParseArguments<Options>(args).WithParsed(Run);
    }

    private static void Run(Options options)
    {
        // Prepare directories 
        DirectoryInfo root;
        try
        {
            root = Directory.CreateDirectory(Path.Combine(options.Path, "temp"));
        }
        catch (Exception e)
        {
            LogError(e, options.Verbose);
            return; 
        }

        DirectoryInfo filesDirectory;
        try
        {
            filesDirectory = root.CreateSubdirectory("files");
        }
        catch (Exception e)
        {
            LogError(e, options.Verbose);
            return;
        }

        IEnumerable<string> files;
        try
        {
            files = Directory.EnumerateFiles(Path.Combine(options.Path, ".new"));
        }
        catch (Exception e)
        {
            LogError(e, options.Verbose);
            return;
        }
        
        using var algorithm = SHA256.Create();
        Dictionary<string, string> updateInfo = new();
        
        foreach (string filePath in files)
        {
            // Calculate hash of each file. This is needed to calculate the update hash and generate the .info file later
            byte[] fileContents;

            try
            {
                fileContents = File.ReadAllBytes(filePath);
            }
            catch (Exception e)
            {
                LogError(e, options.Verbose);
                return;
            }
            updateInfo[filePath] = algorithm.ComputeHashS(fileContents);
            
            try
            {
                File.Copy(filePath, Path.Combine(filesDirectory.FullName, Path.GetFileName(filePath)), true);
            }
            catch (Exception e)
            {
                LogError(e, options.Verbose);
                return;
            }
        }

        using (StreamWriter infoFile = CreateInfoFile(root.FullName, Encoding.UTF8))
        {
            foreach ((string filePath, string fileHash) in updateInfo)
            {
                infoFile.WriteLine($"{Path.GetFileName(filePath)} {fileHash}");
            }
        }

        string combinedHashes = updateInfo.Values.Aggregate(string.Empty, (current, hash) => current + hash);
        byte[] combinedHashesBytes = Encoding.ASCII.GetBytes(combinedHashes);
        string updateHash = algorithm.ComputeHashS(combinedHashesBytes);

        string realUpdatePath = Path.Combine(options.Path, updateHash);

        try
        {
            Directory.Move(root.FullName, realUpdatePath);
        }
        catch (Exception e)
        {
            LogError(e, options.Verbose);
            return;
        }

        using StreamWriter rootInfoFile = CreateInfoFile(options.Path, Encoding.ASCII);
        rootInfoFile.WriteLine($"latest {updateHash}");
    }

    private static StreamWriter CreateInfoFile(string path, Encoding encoding)
    {
        FileStreamOptions fsOptions = new()
        {
            Access = FileAccess.Write, Mode = FileMode.Create
        };
        return new StreamWriter(Path.Combine(path, ".info"), encoding, fsOptions);
    }

    private static void LogError(Exception exception, bool verbose)
    {
        Console.Error.WriteLine(verbose ? exception : exception.Message);
    }
}