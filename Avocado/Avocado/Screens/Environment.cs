using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Avocado
{
	public class Environment : GameObject
	{
		Texture2D backgroundTexture;
		Texture2D foregroundTexture;

		Vector2[] backgroundPositions;
		Vector2[] foregroundPositions;

		int backgroundVelocity;
		int foregroundVelocity;

		public Environment(Texture2D backgroundTexture, Texture2D foregroundTexture,
			int backgroundVelocity, int foregroundVelocity, int screenWidth)
		{
			this.backgroundTexture = backgroundTexture;
			this.foregroundTexture = foregroundTexture;

			this.backgroundVelocity = backgroundVelocity;
			this.foregroundVelocity = foregroundVelocity;

			this.backgroundPositions = new Vector2[screenWidth / this.backgroundTexture.Width];
			this.foregroundPositions = new Vector2[screenWidth / this.foregroundTexture.Width];

			for (int i = 0; i < this.backgroundPositions.Length; i++)
			{
				this.backgroundPositions[i] = new Vector2(i * this.backgroundTexture.Width, 0);
			}

			for (int i = 0; i < this.foregroundPositions.Length; i++)
			{
				this.foregroundPositions[i] = new Vector2(i * this.foregroundTexture.Width, 0);
			}
		}

		public void Update(GameTime gameTime)
		{
			for (int i = 0; i < this.backgroundPositions.Length; i++)
			{
				this.backgroundPositions[i].X += this.backgroundVelocity * gameTime.ElapsedGameTime.Seconds;
			}

			for (int i = 0; i < this.foregroundPositions.Length; i++)
			{
				this.foregroundPositions[i].X += this.foregroundVelocity * gameTime.ElapsedGameTime.Seconds;
			}
		}

		public void Draw(SpriteBatch spriteBatch)
		{
			for (int i = 0; i < this.backgroundPositions.Length; i++)
			{
				spriteBatch.Draw(this.backgroundTexture, this.backgroundPositions[i], Color.White);
			}

			for (int i = 0; i < this.foregroundPositions.Length; i++)
			{
				spriteBatch.Draw(this.foregroundTexture, this.foregroundPositions[i], Color.White);
			}
		}
	}
}
