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
        for (var i = 0; i < 2; i++)
        {
            var powered = i == 1;
            
            _tiles.Add(new Cell(Piece.Corner, Rotation.Zero, powered), contentManager.Load<Texture2D>("south-east"));

            _tiles.Add(new Cell(Piece.Corner, Rotation.Ninety, powered), contentManager.Load<Texture2D>("south-west"));

            _tiles.Add(new Cell(Piece.Corner, Rotation.OneEighty, powered), contentManager.Load<Texture2D>("north-west"));

            _tiles.Add(new Cell(Piece.Corner, Rotation.TwoSeventy, powered), contentManager.Load<Texture2D>("north-east"));

            _tiles.Add(new Cell(Piece.Straight, Rotation.Zero, powered), contentManager.Load<Texture2D>("vertical"));

            _tiles.Add(new Cell(Piece.Straight, Rotation.Ninety, powered), contentManager.Load<Texture2D>("horizontal"));

            _tiles.Add(new Cell(Piece.Straight, Rotation.OneEighty, powered), contentManager.Load<Texture2D>("vertical"));

            _tiles.Add(new Cell(Piece.Straight, Rotation.TwoSeventy, powered), contentManager.Load<Texture2D>("horizontal"));

            _tiles.Add(new Cell(Piece.Tee, Rotation.Zero, powered), contentManager.Load<Texture2D>("east-south-west"));

            _tiles.Add(new Cell(Piece.Tee, Rotation.Ninety, powered), contentManager.Load<Texture2D>("north-south-west"));

            _tiles.Add(new Cell(Piece.Tee, Rotation.OneEighty, powered), contentManager.Load<Texture2D>("north-east-west"));

            _tiles.Add(new Cell(Piece.Tee, Rotation.TwoSeventy, powered), contentManager.Load<Texture2D>("north-south-east"));

            _tiles.Add(new Cell(Piece.Terminator, Rotation.Zero, powered), contentManager.Load<Texture2D>("end-north"));

            _tiles.Add(new Cell(Piece.Terminator, Rotation.Ninety, powered), contentManager.Load<Texture2D>("end-east"));

            _tiles.Add(new Cell(Piece.Terminator, Rotation.OneEighty, powered), contentManager.Load<Texture2D>("end-south"));

            _tiles.Add(new Cell(Piece.Terminator, Rotation.TwoSeventy, powered), contentManager.Load<Texture2D>("end-west"));
        }
    }
}