using System;
using System.Collections.Generic;
using System.Linq
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Avocado
{
	class GameplayScreen : GameScreen
	{
		#region Fields

		ContentManager content;
		ScrollingEnvironment background;
		ScrollingEnvironment foreground;
		int scrollVelocity;
		float pauseAlpha;
		
		List<Player> players;
		List<Enemy> enemies;
		List<Entity> entities;

		#endregion

		#region Initialization

		public GameplayScreen()
		{
			// TODO: customize levels, possibly parse textfiles specify level content, scrollVelocity, etc...
			this.scrollVelocity = 200;
			this.TransitionOffTime = TimeSpan.FromSeconds(1.5f);
			this.TransitionOnTime = TimeSpan.FromSeconds(0.5f);
		}

		public override void LoadContent()
		{
			if (this.content == null)
			{
				this.content = new ContentManager(this.ScreenManager.Game.Services, "Content");
			}

			this.players = new List<Player>();
			this.enemies = new List<Enemy>();

			this.players.Add(new Player(this.content.Load<Texture2D>("Character/playerStand"), 100, 10));

			this.background = new ScrollingEnvironment(
				this.content.Load<Texture2D>("Environment/background"), this.scrollVelocity,
				this.ScreenManager.GraphicsDevice.Viewport.Width);
			this.foreground = new ScrollingEnvironment(
				this.content.Load<Texture2D>("Environment/foreground"), this.scrollVelocity * 2,
				this.ScreenManager.GraphicsDevice.Viewport.Width);

			this.ScreenManager.Game.ResetElapsedTime();
		}

		public override void UnloadContent()
		{
			this.content.Unload();
		}

		#endregion

		#region Handle Input

		public override void HandleInput(InputState input)
		{
			if (input == null)
			{
				throw new ArgumentNullException("input");
			}

			bool gamePadDisconnected = false;

			if (input.IsPauseGame(this.ControllingPlayer) || gamePadDisconnected)
			{
				this.ScreenManager.AddScreen(new PauseMenuScreen(), this.ControllingPlayer);
			}
			else
			{
				for (int i = 0; i < this.players.Count; i++)
				{
					this.players[i].HandleInput(input, i);
				}
			}
		}

		#endregion

		#region Update and Draw

		public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
		{
			base.Update(gameTime, otherScreenHasFocus, false);

			this.pauseAlpha = coveredByOtherScreen ?
				Math.Min(this.pauseAlpha + 1.0f / 32, 1) :
				Math.Max(this.pauseAlpha - 1.0f / 32, 0);

			if (this.IsActive)
			{
				this.background.Update(gameTime);
				this.foreground.Update(gameTime);

				this.entities = this.players.Concat(this.enemies).ToList();

				foreach (Entity entity in this.entities)
				{
					entity.Update(gameTime)
				}

				foreach (Entity entity in this.entities)
				{
					// TODO: should update direction, not position
					entity.Position.X -= this.scrollVelocity * gameTime.ElapsedGameTime.Milliseconds / 1000.0f;
				}

				// TODO: sort entities based on Y position
			}
		}

		public override void Draw(GameTime gameTime)
		{
			this.ScreenManager.SpriteBatch.Begin();
			this.background.Draw(this.ScreenManager.SpriteBatch);

			foreach (Entity entity in this.entities)
			{
				entity.Draw(this.ScreenManager.SpriteBatch);
			}

			this.foreground.Draw(this.ScreenManager.SpriteBatch);
			this.ScreenManager.SpriteBatch.End();

			if (this.TransitionPosition > 0 || this.pauseAlpha > 0)
			{
				float alpha = MathHelper.Lerp(1.0f - this.TransitionAlpha, 1.0f, this.pauseAlpha / 2);
				this.ScreenManager.FadeBackBufferToBlack(alpha);
			}
		}

		#endregion
	}
}
