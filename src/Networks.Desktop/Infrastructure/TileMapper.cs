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
        _tiles.Add(new Cell(Piece.Corner, Rotation.Zero), contentManager.Load<Texture2D>("south-east"));
        
        _tiles.Add(new Cell(Piece.Corner, Rotation.Ninety), contentManager.Load<Texture2D>("south-west"));
        
        _tiles.Add(new Cell(Piece.Corner, Rotation.OneEighty), contentManager.Load<Texture2D>("north-west"));
        
        _tiles.Add(new Cell(Piece.Corner, Rotation.TwoSeventy), contentManager.Load<Texture2D>("north-east"));
        
        _tiles.Add(new Cell(Piece.Straight, Rotation.Zero), contentManager.Load<Texture2D>("vertical"));
        
        _tiles.Add(new Cell(Piece.Straight, Rotation.Ninety), contentManager.Load<Texture2D>("horizontal"));

        _tiles.Add(new Cell(Piece.Straight, Rotation.OneEighty), contentManager.Load<Texture2D>("vertical"));
        
        _tiles.Add(new Cell(Piece.Straight, Rotation.TwoSeventy), contentManager.Load<Texture2D>("horizontal"));
        
        _tiles.Add(new Cell(Piece.Tee, Rotation.Zero), contentManager.Load<Texture2D>("east-south-west"));
        
        _tiles.Add(new Cell(Piece.Tee, Rotation.Ninety), contentManager.Load<Texture2D>("north-south-west"));
        
        _tiles.Add(new Cell(Piece.Tee, Rotation.OneEighty), contentManager.Load<Texture2D>("north-east-west"));
        
        _tiles.Add(new Cell(Piece.Tee, Rotation.TwoSeventy), contentManager.Load<Texture2D>("north-east-south"));
    }
}