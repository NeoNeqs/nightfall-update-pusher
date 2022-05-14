using CommandLine;

namespace Nightfall.UpdatePusher;

public sealed class Options
{
    [Option('p', "path", Required = true, HelpText = "Sets the path to push updates on/from.")]
    public string Path { get; set; } = null!;

    [Option('v', "verbose", Required = false, HelpText = "Sets whether to print the stacktrace of an exception.")]
    public bool Verbose { get; set; } = false;

}