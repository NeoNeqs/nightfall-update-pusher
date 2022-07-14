using System.Net.Http.Headers;
using System.Text;

namespace Nightfall.UpdatePusher;

/// <summary>
///     Handles operations and parses .info files.
/// </summary>
public sealed class InfoFile : IDisposable
{
    private readonly string _filePath;
    private StreamWriter? _writer;
    
    // Exists - Open for read
    // Not Exist - CreateNew
    public InfoFile(string filePath)
    {
        _filePath = filePath;
    }

    /*public bool OpenWrite()
    {

    }*/

    public bool CreateNew()
    {
        FileStreamOptions fsOptions = new()
        {
            Access = FileAccess.Write, Mode = FileMode.CreateNew
        };

        try
        {
            _writer = new StreamWriter(_filePath, Encoding.UTF8, fsOptions);
        }
        catch (Exception e)
        {
            Utils.LogError(e);
            return false;
        }

        return true;
    }


    public bool WritePair(string key, string value)
    {
        if (_writer is null) return false;
        
        _writer.WriteLine($"{key} {value}");

        return true;
    }

    public void Dispose()
    {
        _writer?.Dispose();
    }
}