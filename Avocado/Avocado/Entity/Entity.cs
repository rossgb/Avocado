using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;


namespace Avocado
{
	public class Entity
	{
		Texture2D texture;
		int speed;
		
		public Vector2 Direction;
		public Vector2 Position;

		public Entity(Texture2D texture, int speed)
		{
			this.texture = texture;
			this.speed = speed;
		}

		public virtual void Update(GameTime gameTime)
		{
			this.Position += this.Direction * this.speed;
		}

		public virtual void Draw(SpriteBatch spriteBatch)
		{
			spriteBatch.Draw(this.texture, this.Position, Color.White);
		}
	}
}
