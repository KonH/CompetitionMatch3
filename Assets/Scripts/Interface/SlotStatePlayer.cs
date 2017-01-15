using UnityEngine;
using UnityEngine.EventSystems;

public class SlotStatePlayer : MonoBehaviour {
	public EventSystem EventSystem;

	public SlotStateHolder Holder;

	void Update() {
		EventSystem.enabled = Holder.CanTurn(false) || (Holder.CurrentState.Status == TurnType.End);
	}
}
