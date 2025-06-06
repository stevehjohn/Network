namespace Networks.Engine.Board;

public static class Connector
{
    public static readonly Dictionary<(Piece, Rotation), List<Direction>> Connections = new()
    {
        { (Piece.Terminator, Rotation.Zero), [new Direction(-1, 0)] },
        { (Piece.Terminator, Rotation.Ninety), [new Direction(0, -1)] },
        { (Piece.Terminator, Rotation.OneEighty), [new Direction(1, 0)] },
        { (Piece.Terminator, Rotation.TwoSeventy), [new Direction(0, 1)] },
        { (Piece.Straight, Rotation.Zero), [new Direction(0, -1), new Direction(0, 1)] },
        { (Piece.Straight, Rotation.Ninety), [new Direction(-1, 0), new Direction(1, 0)] },
        { (Piece.Straight, Rotation.OneEighty), [new Direction(0, -1), new Direction(0, 1)] },
        { (Piece.Straight, Rotation.TwoSeventy), [new Direction(-1, 0), new Direction(1, 0)] },
        { (Piece.Corner, Rotation.Zero), [new Direction(1, 0), new Direction(0, 1)] },
        { (Piece.Corner, Rotation.Ninety), [new Direction(-1, 0), new Direction(0, 1)] },
        { (Piece.Corner, Rotation.OneEighty), [new Direction(-1, 0), new Direction(0, -1)] },
        { (Piece.Corner, Rotation.TwoSeventy), [new Direction(1, 0), new Direction(0, -1)] },
        { (Piece.Tee, Rotation.Zero), [new Direction(-1, 0), new Direction(1, 0), new Direction(0, 1)] },
        { (Piece.Tee, Rotation.Ninety), [new Direction(-1, 0), new Direction(0, -1), new Direction(0, 1)] },
        { (Piece.Tee, Rotation.OneEighty), [new Direction(-1, 0), new Direction(1, 0), new Direction(0, -1)] },
        { (Piece.Tee, Rotation.TwoSeventy), [new Direction(1, 0), new Direction(0, -1), new Direction(0, 1)] }
    };
}