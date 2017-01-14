using UnityEngine;

public class SlotStateHolder : MonoBehaviour {

	public int Width = 0;
	public int Height = 0;

	public SlotState CurrentState { 
		get {
			if( _currentState == null ) {
				_currentState = new SlotState(Width, Height);
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
		_currentState = SlotLogics.MakeSwap(_currentState, leftPos, rightPos, true);
	}
}
