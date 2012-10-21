using System;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Avocado
{
	class GameplayScreen : GameScreen
	{
		ContentManager content;
		Environment environment;

		float pauseAlpha;

		public GameplayScreen(ContentManager content)
		{
			this.content = content;
		}

		public override void LoadContent()
		{
			this.environment = new Environment(
				this.content.Load<Texture2D>("Background/background"), 
				this.content.Load<Texture2D>("Background/foreground"), 
				-1, -2, this.ScreenManager.GraphicsDevice.Viewport.Width);
		}

		public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
		{
			base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);

			this.pauseAlpha = coveredByOtherScreen ? 
				Math.Min(this.pauseAlpha + 1.0f / 32, 1) :
				Math.Max(this.pauseAlpha - 1.0f / 32, 0);

			if (this.IsActive)
			{
				this.environment.Update(gameTime);
			}
		}

		public override void HandleInput(InputState input)
		{
			input.Update();



			if (input.IsPauseGame(null))
			{
				this.ExitScreen();
			}
		}
	}
}
