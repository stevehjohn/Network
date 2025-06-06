// ReSharper disable AutoPropertyCanBeMadeGetOnly.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global

using JetBrains.Annotations;
using Networks.Engine.Board;

namespace Networks.Engine.Models;

[UsedImplicitly]
public class Data
{
    public Piece[] GridLayout { get; set; } = [];

    public int PowerCell { get; set; }
}