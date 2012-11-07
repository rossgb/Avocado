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
		ScrollingEnvironment clouds;

		float scrollVelocity;
		float pauseAlpha;

		List<Entity> entities;
		List<Player> players;
		List<Enemy> enemies;
		List<Item> items;
		List<Projectile> projectiles;

		SpatialHash enemyHash;
		SpatialHash itemHash;

		#endregion

		#region Initialization

		public GameplayScreen()
		{
			// TODO: customize levels, possibly parse textfiles specify level content, scrollVelocity, etc...
			this.scrollVelocity = 0.2f;
			this.TransitionOffTime = TimeSpan.FromSeconds(1.5f);
			this.TransitionOnTime = TimeSpan.FromSeconds(0.5f);

			this.enemies = new List<Enemies();
			this.items = new List<Item>();
			this.players = new List<Player>();
			this.projectiles = new List<Projectiles>();
			
			this.enemyHash = new SpatialHash(30);
			this.itemHash = new SpatialHash(30);
		}

		public override void LoadContent()
		{
			if (this.content == null)
			{
				this.content = new ContentManager(this.ScreenManager.Game.Services, "Content");
			}

			// Create players.
			this.players.Add(new Player(this.ScreenManager.BlankTexture,
				new Vector2(500, 300), 100, 0.5f));
			this.players.Add(new Player(this.ScreenManager.BlankTexture, 
				new Vector2(500, 280), 100, 0.5f));
			this.players[0].Color = Color.Blue;

			// Add players to entities.
			this.entities.AddRange(this.players);

			// Create scrolling enivronment for level.
			this.background = new ScrollingEnvironment(
				this.content.Load<Texture2D>("Environment/background"), this.scrollVelocity,
				this.ScreenManager.GraphicsDevice.Viewport.Width);
			this.foreground = new ScrollingEnvironment(
				this.content.Load<Texture2D>("Environment/foreground"), this.scrollVelocity * 1.5f,
				this.ScreenManager.GraphicsDevice.Viewport.Width);
			this.clouds = new ScrollingEnvironment(
				this.content.Load<Texture2D>("Environment/clouds"), this.scrollVelocity * 2,
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
			this.enemyHash.Repopulate(this.enemies);
			this.itemHash.Repopulate(this.items);

			Rectangle bounds = this.ScreenManager.GraphicsDevice.Viewport.Bounds;

			foreach (Player player in this.players)
			{
				player.Position.X = MathHelper.Clamp(player.Position.X, 0, bounds.Width);
				player.Position.Y = MathHelper.Clamp(player.Position.Y, 0, bounds.Height);

				List<Entity> collisionCandidates = this.spatialHash.Query(player);
				
				// Resolve collisions!
			}

			foreach(Projectile projectile in this.projectiles)
			{
				List<Enemy> toPotentiallyBeDecimated = this.enemyHash.Query(projectile);
				// Decimate enemies here.
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
				this.entities

				this.background.Update(gameTime);
				this.foreground.Update(gameTime);
				this.clouds.Update(gameTime);

				foreach (Entity entity in this.entities)
				{
					entity.Update(gameTime);
					entity.Position.X -= this.scrollVelocity * gameTime.ElapsedGameTime.Milliseconds;
				}

				this.ResolveCollisions();

				// Sort entities by Y position to draw in correct order.
				this.entities.Sort((a, b) => a.Position.Y.CompareTo(b.Position.Y));
			}
		}

		public override void Draw(GameTime gameTime)
		{
			this.background.Draw(this.ScreenManager.SpriteBatch);
			//this.entities.ForEach(entity => entity.Draw(this.ScreenManager.SpriteBatch));
			this.foreground.Draw(this.ScreenManager.SpriteBatch);
			// TEMPORARY FOR COLLISION TESTING
			this.entities.ForEach(entity => entity.Draw(this.ScreenManager.SpriteBatch));
			this.clouds.Draw(this.ScreenManager.SpriteBatch);

			if (this.TransitionPosition > 0 || this.pauseAlpha > 0)
			{
				float alpha = MathHelper.Lerp(1.0f - this.TransitionAlpha, 1.0f, this.pauseAlpha / 2);
				this.ScreenManager.FadeBackBufferToBlack(alpha);
			}
		}

		#endregion
	}
}
