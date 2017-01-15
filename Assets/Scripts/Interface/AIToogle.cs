using UnityEngine;
using UnityEngine.UI;

public class AIToogle : MonoBehaviour {

	public SlotStateHolder Holder;
	public SettingsLoader Loader;

	public Toggle Toggle;

	public bool FirstPlayer;

	void Start() {
		Load();
		Toggle.onValueChanged.AddListener(OnChange);
		OnChange(Toggle.isOn);
	}

	void Load() {
		var settings = Loader.CurrentSettings;
		Toggle.isOn = FirstPlayer ? settings.AI1 : settings.AI2;
	}

	void OnChange(bool value) {
		if( FirstPlayer ) {
			Holder.AI1 = Loader.CurrentSettings.AI1 = value;
		} else {
			Holder.AI2 = Loader.CurrentSettings.AI2 = value;
		}
	}
}
