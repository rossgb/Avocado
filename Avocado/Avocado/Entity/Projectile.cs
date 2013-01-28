using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace Avocado
{
	class Projectile : Entity
	{
        public int damage;
        Texture2D texture;

		public Projectile(Texture2D texture, Vector2 position, float speed, int damage, Vector2 direction, int radius = 25) :
			base(texture, position, speed, radius)
		{
            this.damage = damage;
            this.Direction = direction;
            this.IsMoving = true;
            this.texture = texture;
		}

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(this.texture, this.Position, this.Body, this.Color, (float)Math.Atan2(Direction.Y, Direction.X), new Vector2(this.Radius, this.Radius), 1.0f, SpriteEffects.None, 0f);
            //base.Draw(spriteBatch);
        }
	}
}
