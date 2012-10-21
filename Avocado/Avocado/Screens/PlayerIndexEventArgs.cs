using System;
using Microsoft.Xna.Framework;

namespace Avocado
{
	class PlayerIndexEventArgs : EventArgs
	{
		PlayerIndex playerIndex;

		public PlayerIndex PlayerIndex
		{
			get { return playerIndex; }
		}

		public PlayerIndexEventArgs(PlayerIndex playerIndex)
		{
			this.playerIndex = playerIndex;
		}
	}
}
