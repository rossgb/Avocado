using System;
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

		List<Enemy> enemies;
		List<Entity> entities;
		List<Item> items;
		List<Player> players;
		List<Projectile> projectiles;

		SpatialHash<Enemy> enemyMap;
		SpatialHash<Item> itemMap;

		const int cellSize = 30;

		#endregion

		#region Initialization

		public GameplayScreen()
		{
			// TODO: customize levels, possibly parse textfiles specify level content, scrollVelocity, etc...
			this.scrollVelocity = 0.2f;
			this.TransitionOffTime = TimeSpan.FromSeconds(1.5f);
			this.TransitionOnTime = TimeSpan.FromSeconds(0.5f);

			this.enemies = new List<Enemy>();
			this.entities = new List<Entity>();
			this.items = new List<Item>();
			this.players = new List<Player>();
			this.projectiles = new List<Projectile>();

			this.enemyMap = new SpatialHash<Enemy>(cellSize);
			this.itemMap = new SpatialHash<Item>(cellSize);
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
			// Rebuild spatial hash.
			this.enemyMap.Repopulate(this.enemies);
			this.itemMap.Repopulate(this.items);

			Rectangle bounds = this.ScreenManager.GraphicsDevice.Viewport.Bounds;

			// Check player collisions with players, enemies, and items.
			foreach (Player player in this.players)
			{
				// Clamp players to screen.
				player.Position.X = MathHelper.Clamp(player.Position.X, 0, bounds.Width);
				player.Position.Y = MathHelper.Clamp(player.Position.Y, 0, bounds.Height);

				// Players.
				foreach (Player other in this.players)
				{
					if (player == other)
					{
						continue;
					}

					if (Vector2.Distance(player.Position, other.Position) <= player.Radius + other.Radius)
					{
						Collision.resolve(player, other);
					}
				}

				// Enemies.
				foreach (Enemy enemy in this.enemyMap.Query(player))
				{
					Collision.resolve(player, enemy);
				}

				// Items.
				foreach (Item item in this.itemMap.Query(player))
				{
					Collision.resolve(player, item);
				}
			}

			// Check projectile collisions with enemies.
			foreach (Projectile projectile in this.projectiles)
			{
				foreach (Enemy enemy in this.enemyMap.Query(projectile))
				{
					Collision.resolve(projectile, enemy);
				}
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
				this.clouds.Update(gameTime);

				// Rebuild entity list.
				this.entities.Clear();
				this.entities.AddRange(this.enemies);
				this.entities.AddRange(this.items);
				this.entities.AddRange(this.players);
				this.entities.AddRange(this.projectiles);

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
			this.entities.ForEach(entity => entity.Draw(this.ScreenManager.SpriteBatch));
			this.foreground.Draw(this.ScreenManager.SpriteBatch);
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
