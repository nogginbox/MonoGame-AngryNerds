using Genbox.VelcroPhysics.Dynamics;
using Genbox.VelcroPhysics.Factories;
using Genbox.VelcroPhysics.Shared;
using Genbox.VelcroPhysics.Utilities;
using Microsoft.Xna.Framework;

namespace Nogginbox.AngryNerds.Sprites
{
    public class SpriteBorder
    {
        private readonly Body _body;

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
            _body.Friction = 1f;
        }

        public void RemoveFromWorld()
        {
            _body.RemoveFromWorld();
        }
    }
}