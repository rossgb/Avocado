using System;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Avocado
{

	public class Player : Entity
	{
		public int health;
		public bool firing;
		public double timeSinceLastShot;
		public double reloadTime;
        public int damage;
        public int score;

		public Player(Texture2D texture, Vector2 position, int health, float speed, int radius) : 
			base(texture, position, speed, radius)
		{
			this.health = health;
			this.firing = false;
			this.reloadTime = 0.5;
			this.timeSinceLastShot = 0.0;
            this.damage = 1;
            this.score = 100;
		}

		public void HandleInput(InputState input, int index)
		{
			KeyboardState keyboardState = input.CurrentKeyboardStates[index];
			GamePadState gamePadState = input.CurrentGamePadStates[index];
            
			this.Direction = Vector2.Zero;
			
			// XBox controller input.
			this.Direction.X += gamePadState.ThumbSticks.Left.X;
			this.Direction.Y -= gamePadState.ThumbSticks.Left.Y;

			// Hacky keyboard input for development purposes.
			if (keyboardState.IsKeyDown(index == 0 ? Keys.A : Keys.Left))
				this.Direction.X--;

			if (keyboardState.IsKeyDown(index == 0 ? Keys.D : Keys.Right))
				this.Direction.X++;

			if (keyboardState.IsKeyDown(index == 0 ? Keys.W : Keys.Up))
				this.Direction.Y--;

			if (keyboardState.IsKeyDown(index == 0 ? Keys.S : Keys.Down))
				this.Direction.Y++;

			// Normalization only necessary for keyboard input.
			if (this.Direction.Length() > 1.0f)
			{
				this.Direction.Normalize();
			}
			
			//fire ze missiles
			if (keyboardState.IsKeyDown(index == 0 ? Keys.Space : Keys.NumPad0))
				this.firing = true;
			else
				this.firing = false;
		}
	}
}
