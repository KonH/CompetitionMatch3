using UnityEngine;
using UnityEngine.UI;

public class FieldSizeSlider : MonoBehaviour {
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
		Slider.value = Loader.CurrentSettings.FieldSize;
	}

	void OnChange(float value) {
		var intValue = (int)value;
		Holder.Width = intValue;
		Holder.Height = intValue;
		Indicator.text = intValue.ToString();
		Loader.CurrentSettings.FieldSize = intValue;
	}
}
