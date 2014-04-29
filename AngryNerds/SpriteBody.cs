using System.Diagnostics;
using FarseerPhysics;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace AngryNerds
{
	public class SpriteBody
	{
		private readonly Texture2D _texture;
		private readonly Body _body;

		private readonly Vector2 _offset;
		private  Rectangle _bounds;

		public SpriteBody(World world, Texture2D texture, Vector2 position)
		{
			_texture = texture;

			_body = BodyFactory.CreateCircle(world,
				ConvertUnits.ToSimUnits(texture.Width) / 2f,
				10f,
				ConvertUnits.ToSimUnits(position));

			_body.BodyType = BodyType.Dynamic;
			_body.Restitution = 0.7f;
			_body.Friction = 5f;


			_offset = new Vector2(texture.Width / 2f, texture.Height / 2f);
			_bounds = new Rectangle((int)position.X, (int)position.Y, texture.Width, texture.Height);
		}

		public void Draw(SpriteBatch spriteBatch)
		{
			spriteBatch.Draw(_texture, ConvertUnits.ToDisplayUnits(_body.Position) - _offset);
		}

		public void UpdateApplyForce(Vector2 forceInPixels)
		{
			if (forceInPixels == Vector2.Zero) return;

			var simForce = ConvertUnits.ToSimUnits(forceInPixels);
			Debug.WriteLine("Force: {0} => {1}", forceInPixels, simForce);
			_body.ApplyForce(forceInPixels * 5);
		}

		public bool PositionContained(Vector2 actionPosition)
		{
			var bodyScreenPosition = ConvertUnits.ToDisplayUnits(_body.Position) - _offset;
			_bounds.X = (int)bodyScreenPosition.X;
			_bounds.Y = (int)bodyScreenPosition.Y;
			return _bounds.Contains(actionPosition);
		}
	}
}
