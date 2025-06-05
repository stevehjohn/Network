namespace Networks.Engine.Board;

public static class Connector
{
    public static readonly Dictionary<(Piece, Rotation), List<(int Dx, int Dy)>> Connections = new()
    {
        { (Piece.Terminator, Rotation.Zero), [(-1, 0)] },
        { (Piece.Terminator, Rotation.Ninety), [(0, -1)] },
        { (Piece.Terminator, Rotation.OneEighty), [(1, 0)] },
        { (Piece.Terminator, Rotation.TwoSeventy), [(0, 1)] },
        { (Piece.Straight, Rotation.Zero), [(0, -1), (0, 1)] },
        { (Piece.Straight, Rotation.Ninety), [(-1, 0), (1, 0)] },
        { (Piece.Straight, Rotation.OneEighty), [(0, -1), (0, 1)] },
        { (Piece.Straight, Rotation.TwoSeventy), [(-1, 0), (1, 0)] },
        { (Piece.Corner, Rotation.Zero), [(1, 0), (0, 1)] },
        { (Piece.Corner, Rotation.Ninety), [(-1, 0), (0, 1)] },
        { (Piece.Corner, Rotation.OneEighty), [(-1, 0), (0, -1)] },
        { (Piece.Corner, Rotation.TwoSeventy), [(1, 0), (0, -1)] },
        { (Piece.Tee, Rotation.Zero), [(-1, 0), (1, 0), (0, 1)] },
        { (Piece.Tee, Rotation.Ninety), [(-1, 0), (0, -1), (0, 1)] },
        { (Piece.Tee, Rotation.OneEighty), [(-1, 0), (1, 0), (0, -1)] },
        { (Piece.Tee, Rotation.TwoSeventy), [(1, 0), (0, -1), (0, 1)] }
    };
}