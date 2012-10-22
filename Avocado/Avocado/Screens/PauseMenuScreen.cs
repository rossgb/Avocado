using System.Diagnostics;
using Microsoft.Xna.Framework;

namespace Avocado
{
	class PauseMenuScreen : MenuScreen
	{
		#region Initialization

		public PauseMenuScreen() :
			base("AVOCADO")
		{
			MenuEntry resumeGameMenuEntry = new MenuEntry("RESUME");
			MenuEntry quitGameMenuEntry = new MenuEntry("MENU");

			resumeGameMenuEntry.Selected += this.OnCancel;
			quitGameMenuEntry.Selected += this.QuitGameMenuEntrySelected;

			this.MenuEntries.Add(resumeGameMenuEntry);
			this.MenuEntries.Add(quitGameMenuEntry);
		}

		#endregion

		#region Handle Input

		void QuitGameMenuEntrySelected(object sender, PlayerIndexEventArgs e)
		{
			LoadingScreen.Load(this.ScreenManager, false, null, new MainMenuScreen());
		}

		#endregion

		#region Draw

		public override void Draw(GameTime gameTime)
		{
			this.ScreenManager.SpriteBatch.Begin();
			base.Draw(gameTime);
			this.ScreenManager.SpriteBatch.End();
		}

		#endregion
	}
}
