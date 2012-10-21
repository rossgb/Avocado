using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Avocado
{
	class AvocadoGame : Game
	{
		#region Fields

		GraphicsDeviceManager graphics;
		ScreenManager screenManager;

		#endregion

		public AvocadoGame()
		{
			this.Content.RootDirectory = "Content";
			
			this.graphics = new GraphicsDeviceManager(this);
			this.graphics.IsFullScreen = true;
			this.graphics.PreferredBackBufferWidth = 2048;
			this.graphics.PreferredBackBufferHeight = 768;

			this.screenManager = new ScreenManager(this);
			this.screenManager.AddScreen(new MainMenuScreen(), null);
			this.Components.Add(screenManager);
		}

		protected override void Draw(GameTime gameTime)
		{
			this.graphics.GraphicsDevice.Clear(Color.Black);
			base.Draw(gameTime);
		}
	}

	static class Program
	{
		static void Main()
		{
			using (AvocadoGame game = new AvocadoGame())
			{
				game.Run();
			}
		}
	}
}
