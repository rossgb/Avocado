using System;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;


namespace Avocado
{
	public class Player : Entity
	{
		int health;

		public Player(Texture2D texture, int health, int speed) : 
			base(texture, speed)
		{
			this.health = health;
		}

		public override void Update(GameTime gameTime)
		{
			base.Update(gameTime);
		}

		public override void Draw(SpriteBatch spriteBatch)
		{
			base.Draw(spriteBatch);
		}

		public void HandleInput(InputState input, int index)
		{
			this.Direction = input.CurrentGamePadStates[index].ThumbSticks.Left;
			this.Direction.Y *= -1;
		}
	}
}
