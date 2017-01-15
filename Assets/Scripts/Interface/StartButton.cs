using UnityEngine;
using UnityEngine.UI;

public class StartButton : MonoBehaviour {
	public Button Button;
	public SettingsLoader Loader;

	public GameObject SettingsView;
	public GameObject GameView;

	void Start() {
		Button.onClick.AddListener(OnClick);
	}

	void OnClick() {
		Loader.SaveSettings();
		SettingsView.SetActive(false);
		GameView.SetActive(true);
	}
}
