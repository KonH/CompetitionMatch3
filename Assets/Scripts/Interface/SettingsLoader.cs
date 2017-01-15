using UnityEngine;
using UDBase.Controllers.SaveSystem;

public class SettingsLoader : MonoBehaviour {
	public GameSettings CurrentSettings {
		get {
			var settings = Save.GetNode<GameSettings>();
			if( settings == null ) {
				settings = new GameSettings();
				Save.SaveNode(settings);
			}
			return settings;
		}
	}

	public void SaveSettings() {
		Save.SaveNode(CurrentSettings);
	}
}
