using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SlotPlayerView : MonoBehaviour {

	public SlotStateHolder Holder;

	public int PlayerIndex;

	public Image Image;
	
	bool IsCurrentPlayer() {
		var state = Holder.CurrentState;
		if( state.CurrentPlayer != null ) {
			return state.Players.IndexOf(state.CurrentPlayer) == PlayerIndex;
		}
		return false;
	}

	void Update () {
		Image.enabled = IsCurrentPlayer();
	}
}
