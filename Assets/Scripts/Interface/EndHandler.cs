using UnityEngine;
using UnityEngine.UI;
using UDBase.Controllers.SceneSystem;

public class EndHandler : MonoBehaviour {
	public GameObject Panel;
	public Button RetryButton;
	public StatsLoader Loader;

	public SlotStateHolder Holder;

	void Start() {
		Panel.SetActive(false);
		RetryButton.onClick.AddListener(OnRetry);
	}

	void Update() {
		var ended = Holder.CurrentState.Status == TurnType.End;
		Panel.SetActive(ended);
		if( ended ) {
			if( IsHumanWon() ) {
				Loader.Stats.HumanWins++;
			} else {
				Loader.Stats.AIWins++;
			}
			Loader.SaveStats();
			enabled = false;
		}
	}

	bool IsHumanWon() {
		var state = Holder.CurrentState;
		for( int i = 0; i < state.Players.Count; i++ ) {
			var player = state.Players[i];
			if( player.HP > 0 ) {
				return !player.AI;
			}
		}
		return false;
	}

	void OnRetry() {
		Scene.ReloadScene();
	}
}
