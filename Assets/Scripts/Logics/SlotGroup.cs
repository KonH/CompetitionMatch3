using System.Collections.Generic;

public class SlotGroup : Dictionary<SlotPosition, Slot> {
	
	public int Width { get; private set; }
	public int Height { get; private set; }

	public SlotGroup(int width, int height) {
		Width = width;
		Height = height;
	}

	public override string ToString() {
		var str = "";
		for( int y = 0; y < Height; y++ ) {
			for( int x = 0; x < Width; x++ ) {
				var pos = new SlotPosition(x, y);
				Slot slot = null;
				if( TryGetValue(pos, out slot) ) {
					str += slot == null ? "?" : slot.ToString();
				} else {
					str += "...";
				}
				str += " ";
			}
			str += "\n";
		}
		return str;
	}
}
