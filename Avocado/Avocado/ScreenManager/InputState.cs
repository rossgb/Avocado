using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace Avocado
{
	public class InputState
	{
		public const int MaxInputs = 4;
		public readonly GamePadState[] CurrentGamePadStates;
		public readonly GamePadState[] LastGamePadStates;
		public readonly bool[] GamePadWasConnected;

		public InputState()
		{
			this.CurrentGamePadStates = new GamePadState[MaxInputs];
			this.LastGamePadStates = new GamePadState[MaxInputs];
			this.GamePadWasConnected = new bool[MaxInputs];
		}

		public void Update()
		{
			for (int i = 0; i < MaxInputs; i++)
			{
				LastGamePadStates[i] = CurrentGamePadStates[i];
				CurrentGamePadStates[i] = GamePad.GetState((PlayerIndex)i);

				if (CurrentGamePadStates[i].IsConnected)
				{
					GamePadWasConnected[i] = true;
				}
			}
		}

		public bool IsNewButtonPress(Buttons button, PlayerIndex? controllingPlayer, out PlayerIndex playerIndex)
		{
			if (controllingPlayer.HasValue)
			{
				playerIndex = controllingPlayer.Value;

				int i = (int)playerIndex;

				return (CurrentGamePadStates[i].IsButtonDown(button) && 
					LastGamePadStates[i].IsButtonUp(button));
			}
			else
			{
				return IsNewButtonPress(button, PlayerIndex.One, out playerIndex) ||
					IsNewButtonPress(button, PlayerIndex.Two, out playerIndex) ||
					IsNewButtonPress(button, PlayerIndex.Three, out playerIndex) ||
					IsNewButtonPress(button, PlayerIndex.Four, out playerIndex);
			}
		}

		public bool IsMenuSelect(PlayerIndex? controllingPlayer,
								 out PlayerIndex playerIndex)
		{
			return IsNewButtonPress(Buttons.A, controllingPlayer, out playerIndex);
		}

		public bool IsMenuCancel(PlayerIndex? controllingPlayer,
								 out PlayerIndex playerIndex)
		{
			return IsNewButtonPress(Buttons.B, controllingPlayer, out playerIndex);
		}

		public bool IsMenuUp(PlayerIndex? controllingPlayer)
		{
			PlayerIndex playerIndex;

			return IsNewButtonPress(Buttons.LeftThumbstickUp, controllingPlayer, out playerIndex);
		}

		public bool IsMenuDown(PlayerIndex? controllingPlayer)
		{
			PlayerIndex playerIndex;

			return IsNewButtonPress(Buttons.LeftThumbstickDown, controllingPlayer, out playerIndex);
		}

		public bool IsPauseGame(PlayerIndex? controllingPlayer)
		{
			PlayerIndex playerIndex;

			return IsNewButtonPress(Buttons.Start, controllingPlayer, out playerIndex);
		}
	}
}

