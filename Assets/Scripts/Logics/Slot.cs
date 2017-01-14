using UDBase.Utils;

public class Slot {
    public SlotType Type { get; private set; }

    public Slot(SlotType type) {
        Type = type;
    }

	public override string ToString() {
        switch( Type ) {
            case SlotType.Blue: return "B";
            case SlotType.Green: return "G";
            case SlotType.Red: return "R";
            case SlotType.Yellow: return "Y";
            case SlotType.Cyan: return "C";
        }
        return "?";
    }

    public static Slot GetRandom() {
        var type = RandomUtils.GetEnumValue<SlotType>();
        return new Slot(type);
    }
}
