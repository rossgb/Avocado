using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Avocado
{
	class MainMenuScreen : MenuScreen
	{
		#region Fields

		ContentManager content;
		ScrollingEnvironment background;
		ScrollingEnvironment foreground;
		Texture2D menuBackdrop;
		Rectangle menuBackdropArea;
		float scrollVelocity;

		#endregion

		#region Initialization

		public MainMenuScreen() :
			base("AVOCADO")
		{
			this.scrollVelocity = 0.3f;

			MenuEntry playGameMenuEntry = new MenuEntry("PLAY");
			MenuEntry quitMenuEntry = new MenuEntry("QUIT");

			playGameMenuEntry.Selected += this.PlayGameMenuEntrySelected;
			quitMenuEntry.Selected += this.OnCancel;

			this.MenuEntries.Add(playGameMenuEntry);
			this.MenuEntries.Add(quitMenuEntry);
		}

		public override void LoadContent()
		{
			if (this.content == null)
			{
				this.content = new ContentManager(this.ScreenManager.Game.Services, "Content");
			}

			this.background = new ScrollingEnvironment(
				this.content.Load<Texture2D>("Environment/background"), this.scrollVelocity,
				this.ScreenManager.GraphicsDevice.Viewport.Width);
			this.foreground = new ScrollingEnvironment(
				this.content.Load<Texture2D>("Environment/foreground"), this.scrollVelocity * 2,
				this.ScreenManager.GraphicsDevice.Viewport.Width);
			this.menuBackdrop = new Texture2D(this.ScreenManager.GraphicsDevice, 1, 1);
			this.menuBackdrop.SetData(new[] { Color.Black });
			this.menuBackdropArea = new Rectangle(
				this.ScreenManager.GraphicsDevice.Viewport.Width / 8, 0,
				this.ScreenManager.GraphicsDevice.Viewport.Width / 4, 
				this.ScreenManager.GraphicsDevice.Viewport.Height);
		}

		public override void UnloadContent()
		{
			this.content.Unload();
		}

		#endregion

		#region Handle Input

		void PlayGameMenuEntrySelected(object sender, PlayerIndexEventArgs e)
		{
			LoadingScreen.Load(ScreenManager, false, e.PlayerIndex, new GameplayScreen());
		}

		protected override void OnCancel(PlayerIndex playerIndex)
		{
			this.ScreenManager.Game.Exit();
		}

		#endregion

		#region Update and Draw

		public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
		{
			base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);
			this.background.Update(gameTime);
			this.foreground.Update(gameTime);
		}

		public override void Draw(GameTime gameTime)
		{
			this.background.Draw(this.ScreenManager.SpriteBatch);
			this.ScreenManager.SpriteBatch.Draw(this.menuBackdrop, this.menuBackdropArea, Color.White * 0.5f);
			
			// Draw menu items.
			base.Draw(gameTime);

			this.foreground.Draw(this.ScreenManager.SpriteBatch);
		}


		#endregion
	}
}
