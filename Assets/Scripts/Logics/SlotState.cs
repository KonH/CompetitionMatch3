using System.Collections.Generic;

public class SlotState {
	public TurnType Status { get; private set; }

	public List<SlotPlayer> Players { get; private set; }
	public SlotPlayer CurrentPlayer { get; private set; }
	public SlotGroup Slots { get; private set; }

	public SlotState(int width, int height, List<SlotPlayer> players) {
		Slots = new SlotGroup(width, height);
		for( int x = 0; x < width; x++ ) {
			for( int y = 0; y < height; y++ ) {
				Slots.Add(new SlotPosition(x, y), null);
			}
		}
		Players = new List<SlotPlayer>();
		for( int i = 0; i < players.Count; i++ ) {
			Players.Add(players[i].Clone());
		}
	}

	public SlotState Clone() {
		var clone = new SlotState(Slots.Width, Slots.Height, Players);
		clone.Status = Status;
		foreach( var slot in Slots ) {
			clone.Slots[slot.Key] = slot.Value;
		}
		clone.CurrentPlayer = CurrentPlayer != null ? clone.Players[Players.IndexOf(CurrentPlayer)] : null;
		return clone;
	}

	public SlotState ChangeStatus(TurnType type) {
		var clone = Clone();
		clone.Status = type;
		return clone;
	}

	public SlotState NextPlayer() {
		var clone = Clone();
		clone.CurrentPlayer = clone.FindNextPlayer();
		return clone;
	}

	SlotPlayer FindNextPlayer() {
		if( CurrentPlayer == null ) {
			return Players[0];
		} else {
			var currentIndex = Players.IndexOf(CurrentPlayer);
			var newIndex = currentIndex + 1;
			if( newIndex >= Players.Count ) {
				newIndex = 0;
			}
			return Players[newIndex];
		}
	}

	public SlotState DecreasePlayerHP(int index, int damage) {
		var clone = Clone();
		clone.Players[index] = clone.Players[index].DecreaseHP(damage);
		return clone;
	}

	public override string ToString() {
		var str = string.Format("Status: {0}, Player 1: {1}, Player 2: {2}\n", Status, Players[0].HP, Players[1].HP);
		str += "Slots:\n";
		str += Slots.ToString();
		return str;
	}
}
