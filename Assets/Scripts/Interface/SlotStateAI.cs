using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlotStateAI : MonoBehaviour {

	class Variant {
		public SlotPosition LeftPos;
		public SlotPosition RightPos;

		public int Damage;

		public Variant(SlotPosition leftPos, SlotPosition rightPos, int damage) {
			LeftPos = leftPos;
			RightPos = rightPos;
			Damage = damage;
		}
	}

	public SlotStateController Controller;
	public SlotStateView View;

	public SlotStateHolder Holder;

	bool CanMakeAction() {
		return Holder.CanTurn(true) && !View.InAction;
	}

	void Update() {
		if( CanMakeAction() ) {
			TryMakeAction();
		}
	}

	void TryMakeAction() {
		var rootState = Holder.CurrentState;
		var rootHp = rootState.NextPlayer().CurrentPlayer.HP;
		var variants = new List<Variant>();
		foreach( var leftPos in rootState.Slots.Keys ) {
			foreach( var rightPos in rootState.Slots.Keys ) {
				if( SlotLogics.CanSwap(rootState, leftPos, rightPos) ) {
					var newState = SlotLogics.MakeSwap(rootState, leftPos, rightPos);
					var newHP = newState.CurrentPlayer.HP;
					var newVariant = new Variant(leftPos, rightPos, rootHp - newHP);
					variants.Add(newVariant);
				}
			}
		}
		MakeBestVariant(variants);
	}

	void MakeBestVariant(List<Variant> variants) {
		var bestVariant = variants[0];
		for( int i = 0; i < variants.Count; i++ ) {
			var curVariant = variants[i];
			if( curVariant.Damage > bestVariant.Damage ) {
				bestVariant = curVariant;
			}
		}
		Controller.ResetSelection();
		Controller.OnClick(bestVariant.LeftPos);
		Controller.OnClick(bestVariant.RightPos);
	}
}
