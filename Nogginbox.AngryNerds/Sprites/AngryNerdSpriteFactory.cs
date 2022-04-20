using Genbox.VelcroPhysics.Dynamics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Nogginbox.AngryNerds.Sprites
{
    public class AngryNerdSpriteFactory
    {
        private readonly Texture2D _angryNerdTexture;
        private readonly World _world;

        public AngryNerdSpriteFactory(World world, ContentManager contentManager)
        {
            _world = world;
            _angryNerdTexture = contentManager.Load<Texture2D>("Nogginhead");
        }

        public SpriteBody CreateAngryNerd(Vector2 position)
        {
            return new SpriteBody(_world, _angryNerdTexture, position);
        }
    }
}