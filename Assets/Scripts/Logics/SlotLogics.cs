using System;
using System.Collections.Generic;

public static class SlotLogics {

	static SlotState Apply(SlotState state, SlotAction action, bool fire = false) {
		if ( action != null ) {
			var newState = action.Apply(state);
			if( fire ) {
				action.Fire();
			}
			return newState;
		}
		return state;
	}

	static Slot GetRandomSlot() {
		return Slot.GetRandom();
	}

	public static SlotState FillSlots(SlotState state, bool fire = false) {
		var newState = state;
		AddSlotAction action;
		do {
			action = SlotLogics.FillFirstSlot(newState);
			if( action != null ) {
				newState = SlotLogics.Apply(newState, action, fire);
			}
		} while(action != null);
		return UpdateState(newState, fire);
	}

	static SlotState FillSlot(SlotState state, SlotPosition pos, bool fire) {
		var action = new AddSlotAction(pos, GetRandomSlot());
		return Apply(state, action, fire);
	}

	static AddSlotAction FillFirstSlot(SlotState state) {
		foreach( var slot in state.Slots ) {
			if ( slot.Value == null ) {
				return new AddSlotAction(slot.Key, GetRandomSlot());
			}
		}
		return null;
	}

	static SlotState Remove(SlotState state, SlotPosition pos, bool fire = false) {
		var newState = Apply(state, new RemoveSlotAction(pos), fire);
		return newState;
	}

	static SlotState Remove(SlotState state, List<SlotPosition> poses, bool fire) {
		var newState = state;
		for( int i = 0; i < poses.Count; i++ ) {
			newState = Apply(newState, new RemoveSlotAction(poses[i]), fire);
		}
		return newState;
	}

	public static SlotState MakeSwap(SlotState state, SlotPosition leftPos, SlotPosition rightPos, bool fire = false) {
		var newState = Swap(state, leftPos, rightPos, fire);
		return UpdateState(newState, fire);
	}

	static SlotState Swap(SlotState state, SlotPosition leftPos, SlotPosition rightPos, bool fire) {
		var action = new SwapSlotAction(leftPos, rightPos);
		return Apply(state, action, fire);
	}

	static SlotState UpdateState(SlotState state, bool fire) {
		var newState = state;
		while( IsUnstableState(newState) ) {
			newState = UpdateGravity(newState, fire);
			newState = UpdateGeneration(newState, fire);
		}
		newState = UpdateGroups(newState, fire);
		if( IsUnstableState(newState) ) {
			return UpdateState(newState, fire);
		} else {
			return newState;
		}
	}

	static bool IsUnstableState(SlotState state) {
		foreach( var slot in state.Slots ) {
			if( slot.Value == null ) {
				return true;
			}
		}
		return false;
	}

	static SlotState UpdateGravity(SlotState state, bool fire) {
		for( int x = 0; x < state.Slots.Width; x++ ) {
			for( int y = 1; y < state.Slots.Height; y++ ) {
				var bottomPos = new SlotPosition(x, y);
				var abovePos = new SlotPosition(x, y - 1);
				var hasAbove = state.Slots[abovePos] != null;
				var hasBottom = state.Slots[bottomPos] != null;
				if( hasAbove && !hasBottom ) {
					var newState = Swap(state, abovePos, bottomPos, fire);
					return UpdateGravity(newState, fire);
				}
			}
		}
		return state;
	}

	static SlotState UpdateGeneration(SlotState state, bool fire) {
		for( int x = 0; x < state.Slots.Width; x++ ) {
			var pos = new SlotPosition(x, 0);
			if( state.Slots[pos] == null ) {
				var newState = FillSlot(state, pos, fire);
				return UpdateGeneration(newState, fire);
			}
		}
		return state;
	}

	static List<SlotGroup> CalculateGroups(SlotState state) {
		var groups = new List<SlotGroup>();
		for( int x = 0; x < state.Slots.Width; x++ ) {
			for( int y = 0; y < state.Slots.Height; y++ ) {
				var pos = new SlotPosition(x, y);
				var slot = state.Slots[pos];
				if( slot != null ) {
					AddToGroup(state, pos, slot, groups);
				}
			}
		}
		var groupsLine = "Groups:";
		for( int i = 0; i < groups.Count; i++ ) {
			groupsLine += "\nGroup:\n" + groups[i].ToString();
		}
		UnityEngine.Debug.Log(groupsLine);
		return groups;
	}

	// TODO: Rewrite
	static void AddToGroup(SlotState state, SlotPosition pos, Slot slot, List<SlotGroup> groups) {
		if( groups.Count > 0 ) {
			for( int i = 0; i < groups.Count; i++ ) {
				var curGroup = groups[i];
				foreach( var item in curGroup ) {
					var curPos = item.Key;
					var curItem = item.Value;
					if( slot.Type == curItem.Type ) {
						var dx = Math.Abs(curPos.X - pos.X);
						var dy = Math.Abs(curPos.Y - pos.Y);
						if( ((dx == 0) && (dy == 1)) || ((dx == 1) && (dy == 0)) ) {
							curGroup.Add(pos, slot);
							return;
						}
					}
				}
			}
		}
		var group = new SlotGroup(state.Slots.Width, state.Slots.Height);
		group.Add(pos, slot);
		groups.Add(group);
	}

	static bool IsGroupCompleted(SlotGroup group) { 
		return group.Count >= 3;
	}

	static SlotState UpdateGroups(SlotState state, bool fire) {
		var groups = CalculateGroups(state);
		var slotsToRemove = new List<SlotPosition>();
		for( int i = 0; i < groups.Count; i++ ) {
			var curGroup = groups[i];
			if( IsGroupCompleted(curGroup) ) {
				foreach( var item in curGroup ) {
					slotsToRemove.Add(item.Key);
				}
			}
		}
		if( slotsToRemove.Count > 0 ) {
			return Remove(state, slotsToRemove, fire);
		} else {
			return state;
		}
	}
}
