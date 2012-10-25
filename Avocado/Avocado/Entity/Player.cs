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

		public Player(Texture2D texture, Vector2 position, int health, int speed) : 
			base(texture, position, speed)
		{
			this.health = health;
		}

		public void HandleInput(InputState input, int index)
		{
			this.Direction 
				= input.CurrentGamePadStates[index].ThumbSticks.Left;
			this.Direction.Y *= -1;
		}
	}
}
