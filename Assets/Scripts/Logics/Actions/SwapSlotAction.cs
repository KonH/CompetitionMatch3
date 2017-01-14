using UDBase.Controllers.EventSystem;

public class SwapSlotAction:SlotAction {

	public SlotPosition NextPosition { get; private set; }

	public SwapSlotAction(SlotPosition leftPos, SlotPosition rightPos):base(leftPos){
		NextPosition = rightPos;
	}


	public override SlotState Apply(SlotState state) {
		var leftSlot = state.Slots[Position];
		var rightSlot = state.Slots[NextPosition];
		var newState = state.Clone();
		newState.Slots[Position] = rightSlot;
		newState.Slots[NextPosition] = leftSlot;
		return newState;
	}

	public override void Fire() {
		Events.Fire(new SwapSlotEvent(Position, NextPosition));
	}
}
