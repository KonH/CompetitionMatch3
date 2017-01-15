using UnityEngine;
using UDBase.Controllers.SaveSystem;

public class StatsLoader : MonoBehaviour {

	public GameStats Stats {
		get {
			var stats = Save.GetNode<GameStats>();
			if( stats == null ) {
				stats = new GameStats();
				Save.SaveNode(stats);
			}
			return stats;
		}
	}

	public void SaveStats() {
		Save.SaveNode(Stats);
	}
}
