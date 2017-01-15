using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlotPlayer {
	public bool AI { get; private set; }
	public int HP { get; private set; }
	public int MaxHP { get; private set; }
	
	public SlotPlayer(bool ai, int hp, int maxHp) {
		AI = ai;
		HP = hp;
		MaxHP = maxHp;
	}

	public SlotPlayer Clone() {
		return new SlotPlayer(AI, HP, MaxHP);
	}

	public SlotPlayer DecreaseHP(int count) {
		var clone = Clone();
		clone.HP -= Mathf.Min(count, HP);
		return clone;
	}
}
