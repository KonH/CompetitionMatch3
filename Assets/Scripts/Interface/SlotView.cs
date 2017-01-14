using System;
using UnityEngine;
using UnityEngine.UI;

public class SlotView : MonoBehaviour {
	public RectTransform RectTransform;
	public Image Image;
	public Button Button;
	public Text Text;

	public SlotPosition Position;
	public Slot Slot = null;

	Action<SlotPosition> _callback;

	public void Init(SlotPosition pos, Slot slot, Action<SlotPosition> onClick) {
		UpdatePosition(pos);
		Slot = slot;
		_callback = onClick;
		Image.color = GetSlotColor(slot);
		Button.onClick.AddListener(() => OnClick());
	}

	public void UpdatePosition(SlotPosition pos) {
		gameObject.name = pos.ToString();
		Text.text = pos.ToString();
		Position = pos;
	}

	void OnClick() {
		_callback(Position);
	}

	Color GetSlotColor(Slot slot) {
		if( slot != null ) {
			switch( slot.Type ) {
				case SlotType.Blue: return Color.blue;
				case SlotType.Green: return Color.green;
				case SlotType.Red: return Color.red;
				case SlotType.Yellow: return Color.yellow;
				case SlotType.Cyan: return Color.cyan;
			}
		}
		return Color.magenta;
	}
}
