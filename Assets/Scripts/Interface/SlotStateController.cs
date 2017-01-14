using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlotStateController : MonoBehaviour {

	public SlotStateHolder Holder = null;
	SlotPosition _firstPos;
	SlotPosition _secondPos;

	void Start() {
		ResetSelection();
	}

	void ResetSelection() {
		_firstPos = SlotPosition.Empty;
		_secondPos = SlotPosition.Empty;
	}

	public void OnClick(SlotPosition newPos) {
		if( _firstPos == SlotPosition.Empty ) {
			_firstPos = newPos;
		} else {
			_secondPos = newPos;
		}
		if( _firstPos == _secondPos ) {	
			ResetSelection();
		} else if( _secondPos != SlotPosition.Empty ) {
			Holder.SwapSlots(_firstPos, _secondPos);
			ResetSelection();
		}
	}
}
