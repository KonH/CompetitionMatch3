public class SlotState {
	public SlotGroup Slots { get; private set; }

	public SlotState(int width, int height) {
		Slots = new SlotGroup(width, height);
		for( int x = 0; x < width; x++ ) {
			for( int y = 0; y < height; y++ ) {
				Slots.Add(new SlotPosition(x, y), null);
			}
		}
	}

	public SlotState Clone() {
		var clone = new SlotState(Slots.Width, Slots.Height);
		foreach( var slot in Slots ) {
			clone.Slots[slot.Key] = slot.Value;
		}
		return clone;
	}

	public override string ToString() {
		var str = "State:\n";
		str += "Slots:\n";
		str += Slots.ToString();
		return str;
	}
}
