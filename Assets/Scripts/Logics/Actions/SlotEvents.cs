public struct AddSlotEvent {
	public SlotPosition Pos;
	public Slot Slot;

	public AddSlotEvent(SlotPosition pos, Slot slot) {
		Pos = pos;
		Slot = slot;
	}
}

public struct RemoveSlotEvent {
	public SlotPosition Pos;

	public RemoveSlotEvent(SlotPosition pos) {
		Pos = pos;
	}
}

public struct SwapSlotEvent {
	public SlotPosition Pos;
	public SlotPosition NextPos;

	public SwapSlotEvent(SlotPosition pos, SlotPosition nextPos) {
		Pos = pos;
		NextPos = nextPos;
	}
}
