using UDBase.Controllers.EventSystem;

public class RemoveSlotAction:SlotAction {

	public RemoveSlotAction(SlotPosition pos):base(pos){}


	public override SlotState Apply(SlotState state) {
		var newState = state.Clone();
		newState.Slots[Position] = null;
		return newState;
	}

	public override void Fire() {
		Events.Fire(new RemoveSlotEvent(Position));
	}
}
