using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SlotPlayerView : MonoBehaviour {

	public SlotStateHolder Holder;

	public int PlayerIndex;

	public Text Text;
	public Image Image;
	public Slider Slider;
	
	bool IsCurrentPlayer() {
		var state = Holder.CurrentState;
		if( state.CurrentPlayer != null ) {
			return state.Players.IndexOf(state.CurrentPlayer) == PlayerIndex;
		}
		return false;
	}

	bool CheckEnded() {
		var state = Holder.CurrentState;
		if( state.Status == TurnType.End ) {
			if( state.Players[PlayerIndex].HP > 0 ) {
				Text.text += " won!";
			} else {
				Text.text += " lose!";
			}
			return true;
		}
		return false;
	}

	void Update () {
		Image.enabled = IsCurrentPlayer();
		Slider.value = GetHP();
		if( CheckEnded() ) {
			enabled = false;
		}
	}

	float GetHP() {
		var state = Holder.CurrentState;
		var player = state.Players[PlayerIndex];
		return (float)player.HP / player.MaxHP;
	}
}
