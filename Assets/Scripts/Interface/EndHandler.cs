using UnityEngine;
using UnityEngine.UI;
using UDBase.Controllers.SceneSystem;

public class EndHandler : MonoBehaviour {
	public GameObject Panel;
	public Button RetryButton;

	public SlotStateHolder Holder;

	void Start() {
		Panel.SetActive(false);
		RetryButton.onClick.AddListener(OnRetry);
	}

	void Update() {
		Panel.SetActive(Holder.CurrentState.Status == TurnType.End);
	}

	void OnRetry() {
		Scene.ReloadScene();
	}
}
