using CommandLine;

namespace Nightfall.UpdatePusher;

public sealed class Options
{
    [Option('s', "source", Required = true, HelpText = "The source directory where the new build files are located.")]
    public string Source { get; set; } = string.Empty;

    [Option('d', "destination", Required = true, HelpText = "The destination directory where the files should be pushed to.")]
    public string Destination { get; set; } = string.Empty;
    
    [Option("build-version", Required = true, HelpText = "A version associated with this build. Should follow semantic versioning standard.")]
    public string BuildVersion { get; set; } = string.Empty;

    [Option('v', "verbose", Required = false, HelpText = "Outputs stacktrace of an error in addition to the message.")]
    public bool IsVerbose { get; set; } = false;

    [Option('q', "quiet", Required = false, Default = false, HelpText = "Disables all handled errors output. Note: Unhandled errors can still appear.")]
    public bool Quiet { get; set; } = false;
}