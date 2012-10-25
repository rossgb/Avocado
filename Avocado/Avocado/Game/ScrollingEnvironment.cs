using System;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Avocado
{
	public class ScrollingEnvironment
	{
		#region Fields

		Texture2D texture;
		Vector2[] positions;

		float velocity;

		#endregion

		public ScrollingEnvironment(Texture2D texture, float velocity, int screenWidth)
		{
			this.texture = texture;
			this.velocity = velocity;
			this.positions = new Vector2[(int)Math.Ceiling((double)screenWidth / this.texture.Width) + 1];

			for (int i = 0; i < this.positions.Length; i++)
			{
				this.positions[i] = new Vector2(i * this.texture.Width, 0);
			}
		}

		public void Update(GameTime gameTime)
		{
			// wrap texture if necessary
			for (int i = 0; i < this.positions.Length; i++)
			{
				if (this.positions[i].X <= -this.texture.Width)
				{
					this.positions[i].X = this.positions[(i + 1) % this.positions.Length].X + this.texture.Width;
					break;
				}
			}


			for (int i = 0; i < this.positions.Length; i++)
			{
				this.positions[i].X -= this.velocity * gameTime.ElapsedGameTime.Milliseconds;
			}
		}

		public void Draw(SpriteBatch spriteBatch)
		{
			for (int i = 0; i < this.positions.Length; i++)
			{
				spriteBatch.Draw(this.texture, this.positions[i], Color.White);
			}
		}
	}
}
