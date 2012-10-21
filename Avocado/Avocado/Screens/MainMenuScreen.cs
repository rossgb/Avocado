using Microsoft.Xna.Framework;

namespace Avocado
{
	class MainMenuScreen : MenuScreen
	{
		#region Initialization

		public MainMenuScreen() :
			base("AVOCADO")
		{
			this.IsPopup = true;

			MenuEntry playGameMenuEntry = new MenuEntry("PLAY");
			MenuEntry quitMenuEntry = new MenuEntry("QUIT");

			playGameMenuEntry.Selected += this.PlayGameMenuEntrySelected;
			quitMenuEntry.Selected += this.OnCancel;

			this.MenuEntries.Add(playGameMenuEntry);
			this.MenuEntries.Add(quitMenuEntry);
		}

		#endregion

		#region Handle Input

		void PlayGameMenuEntrySelected(object sender, PlayerIndexEventArgs e)
		{
			// load screen!
		}

		protected override void OnCancel(PlayerIndex playerIndex)
		{
			this.ScreenManager.Game.Exit();
		}

		#endregion
	}
}
