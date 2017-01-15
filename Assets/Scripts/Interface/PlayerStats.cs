using UnityEngine;
using UnityEngine.UI;

public class PlayerStats : MonoBehaviour {
	public StatsLoader Loader;

	public Text Text;

	public bool IsHuman;

	void Start() {
		var stats = Loader.Stats;
		Text.text = string.Format(Text.text, IsHuman ? stats.HumanWins : stats.AIWins);
	}

}
