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
			this.reloadTime = 0.4;
			this.timeSinceLastShot = 0.0;
            this.damage = 1;
            this.score = 0;
			
		}

		public void HandleInput(InputState input, int index)
		{
			KeyboardState keyboardState = input.CurrentKeyboardStates[index];
			GamePadState gamePadState = input.CurrentGamePadStates[index];

			Vector2 direction = Vector2.Zero;

			// XBox controller input. 
			this.Direction.X += gamePadState.ThumbSticks.Left.X;
			this.Direction.Y -= gamePadState.ThumbSticks.Left.Y;

			// Hacky keyboard input for development purposes.
			if (keyboardState.IsKeyDown(index == 0 ? Keys.A : Keys.Left))
				direction.X--;

			if (keyboardState.IsKeyDown(index == 0 ? Keys.D : Keys.Right))
				direction.X++;

			if (keyboardState.IsKeyDown(index == 0 ? Keys.W : Keys.Up))
				direction.Y--;

			if (keyboardState.IsKeyDown(index == 0 ? Keys.S : Keys.Down))
				direction.Y++;

			// Normalization only necessary for keyboard input.
			if (direction.Length() > 1.0f)
			{
				direction.Normalize();
			}

			if (direction.Length() == 0)
			{
				this.IsMoving = false;
			}
			else
			{
				this.Direction = direction;
				this.IsMoving = true;
			}
			
			//fire ze missiles
			if (keyboardState.IsKeyDown(index == 0 ? Keys.Space : Keys.Enter))
				this.firing = true;
			else
				this.firing = false;
		}
	}
}
