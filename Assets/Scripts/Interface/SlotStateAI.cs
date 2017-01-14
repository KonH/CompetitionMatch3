using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlotStateAI : MonoBehaviour {

	public SlotStateController Controller;
	public SlotStateView View;

	public SlotStateHolder Holder;

	bool CanMakeAction() {
		return !View.InAction;
	}

	void Update() {
		if( CanMakeAction() ) {
			TryMakeAction();
		}
	}

	void TryMakeAction() {
		Debug.Log("Try");
		var rootState = Holder.CurrentState;
		foreach( var leftPos in rootState.Slots.Keys ) {
			foreach( var rightPos in rootState.Slots.Keys ) {
				if( SlotLogics.CanSwap(rootState, leftPos, rightPos) ) {
					Controller.ResetSelection();
					Controller.OnClick(leftPos);
					Controller.OnClick(rightPos);
					break;
				}
			}
		} 
	}
}
