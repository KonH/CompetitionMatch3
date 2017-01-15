using UnityEngine;
using UnityEngine.UI;

public class HPSlider : MonoBehaviour {

	public SlotStateHolder Holder;

	public Slider Slider;

	public Text Indicator;

	void Start() {
		Slider.onValueChanged.AddListener(OnChange);
		OnChange(Slider.value);
	}

	void OnChange(float value) {
		var intValue = (int)value;
		Holder.HP = intValue;
		Indicator.text = intValue.ToString();
	}
}
