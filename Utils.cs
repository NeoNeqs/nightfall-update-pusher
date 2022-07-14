namespace Nightfall.UpdatePusher;

public static class Utils
{
    private static bool _isVerbose;
    private static bool _isQuiet;

    public static void LogError(Exception exception)
    {
        if (!_isQuiet)
        {
            Console.Error.WriteLine(_isVerbose ? exception : exception.Message);
        }
    }

    public static void Configure(bool verbose, bool quiet)
    {
        _isVerbose = verbose;
        _isQuiet = quiet;
    }
}