using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlotPlayer {
	public bool AI { get; private set; }
	
	public SlotPlayer(bool ai) {
		AI = ai;
	}

	public SlotPlayer Clone() {
		return new SlotPlayer(AI);
	}
}
