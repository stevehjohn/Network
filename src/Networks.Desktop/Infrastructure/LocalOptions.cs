// ReSharper disable UnusedAutoPropertyAccessor.Global

using CommandLine;
using JetBrains.Annotations;

namespace Networks.Desktop.Infrastructure;

[UsedImplicitly]
[Verb("local", HelpText = "Run a puzzle from the local file system.")]
public class LocalOptions
{
    [Option('n', "number", Required = true, HelpText = "The puzzle number to run.")]
    public int PuzzleNumber { get; set; }
}