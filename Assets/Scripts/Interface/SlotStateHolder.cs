using UnityEngine;
using System.Collections.Generic;

public class SlotStateHolder : MonoBehaviour {

	public int Width = 0;
	public int Height = 0;

	public bool AI1;
	public bool AI2;

	public int HP;

	public SlotState CurrentState { 
		get {
			if( _currentState == null ) {
				var players = new List<SlotPlayer>();
				players.Add(new SlotPlayer(AI1, HP, HP));
				players.Add(new SlotPlayer(AI2, HP, HP));
				_currentState = new SlotState(Width, Height, players);
			}
			return _currentState;
		} 
	}
	SlotState _currentState = null;

	void Start () {
		GenerateSlots();
	}

	void GenerateSlots() {
		_currentState = SlotLogics.FillSlots(CurrentState, true);
	}

	public void SwapSlots(SlotPosition leftPos, SlotPosition rightPos) {
		if( SlotLogics.CanSwap(_currentState, leftPos, rightPos) ) {
			_currentState = SlotLogics.MakeSwap(_currentState, leftPos, rightPos, true);
		}
	}

	public bool CanTurn(bool ai) {
		if ( CurrentState.Status == TurnType.PlayerTurn ) {
			return ai == CurrentState.CurrentPlayer.AI;
		}
		return false;
	}
}
