using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics; //REMOVE ME
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

        Factory enemyFactory;
		Texture2D fireTexture;

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

            this.enemyFactory = new Factory();
		}

		public override void LoadContent()
		{
			if (this.content == null)
			{
				this.content = new ContentManager(this.ScreenManager.Game.Services, "Content");
			}

			// Create players
			this.players.Add(new Player(this.ScreenManager.BlankTexture,
				new Vector2(500, 300), 100, 0.5f, 25));
			this.players.Add(new Player(this.ScreenManager.BlankTexture, 
				new Vector2(500, 280), 100, 0.5f, 25));
			this.players[0].Color = Color.Blue;
            this.players[1].Color = Color.Red;

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

			this.TempMakeEnemy();
			this.fireTexture = this.content.Load<Texture2D>("General/fireBall");
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

        #region Game Logic

        private void ResolveCollisions()
		{
			// Rebuild spatial hash.
			this.enemyMap.Repopulate(this.enemies);
			this.itemMap.Repopulate(this.items);

			Rectangle bounds = this.ScreenManager.GraphicsDevice.Viewport.Bounds;

			// Check player collisions with players, enemies, and items.
            List<Player> playersToRemove = new List<Player>();
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
                        Vector2 distance = other.Position - player.Position;
                        float overlap = (player.Radius + other.Radius - distance.Length()) / 2.0f;
                        double angle = (float)Math.Atan2(distance.Y, distance.X);

                        player.Position.X -= (float)(overlap * Math.Cos(angle));
                        player.Position.Y -= (float)(overlap * Math.Sin(angle));
                        other.Position.X += (float)(overlap * Math.Cos(angle));
                        other.Position.Y += (float)(overlap * Math.Sin(angle));
					}
				}

				// Resolve enemy collisions.
                this.enemyMap.Query(player).ForEach(enemy =>
                {
					Random rand = new Random();
                    for (int i = 0; i < player.score; i++)
                    {
                        Vector2 coinPos = new Vector2(player.Position.X + rand.Next(20) - 10, player.Position.Y + rand.Next(20) - 10);
						Vector2 coinDir = new Vector2((float)rand.NextDouble() * 2 - 1, (float)rand.NextDouble() * 2 - 1);
                        Coin coin = new Coin(this.content.Load<Texture2D>("general/coin"),coinPos);
                        coin.Direction = coinDir;
                        items.Add(coin);
                    }

					//Player death
                    player.score = 0;
					player.Position.X = 0;
					player.Position.Y = 60;
                });
                
                // Resolve item collisions.
                this.itemMap.Query(player).ForEach(item =>
                {
                    if (item is Coin)
                    {
                        player.score += ((Coin) item).value;
                    }
                    else if (item is Enchantment)
                    {
                        // do something cool...
                    }
                    items.Remove(item);
                });
			}

            List<Projectile> projectilesToRemove = new List<Projectile>();

			// Check projectile collisions with enemies.
			foreach (Projectile projectile in this.projectiles)
			{
                this.enemyMap.Query(projectile).ForEach(enemy => 
                {
                    enemy.health -= projectile.damage;
                    projectilesToRemove.Add(projectile);

                    // Kill enemy logic: destroy & drop items
                    if (enemy.health <= 0) 
                    {
                        Random rand = new Random();
                        Vector2 coinPos = new Vector2(enemy.Position.X + rand.Next(20) - 10, enemy.Position.Y + rand.Next(20) - 10);
						Vector2 coinDir = new Vector2((float)rand.NextDouble() * 2 - 1, (float)rand.NextDouble() * 2 - 1);
                        Coin coin = new Coin(this.content.Load<Texture2D>("general/coin"), coinPos);
                        coin.Direction = coinDir;
                        items.Add(coin);
                        this.enemies.Remove(enemy);
                    }
                });
			}

            // Remove projectiles that have collided with an enemy.
            foreach (Projectile projectile in projectilesToRemove)
            {
                this.projectiles.Remove(projectile);
            }
		}

		private void ResolveCombat(GameTime gametime)
		{

			foreach (Player player in this.players)
			{
				if (gametime.TotalGameTime.TotalSeconds - player.timeSinceLastShot < player.reloadTime)
				{
					continue;
				}
				if (player.firing)
				{
					Projectile projectile = new Projectile(fireTexture,
                        player.Position+ player.Direction*player.Radius, 1.0f, player.damage);

					projectile.Direction = (player.Direction.X == 0 && player.Direction.Y == 0) ?
						new Vector2(1.0f, 0.0f) :
						new Vector2(player.Direction.X, player.Direction.Y);

					player.timeSinceLastShot = gametime.TotalGameTime.TotalSeconds;

                    projectile.Color = Color.White;

					this.projectiles.Add(projectile);
					this.entities.Add(projectile);
				}
			}
		}

		private void TempMakeEnemy() // TEMPORARY
		{
			//string = x y health speed
			Random rand = new Random();
			for (int i = 1500; i < 50000; i += 350)
			{
				string derp = i + " " + rand.Next(100, this.ScreenManager.GraphicsDevice.Viewport.Bounds.Height-100) + " 3 2";
				enemies.Add(enemyFactory.grabEnemy(derp, this.content.Load<Texture2D>("Character/playerStand")));
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
				this.clouds.Update(gameTime);

				Rectangle bounds = this.ScreenManager.GraphicsDevice.Viewport.Bounds;

				// Remove entities that have traveled offscreen.
				this.enemies.RemoveAll(enemy => enemy.Position.X + enemy.Radius < 0);
				this.items.RemoveAll(item => item.Position.X + item.Radius < 0);
				this.projectiles.RemoveAll(projectile =>
					projectile.Position.X + projectile.Radius < 0 ||
					projectile.Position.Y + projectile.Radius < 0 ||
					projectile.Position.X - projectile.Radius > bounds.Width ||
					projectile.Position.Y - projectile.Radius > bounds.Height);
				
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
				this.ResolveCombat(gameTime);

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

			//get text ready for score drawing
			SpriteBatch spriteBatch = ScreenManager.SpriteBatch;
			SpriteFont font = ScreenManager.Font;
			spriteBatch.DrawString(font,players[0].score.ToString(),new Vector2(10, 30),Color.Blue);
			spriteBatch.DrawString(font, players[1].score.ToString(), new Vector2(10, 0), Color.Red);

			if (this.TransitionPosition > 0 || this.pauseAlpha > 0)
			{
				float alpha = MathHelper.Lerp(1.0f - this.TransitionAlpha, 1.0f, this.pauseAlpha / 2);
				this.ScreenManager.FadeBackBufferToBlack(alpha);
			}
		}

		#endregion
	}
}
