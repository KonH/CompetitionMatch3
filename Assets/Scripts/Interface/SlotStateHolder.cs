using UnityEngine;
using System.Collections.Generic;

public class SlotStateHolder : MonoBehaviour {

	public int Width = 0;
	public int Height = 0;

	public SlotState CurrentState { 
		get {
			if( _currentState == null ) {
				var players = new List<SlotPlayer>();
				players.Add(new SlotPlayer(false, 10, 10));
				players.Add(new SlotPlayer(true, 10, 10));
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
