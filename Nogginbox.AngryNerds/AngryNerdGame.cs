using Genbox.VelcroPhysics.Dynamics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Nogginbox.AngryNerds
{
	public class AngryNerdGame : Game
	{
	private GraphicsDeviceManager _graphics;
	private SpriteBatch _spriteBatch;

	private World _world;
	private IList<SpriteBody> _nerdObjects;
	private SpriteBorder _spriteBorder;
	private Texture2D _headTexture;

	public AngryNerdGame()
	{
		_graphics = new GraphicsDeviceManager(this);
		Content.RootDirectory = "Content";
	}

	protected override void Initialize()
	{
		// Setup world with downwards gravity and add a border to stop us loosing objects
		_world = new World(new Vector2(0, 1f));
		_spriteBorder = new SpriteBorder(_world, Window.ClientBounds);
			
		// Setup touch and enable make mouse visible
		IsMouseVisible = true;

		Window.AllowUserResizing = true;
        Window.ClientSizeChanged += Window_ClientSizeChanged;

		base.Initialize();
	}

        private void Window_ClientSizeChanged(object sender, EventArgs e)
        {
			var oldBorder = _spriteBorder;
            _spriteBorder = new SpriteBorder(_world, Window.ClientBounds);
			oldBorder.RemoveFromWorld();
		}

        protected override void LoadContent()
	{
		_spriteBatch = new SpriteBatch(GraphicsDevice);

		// Create main nerd and a pyramid of nerds
		_headTexture = Content.Load<Texture2D>("Nogginhead");
		var nerdObjects = new List<SpriteBody>
		{
			new SpriteBody(_world, _headTexture, new Vector2(10, 10))
		};
		var newNerds = CreateNerdPyramid(_headTexture, 450, 5);
		nerdObjects.AddRange(newNerds);
		_nerdObjects = nerdObjects.ToList();
	}

	private IEnumerable<SpriteBody> CreateNerdPyramid(Texture2D headTexture, int firstNerdX, int nerdHeight)
	{
		var nerdPositions = new List<Vector2>();
		var nerdsInCurrentRow = nerdHeight;
		for (var row = 0; row < nerdHeight; row++)
		{
			var rowY = Window.ClientBounds.Height - row*110 - 64;
			for (var col = 0; col < nerdsInCurrentRow; col++)
			{
				nerdPositions.Add(new Vector2(firstNerdX + col*128, rowY));
			}
			nerdsInCurrentRow--;
			firstNerdX += 64;
		}
		return nerdPositions.Select(nerdPosition => new SpriteBody(_world, headTexture, nerdPosition));
	}


	protected override void Update(GameTime gameTime)
	{
		_world.Step(Math.Min((float)gameTime.ElapsedGameTime.TotalSeconds, (1f / 30f)));

		var position = GetInputPosition();

		// Get force
		Vector2? force = null;
		Vector2 forceActionPosition = Vector2.Zero;

		// Calculate force using last position and update last position
		if (position != null)
		{
			if (_lastPosition != null)
			{
				force = position.Value - _lastPosition;
				forceActionPosition = _lastPosition.Value;
			}

			_lastPosition = position.Value;	
		}
		else
		{
			_lastPosition = null;
		}

		FindActiveNerd(force, forceActionPosition);

		// Apply force to active nerd
		if (force != null && _activeNerd != null)
		{
			_activeNerd.UpdateApplyForce(force.Value);
		}
			
		base.Update(gameTime);
	}

	protected override void Draw(GameTime gameTime)
	{
		GraphicsDevice.Clear(Color.Black);

		_spriteBatch.Begin();
		for (var i = 0; i < _nerdObjects.Count; i++)
		{
			_nerdObjects[i].Draw(_spriteBatch);
		}
		_spriteBatch.End();

		base.Draw(gameTime);
	}

	#region Touch/Mouse input helper methods

	private void FindActiveNerd(Vector2? force, Vector2 forceActionPosition)
	{
		if (force == null)
		{
			Debug.WriteLineIf(_activeNerd != null, "Nerd nulled");
			_activeNerd = null;
		}
		else if (_activeNerd == null)
		{
			for (var i = 0; i < _nerdObjects.Count; i++)
			{
				if (_nerdObjects[i].PositionContained(forceActionPosition))
				{
					_activeNerd = _nerdObjects[i];
					break;
				}
			}
		}
	}

	private static Vector2? GetInputPosition()
	{
		Vector2? position = null;
		var touch = TouchPanel.GetState().FirstOrDefault();
		if (touch.State != TouchLocationState.Invalid)
		{
			position = touch.Position;
		}
		else
		{
			var mouseState = Mouse.GetState();
			if (mouseState.LeftButton == ButtonState.Pressed)
			{
				position = new Vector2(mouseState.Position.X, mouseState.Position.Y);
			}
		}
		return position;
	}

	private Vector2? _lastPosition;
	private SpriteBody _activeNerd;

	#endregion
	}
}