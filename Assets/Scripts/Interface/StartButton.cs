using UnityEngine;
using UnityEngine.UI;

public class StartButton : MonoBehaviour {
	public Button Button;

	public GameObject SettingsView;
	public GameObject GameView;

	void Start() {
		Button.onClick.AddListener(OnClick);
	}

	void OnClick() {
		SettingsView.SetActive(false);
		GameView.SetActive(true);
	}
}
