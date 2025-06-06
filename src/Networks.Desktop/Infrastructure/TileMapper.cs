using System.Collections.Generic;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Networks.Engine.Board;

namespace Networks.Desktop.Infrastructure;

public class TileMapper
{
    private readonly Dictionary<Cell, Texture2D> _tiles = new();

    public Texture2D GetTile(Cell cell)
    {
        return _tiles[cell];
    }

    public void LoadContent(ContentManager contentManager)
    {
        _tiles.Add(new Cell(Piece.Corner, Rotation.Zero), contentManager.Load<Texture2D>("north-east"));
        
        _tiles.Add(new Cell(Piece.Corner, Rotation.Zero), contentManager.Load<Texture2D>("south-east"));
        
        _tiles.Add(new Cell(Piece.Corner, Rotation.Zero), contentManager.Load<Texture2D>("south-west"));
        
        _tiles.Add(new Cell(Piece.Corner, Rotation.Zero), contentManager.Load<Texture2D>("north-west"));
        
        _tiles.Add(new Cell(Piece.Corner, Rotation.Zero), contentManager.Load<Texture2D>("vertical"));
        
        _tiles.Add(new Cell(Piece.Corner, Rotation.Zero), contentManager.Load<Texture2D>("horizontal"));
    }
}