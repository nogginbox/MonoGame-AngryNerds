using FarseerPhysics;
using FarseerPhysics.Common;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;
using Microsoft.Xna.Framework;

namespace AngryNerds
{
	public class SpriteBorder
	{
		private Body _body;

		public SpriteBorder(World world, Rectangle borderRectangle)
		{
			var width = ConvertUnits.ToSimUnits(borderRectangle.Width);
			var height = ConvertUnits.ToSimUnits(borderRectangle.Height);

			var borders = new Vertices(4)
			{
				new Vector2(0, height),
				new Vector2(width, height),
				new Vector2(width, 0),
				new Vector2(0, 0)
			};

			_body = BodyFactory.CreateLoopShape(world, borders);
			_body.Friction = 10f;
		}
	}
}
