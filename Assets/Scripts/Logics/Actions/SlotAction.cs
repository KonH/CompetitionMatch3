public abstract class SlotAction {

	public SlotPosition Position { get; private set; }

	public SlotAction(SlotPosition pos) {
		Position = pos;
	}

	public abstract SlotState Apply(SlotState state);

	public abstract void Fire();
}
