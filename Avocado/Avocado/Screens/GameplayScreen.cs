﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
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
		//List<Enemy> enemies;

		#endregion

		#region Initialization

		public GameplayScreen()
		{
			// TODO: customize levels, possibly parse textfiles specify level content, scrollVelocity, etc...
			this.scrollVelocity = 200;
		}

		public override void LoadContent()
		{
			if (this.content == null)
			{
				this.content = new ContentManager(this.ScreenManager.Game.Services, "Content");
			}

			this.players = new List<Player>();
			this.players.Add(new Player(this.content.Load<Texture2D>("Character/playerStand"), 10, 100));
			
			//this.enemies = new List<Enemy>(); //needs to be filled with a factory
			
			this.background = new ScrollingEnvironment(
				this.content.Load<Texture2D>("Environment/background"), this.scrollVelocity,
				this.ScreenManager.GraphicsDevice.Viewport.Width);
			this.foreground = new ScrollingEnvironment(
				this.content.Load<Texture2D>("Environment/foreground"), this.scrollVelocity * 2,
				this.ScreenManager.GraphicsDevice.Viewport.Width);
		}

		public override void UnloadContent()
		{
			this.content.Unload();
		}

		#endregion

		#region Handle Input

		public override void HandleInput(InputState input)
		{
			if (input.IsPauseGame(null))
			{
				this.ScreenManager.Game.Exit();
			}

			for (int i = 0; i < this.players.Count; i++)
			{
				this.players[i].HandleInput(input, i);
			}
		}

		#endregion

		#region Update and Draw

		public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
		{
			base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);

			this.pauseAlpha = coveredByOtherScreen ?
				Math.Min(this.pauseAlpha + 1.0f / 32, 1) :
				Math.Max(this.pauseAlpha - 1.0f / 32, 0);

			if (this.IsActive)
			{
				this.background.Update(gameTime);
				this.foreground.Update(gameTime);

				foreach (Player player in this.players)
				{
					player.Position.X -= this.scrollVelocity * gameTime.ElapsedGameTime.Milliseconds / 1000.0f;
				}

				foreach (Player player in this.players)
				{
					player.Update(gameTime);
				}
			}
		}

		public override void Draw(GameTime gameTime)
		{
			this.ScreenManager.SpriteBatch.Begin();
			this.background.Draw(this.ScreenManager.SpriteBatch);

			foreach (Player player in this.players)
			{
				player.Draw(this.ScreenManager.SpriteBatch);
			}

			this.foreground.Draw(this.ScreenManager.SpriteBatch);
			this.ScreenManager.SpriteBatch.End();
		}

		#endregion
	}
}
