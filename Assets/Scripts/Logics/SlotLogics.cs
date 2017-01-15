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
		} else if ( IsBlockedState(newState) ) {
			newState = ResolveState(newState, fire);
			return UpdateState(newState, fire);
		} else {
			newState = UpdateStatus(newState, fire);
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
		var groups = CreateGroups(state.Slots);
		var groupsLine = "Groups:";
		for( int i = 0; i < groups.Count; i++ ) {
			groupsLine += "\nGroup:\n" + groups[i].ToString();
		}
		UnityEngine.Debug.Log(groupsLine);
		return groups;
	}

	static List<SlotGroup> CreateGroups(SlotGroup parent) {
		var groups = new List<SlotGroup>();
		var allPos = new List<SlotPosition>(parent.Keys);
		while( allPos.Count > 0 ) {
			var curPos = allPos[0];
			var curSlot = parent[curPos];
			if( curSlot != null ) {
				var xGroup = new SlotGroup(parent.Width, parent.Height);
				xGroup.Add(curPos, curSlot);
				AddAllRightSlots(curPos, curSlot.Type, parent, xGroup, allPos);
				if( xGroup.Count > 1 ) {
					groups.Add(xGroup);
				}
				for( int x = 0; x < xGroup.Count; x++ ) {
					var yGroup = new SlotGroup(parent.Width, parent.Height);
					var pos = new SlotPosition(curPos.X + x, curPos.Y);
					yGroup.Add(pos, curSlot);
					AddAllBottomSlots(pos, curSlot.Type, parent, yGroup, allPos);
					if( yGroup.Count > 1 ) {
						groups.Add(yGroup);
					}
				}
			}
			allPos.Remove(curPos);
		}
		return groups;
	}

	static void AddAllRightSlots(SlotPosition pos, SlotType type, SlotGroup parent, SlotGroup group, List<SlotPosition> allPos) {
		var x = pos.X;
		do {
			x++;
			var rightPos = new SlotPosition(x, pos.Y);
			Slot rightSlot = null;
			if( parent.TryGetValue(rightPos, out rightSlot) ) {
				if( rightSlot.Type == type ) {
					group.Add(rightPos, rightSlot);
					allPos.Remove(rightPos);
					continue;
				}
			}
			break;
		} while(true);
	}

	static void AddAllBottomSlots(SlotPosition pos, SlotType type, SlotGroup parent, SlotGroup group, List<SlotPosition> allPos) {
		var y = pos.Y;
		do {
			y++;
			var bottomPos = new SlotPosition(pos.X, y);
			Slot bottomSlot = null;
			if( parent.TryGetValue(bottomPos, out bottomSlot) ) {
				if( bottomSlot.Type == type ) {
					group.Add(bottomPos, bottomSlot);
					allPos.Remove(bottomPos);
					continue;
				}
			}
			break;
		} while(true);
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
					if( !slotsToRemove.Contains(item.Key) ) {
						slotsToRemove.Add(item.Key);
					}
				}
			}
		}
		if( slotsToRemove.Count > 0 ) {
			return Remove(state, slotsToRemove, fire);
		} else {
			return state;
		}
	}

	static bool HasAnyCompletedGroup(SlotState state) {
		var groups = CalculateGroups(state);
		for( int i = 0; i < groups.Count; i++ ) {
			var curGroup = groups[i];
			if( IsGroupCompleted(curGroup) ) {
				return true;
			}
		}
		return false;
	} 

	static SlotState UpdateStatus(SlotState state, bool fire) {
		var newState = state;
		if( state.Status == TurnType.Break ) {
			newState = state.ChangeStatus(TurnType.PlayerTurn);
		}
		newState = newState.NextPlayer();
		return newState;
	}

	public static bool CanSwap(SlotState state, SlotPosition leftPos, SlotPosition rightPos) {
		var dx = Math.Abs(leftPos.X - rightPos.X);
		var dy = Math.Abs(leftPos.Y - rightPos.Y);
		if( (dx == 1 && dy == 0) || (dx == 0 && dy == 1) ) {
			var newState = Swap(state, leftPos, rightPos, false);
			return HasAnyCompletedGroup(newState);
		}
		return false;
	}

	static bool IsBlockedState(SlotState state) {
		foreach( var leftKey in state.Slots.Keys ) {
			foreach( var rightKey in state.Slots.Keys ) {
				if( CanSwap(state, leftKey, rightKey) ) {
					return false;
				}
			}
		}
		return true;
	}
	static SlotState ResolveState(SlotState state, bool fire) {
		var newState = state.ChangeStatus(TurnType.Break);
		while( HasAnyCompletedGroup(newState) || IsBlockedState(newState) ) {
			var allPos = new List<SlotPosition>(newState.Slots.Keys);
			newState = Remove(newState, allPos, fire);
			newState = FillSlots(newState, fire);
		}
		return newState;
	}
}
