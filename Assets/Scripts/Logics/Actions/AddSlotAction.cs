using UDBase.Controllers.EventSystem;

public class AddSlotAction:SlotAction {

	public Slot Slot { get; private set; }

	public AddSlotAction(SlotPosition pos, Slot slot):base(pos){
		Slot = slot;
	}


	public override SlotState Apply(SlotState state) {
		var newState = state.Clone();
		newState.Slots[Position] = Slot;
		return newState;
	}

	public override void Fire() {
		Events.Fire(new AddSlotEvent(Position, Slot));
	}
}
