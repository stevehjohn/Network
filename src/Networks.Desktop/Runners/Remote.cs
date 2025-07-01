using System;
using System.Net;
using System.Threading;
using Networks.Desktop.Infrastructure;
using Networks.Desktop.Presentation;
using Networks.Engine.Board;
using Networks.Engine.Infrastructure;

namespace Networks.Desktop.Runners;

public class Remote
{
    public void Run(RemoteOptions options)
    {
        using var renderer = new PuzzleRenderer();

        var client = new PuzzleClient();

        for (var i = 0; i < options.Quantity; i++)
        {
            (DateOnly Date, Grid Grid, int Variant)? puzzle = null;

            for (var retry = 1; retry < 21; retry++)
            {
                try
                {
                    puzzle = client.GetNextPuzzle(options.Difficulty);
                }
                catch
                {
                    //
                }

                if (puzzle != null)
                {
                    break;
                }

                var sleep = (int) Math.Pow(retry, 2);

                for (var timer = 0; timer < sleep; timer++)
                {
                    Thread.Sleep(1_000);
                }
            }

            if (puzzle == null)
            {
                break;
            }

            renderer.Grid = puzzle.Value.Grid;

            renderer.SkipFrames = 1;

            renderer.Run();
            
            for (var retry = 1; retry < 21; retry++)
            {
                var statusCode = client.SendResult(puzzle.Value.Date, puzzle.Value.Grid, puzzle.Value.Variant);
            
                if (statusCode != HttpStatusCode.OK)
                {
                    var sleep = (int) Math.Pow(retry, 2);
            
                    for (var timer = 0; timer < sleep; timer++)
                    {
                        Thread.Sleep(1_000);
                    }
                }
                else
                {
                    break;
                }
            }
        }
    }
}