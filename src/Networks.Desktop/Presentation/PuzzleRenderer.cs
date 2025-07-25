using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Networks.Desktop.Infrastructure;
using Networks.Engine;
using Networks.Engine.Board;

namespace Networks.Desktop.Presentation;

public class PuzzleRenderer : Game
{
    private readonly int _width;

    private readonly int _height;

    private readonly Solver _solver;

    private readonly ConcurrentQueue<Grid> _changeQueue = [];

    private readonly Stopwatch _stopwatch = new();

    private readonly FrameCounter _frameCounter = new();

    private int _skipFrames;

    private readonly TileMapper _tileMapper;

    // ReSharper disable once NotAccessedField.Local
    private GraphicsDeviceManager _graphics;

    private SpriteBatch _spriteBatch;

    private bool _isSolving;

    private bool _isComplete;

    private SpriteFont _font;

    private SpriteFont _smallFont;

    private long _stepCount;

    private long _frameCount;

    private Task _task;

    private KeyboardState _previousKeyboardState;

    private bool _dequeue;

    public Grid Grid { get; set; }

    public PuzzleRenderer()
    {
        _width = Constants.PuzzleMaxWidth * Constants.TileWidth;

        _height = Constants.PuzzleMaxHeight * Constants.TileHeight / 2 + Constants.TileHeight * 6;

        _graphics = new GraphicsDeviceManager(this)
        {
            PreferredBackBufferWidth = _width,
            PreferredBackBufferHeight = _height
        };

        Content.RootDirectory = "_Content";

        _tileMapper = new TileMapper();

        IsMouseVisible = true;

        _solver = new Solver
        {
            StepCallback = EnqueueStep
        };
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);

        _tileMapper.LoadContent(Content);

        _font = Content.Load<SpriteFont>("font");

        _smallFont = Content.Load<SpriteFont>("small-font");
    }

    protected override void Update(GameTime gameTime)
    {
        _frameCounter.Update((float) gameTime.ElapsedGameTime.TotalSeconds);

        if (! _isSolving)
        {
            _task = new Task(() =>
            {
                try
                {
                    _solver.Solve(Grid.Clone());
                }
                catch (Exception exception)
                {
                    Console.WriteLine(exception);
                }

                _isComplete = true;
            });

            _isSolving = true;

            _task.Start();
        }

        var keyboardState = Keyboard.GetState();

        _dequeue = _previousKeyboardState.IsKeyDown(Keys.Space) && keyboardState.IsKeyUp(Keys.Space);

        if (_previousKeyboardState.IsKeyDown(Keys.Right) && keyboardState.IsKeyUp(Keys.Right))
        {
            if (_skipFrames == 0)
            {
                _skipFrames = 1;
                
                _stopwatch.Start();
            }
            else
            {
                _skipFrames *= 2;
            }
            
            _skipFrames = Math.Min(_skipFrames, 1_000_000);
        }

        if (_previousKeyboardState.IsKeyDown(Keys.Left) && keyboardState.IsKeyUp(Keys.Left))
        {
            _skipFrames /= 2;

            _skipFrames = Math.Max(_skipFrames, 0);

            if (_skipFrames == 0)
            {
                _stopwatch.Stop();
            }
        }

        _previousKeyboardState = keyboardState;

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.Black);

        _spriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.AlphaBlend);

        if (! _changeQueue.IsEmpty)
        {
            if (_skipFrames == 0)
            {
                if (_dequeue)
                {
                    if (_changeQueue.TryDequeue(out var grid))
                    {
                        _frameCount++;

                        Grid = grid;
                    }
                }
            }
            else
            {
                for (var i = 0; i < _skipFrames; i++)
                {
                    if (_changeQueue.TryDequeue(out var grid))
                    {
                        _frameCount++;

                        Grid = grid;
                    }
                }
            }
        }
        else
        {
            if (_isComplete)
            {
                _stopwatch.Stop();
            }
        }

        var originX = _width / 2 - (Grid.Width - Grid.Height) * Constants.TileWidth / 2 / 2 - Constants.TileWidth / 2;

        const int originY = Constants.TileHeight * 2;

        for (var y = 0; y < Grid.Height; y++)
        {
            for (var x = 0; x < Grid.Width; x++)
            {
                var cell = Grid[x, y];

                var tile = _tileMapper.GetTile(cell);

                var isometricX = (x - y) * Constants.TileWidth / 2 + originX;

                var isometricY = (x + y) * Constants.TileCentre + originY;

                var colour = x == Grid.PowerSource.X && y == Grid.PowerSource.Y ? Color.OrangeRed :
                    cell.IsPowered ? Color.Orange : Color.White;

                _spriteBatch.Draw(tile, new Rectangle(isometricX, isometricY, Constants.TileWidth, Constants.TileHeight),
                    new Rectangle(0, 0, Constants.TileWidth, Constants.TileHeight), colour, 0, Vector2.Zero, SpriteEffects.None, (x + y) / 100f);
            }
        }

        var padding = (int) _font.MeasureString("0").X / 2;

        var fontHeight = (int) _font.MeasureString("0").Y;

        var text = $"{_frameCount:N0} / {_stepCount:N0}";

        _spriteBatch.DrawString(_font, text, new Vector2(padding * 4, _height - fontHeight * 2), Color.White);

        text = @$"{_stopwatch.Elapsed:h\:mm\:ss\.fff}";

        _spriteBatch.DrawString(_font, text, new Vector2(padding * 4, _height - fontHeight * 3), Color.White);

        text = $"{_skipFrames:N0}x";

        _spriteBatch.DrawString(_smallFont, text, new Vector2(_width - padding * 4 - _smallFont.MeasureString(text).X, _height - fontHeight * 2), Color.White);

        text = $"{_frameCounter.AverageFramesPerSecond:N0} FPS";

        _spriteBatch.DrawString(_smallFont, text, new Vector2(_width - padding * 4 - _smallFont.MeasureString(text).X, (int) (_height - fontHeight * 1.5)), Color.White);

        _spriteBatch.End();

        base.Draw(gameTime);
    }

    private void EnqueueStep(Grid grid)
    {
        _stepCount++;

        _changeQueue.Enqueue(grid.Clone());
    }
}