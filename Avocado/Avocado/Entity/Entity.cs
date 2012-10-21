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

		public Vector2 position;

		public int Width
		{
			get { return texture.Width; }
		}
		public int Height
		{
			get { return texture.Height; }
		}

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
			spriteBatch.Draw(this.texture, this.position, Color.White);
		}
	}
}
