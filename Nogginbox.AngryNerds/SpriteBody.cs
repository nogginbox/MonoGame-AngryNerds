using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Genbox.VelcroPhysics.Dynamics;
using Genbox.VelcroPhysics.Factories;
using Genbox.VelcroPhysics.Utilities;

namespace Nogginbox.AngryNerds
{
    public class SpriteBody
    {
        private readonly Texture2D _texture;
        private readonly Body _body;

        private readonly Vector2 _textureOffset;
        private  Rectangle _bounds;

        public SpriteBody(World world, Texture2D texture, Vector2 positionInPixels)
        {
            _body = BodyFactory.CreateCircle(world,
                radius: ConvertUnits.ToSimUnits(texture.Width) / 2f,
                density: 10f,
                position: ConvertUnits.ToSimUnits(positionInPixels));

            _body.BodyType = BodyType.Dynamic;
            _body.Restitution = 0.8f;
            _body.Friction = 1f;

            _texture = texture;
            // The texture is drawn using top left coords, so work out offset to use
            _textureOffset = new Vector2(texture.Width / 2f, texture.Height / 2f);

            // Bounding rectangle required for checking user input from touch or mouse
            _bounds = new Rectangle((int)positionInPixels.X, (int)positionInPixels.Y, texture.Width, texture.Height);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(
                _texture,
                ConvertUnits.ToDisplayUnits(_body.Position) - _textureOffset,
                Color.White);
        }

        public void UpdateApplyForce(Vector2 forceInPixels)
        {
            _body.ApplyForce(forceInPixels * 5);
        }

        public bool PositionContained(Vector2 actionPosition)
        {
            var bodyScreenPosition = ConvertUnits.ToDisplayUnits(_body.Position) - _textureOffset;
            _bounds.X = (int)bodyScreenPosition.X;
            _bounds.Y = (int)bodyScreenPosition.Y;
            return _bounds.Contains(actionPosition);
        }
    }
}