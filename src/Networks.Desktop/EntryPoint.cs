using CommandLine;
using Networks.Desktop.Infrastructure;
using Networks.Desktop.Runners;

namespace Networks.Desktop;

public static class EntryPoint
{
    public static void Main(string[] arguments)
    {
        var parser = new Parser(settings =>
        {
            settings.CaseInsensitiveEnumValues = true;
        });
        
        parser.ParseArguments<LocalOptions, RemoteOptions>(arguments)
            .WithParsed<LocalOptions>(options => new Local().Run(options.PuzzleNumber))
            .WithParsed<RemoteOptions>(options => new Remote().Run(options));
    }
}