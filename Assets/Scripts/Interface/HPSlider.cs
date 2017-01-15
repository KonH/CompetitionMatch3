using UnityEngine;
using UnityEngine.UI;

public class HPSlider : MonoBehaviour {

	public SlotStateHolder Holder;
	public SettingsLoader Loader;

	public Slider Slider;

	public Text Indicator;

	void Start() {
		Load();
		Slider.onValueChanged.AddListener(OnChange);
		OnChange(Slider.value);
	}

	void Load() {
		Slider.value = Loader.CurrentSettings.HP;
	}

	void OnChange(float value) {
		var intValue = (int)value;
		Holder.HP = intValue;
		Indicator.text = intValue.ToString();
		Loader.CurrentSettings.HP = intValue;
	}
}
