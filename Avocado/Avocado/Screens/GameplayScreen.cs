using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
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
		float scrollVelocity;
		float pauseAlpha;
		
		List<Player> players;
		List<Entity> entities;

		#endregion

		#region Initialization

		public GameplayScreen()
		{
			// TODO: customize levels, possibly parse textfiles specify level content, scrollVelocity, etc...
			this.scrollVelocity = 0.2f;
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

			this.entities = new List<Entity>();

			this.players.Add(new Player(this.content.Load<Texture2D>("Character/playerStand"), new Vector2(500, 300), 100, 0.5f));
			this.players.Add(new Player(this.content.Load<Texture2D>("Character/playerStand"), new Vector2(500, 280), 100, 0.5f));
			this.entities.AddRange(this.players);

			this.background = new ScrollingEnvironment(
				this.content.Load<Texture2D>("Environment/background"), this.scrollVelocity,
				this.ScreenManager.GraphicsDevice.Viewport.Width);
			this.foreground = new ScrollingEnvironment(
				this.content.Load<Texture2D>("Environment/foreground"), this.scrollVelocity * 2,
				this.ScreenManager.GraphicsDevice.Viewport.Width);

			this.ScreenManager.Game.ResetElapsedTime();

			Debug.WriteLine(this.ScreenManager.GraphicsDevice.Viewport.Bounds);
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

			int playerIndex = (int) this.ControllingPlayer.Value;
			bool gamePadDisconnected = !input.CurrentGamePadStates[playerIndex].IsConnected &&
				input.GamePadWasConnected[playerIndex]; ;

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

		private void ResolveCollisions()
		{
			Rectangle bounds = this.ScreenManager.GraphicsDevice.Viewport.Bounds;

			foreach (Player player in this.players)
			{
				player.Position.X = MathHelper.Clamp(player.Position.X, 0, bounds.Width);
				player.Position.Y = MathHelper.Clamp(player.Position.Y, 0, bounds.Height);
			}
		}

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

				foreach (Entity entity in this.entities)
				{
					entity.Update(gameTime);
					entity.Position.X -= this.scrollVelocity * gameTime.ElapsedGameTime.Milliseconds;
				}

                this.ResolveCollisions();

				// Sort entities by Y position to draw in correct order.
				this.entities.Sort(delegate(Entity a, Entity b) 
				{
					return a.Position.Y.CompareTo(b.Position.Y);
				});
			}
		}

		public override void Draw(GameTime gameTime)
		{
			this.background.Draw(this.ScreenManager.SpriteBatch);
            this.entities.ForEach(entity => entity.Draw(this.ScreenManager.SpriteBatch));
			this.foreground.Draw(this.ScreenManager.SpriteBatch);

			if (this.TransitionPosition > 0 || this.pauseAlpha > 0)
			{
				float alpha = MathHelper.Lerp(1.0f - this.TransitionAlpha, 1.0f, this.pauseAlpha / 2);
				this.ScreenManager.FadeBackBufferToBlack(alpha);
			}
		}

		#endregion
	}
}
