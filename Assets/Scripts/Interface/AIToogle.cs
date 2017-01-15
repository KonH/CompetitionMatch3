using UnityEngine;
using UnityEngine.UI;

public class AIToogle : MonoBehaviour {

	public SlotStateHolder Holder;

	public Toggle Toggle;

	public bool FirstPlayer;

	void Start() {
		Toggle.onValueChanged.AddListener(OnChange);
		OnChange(Toggle.isOn);
	}

	void OnChange(bool value) {
		if( FirstPlayer ) {
			Holder.AI1 = value;
		} else {
			Holder.AI2 = value;
		}
	}
}
