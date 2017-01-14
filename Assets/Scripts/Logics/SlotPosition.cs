using System;

[Serializable]
public struct SlotPosition {
	public int X { get; private set; }
	public int Y { get; private set; }

	public SlotPosition(int x, int y) {
		X = x;
		Y = y;
	}

	public override string ToString() {
		return string.Format("{0}:{1}", X, Y);
	}

	public static SlotPosition Empty {
		get {
			return new SlotPosition(-1, -1);
		}
	}

	public override bool Equals(object obj) {
		return IsEquals(this, (SlotPosition)obj);
    }

	public override int GetHashCode() {
    	return X * 0x00010000 + Y;
    }

	public static bool operator ==(SlotPosition leftPos, SlotPosition rightPos) {
		return IsEquals(leftPos, rightPos);
	}

	public static bool operator !=(SlotPosition leftPos, SlotPosition rightPos) {
		return !IsEquals(leftPos, rightPos);
	}

	static bool IsEquals(SlotPosition leftPos, SlotPosition rightPos) {
		return 
		(leftPos.X == rightPos.X) &&
		(leftPos.Y == rightPos.Y);
	}
}
