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

		public Entity(Texture2D texture, Vector2 position, int speed)
		{
			this.texture = texture;
			this.Position = position;
			this.speed = speed;
		}

		public virtual void Update(GameTime gameTime)
		{
			this.Position += this.Direction * this.speed * gameTime.ElapsedGameTime.Milliseconds;
		}

		public virtual void Draw(SpriteBatch spriteBatch)
		{
			spriteBatch.Draw(this.texture, this.Position, Color.White);
		}
	}
}
