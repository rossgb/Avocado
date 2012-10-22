using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;


namespace Avocado
{
	public class Entity
	{
		Texture2D texture;
		protected int velocity;
		public Vector2 Position;

		public Entity(Texture2D texture, int velocity)
		{
			this.texture = texture;
			this.velocity = velocity;
		}

		public virtual void Update(GameTime gameTime)
		{
		}

		public virtual void Draw(SpriteBatch spriteBatch)
		{
			spriteBatch.Draw(this.texture, this.Position, Color.White);
		}
	}
}
