using System;
using System.Collections.Generic;
using UnityEngine;
using UDBase.Controllers.EventSystem;
using DG.Tweening;

public class SlotStateView : MonoBehaviour {

	public SlotView ItemView = null;
	public SlotStateController Controller = null;
	public SlotStateHolder Holder = null;

	RectTransform _parentTrans = null;
	float _wParentSize = 0;
	float _hParentSize = 0;
	float _wItemSize = 0;
	float _hItemSize = 0;
	float _wSpace = 0;
	float _hSpace = 0;
	float _wStart = 0;
	float _hStart = 0;
	float _hBaseIndex = 0;

	Dictionary<SlotPosition, SlotView> _slots = new Dictionary<SlotPosition, SlotView>();

	bool _inAction = false;
	Queue<Action> _actions = new Queue<Action>();

	void Awake () {
		Init();
		Subscribe();
	}

	void Init() {
		_parentTrans = GetComponent<RectTransform>();
		_wParentSize = _parentTrans.sizeDelta.x;
		_hParentSize = _parentTrans.sizeDelta.y;
		var state = Holder.CurrentState;
		var wItems = state.Slots.Width;
		var hItems = state.Slots.Height;
		_hBaseIndex = hItems - 1;
		_wItemSize = _wParentSize/wItems * 0.9f;
		_hItemSize = _hParentSize/hItems * 0.9f;
		_wSpace = _wParentSize/wItems * 0.1f;
		_hSpace = _hParentSize/hItems * 0.1f;
		_wStart = (_wSpace + _wItemSize)/2 - _wParentSize/2;
		_hStart = (_hSpace + _hItemSize)/2 - _hParentSize/2;
	}

	void Subscribe() {
		Events.Subscribe<AddSlotEvent>(this, OnAddSlotEvent);
		Events.Subscribe<RemoveSlotEvent>(this, OnRemoveSlotEvent);
		Events.Subscribe<SwapSlotEvent>(this, OnSwapSlotEvent);
	}

	void OnAddSlotEvent(AddSlotEvent e) {
		AddAction(() => CreateItem(e.Pos, e.Slot));
	}

	void OnRemoveSlotEvent(RemoveSlotEvent e) {
		AddAction(() => RemoveItem(e.Pos));
	}

	void OnSwapSlotEvent(SwapSlotEvent e) {
		AddAction(() => SwapItems(e.Pos, e.NextPos));
	}

	void AddAction(Action act) {
		if( _inAction ) {
			_actions.Enqueue(act);
		} else {
			ProcessAction(act);
		}
	}

	void OnActionComplete() {
		_inAction = false;
		if( _actions.Count > 0 ) {
			var newAct = _actions.Dequeue();
			ProcessAction(newAct);
		}
		Debug.Log(Holder.CurrentState.ToString());
	}

	void ProcessAction(Action act) {
		_inAction = true;
		act();
	}

	Vector2 GetItemPosition(SlotPosition pos) {
		var localX = _wStart;
		localX += (_wItemSize + _wSpace) * pos.X;
		var localY = _hStart;
		localY += (_hItemSize + _hSpace) * (_hBaseIndex - pos.Y);
		return new Vector2(localX, localY);
	}

	void CreateItem(SlotPosition pos, Slot slot) {
		var instance = Instantiate(ItemView) as SlotView;
		var childTrans = instance.RectTransform;
		childTrans.SetParent(_parentTrans);
		childTrans.sizeDelta = new Vector2(_wItemSize, _hItemSize);
		var transPos = GetItemPosition(pos);
		childTrans.localPosition = transPos;
		instance.Init(pos, slot, OnClick);
		if( _slots.ContainsKey(pos) ) {
			_slots[pos] = instance;
		} else {
			_slots.Add(pos, instance);
		}
		OnCreate(instance);
	}

	void OnClick(SlotPosition pos) {
		if( !_inAction ) {
			Controller.OnClick(pos);
		}
	}

	void OnCreate(SlotView view) {
		view.RectTransform.localScale = Vector3.zero;
		view.RectTransform.DOScale(Vector3.one, 0.1f).OnComplete(OnActionComplete);
	}

	void RemoveItem(SlotPosition pos) {
		var view = _slots[pos];
		_slots[pos] = null;
		OnRemove(view);
	}

	void OnRemove(SlotView view) {
		view.RectTransform.DOScale(Vector3.zero, 0.2f).OnComplete(
			() => {
				Destroy(view.gameObject);
				OnActionComplete();
			});
	}

	SlotView TryGetView(SlotPosition pos) {
		SlotView view = null;
		_slots.TryGetValue(pos, out view);
		return view;
	}

	void SwapItems(SlotPosition leftPos, SlotPosition rightPos) {
		var leftItem = TryGetView(leftPos);
		var rightItem = TryGetView(rightPos);
		var seq = DOTween.Sequence();
		UpdateItemPosition(seq, leftItem, rightPos);
		UpdateItemPosition(seq, rightItem, leftPos);
		seq.OnComplete(() => OnActionComplete());
	}

	void UpdateItemPosition(Sequence seq, SlotView view, SlotPosition pos) {
		if( view ) {
			var nextPos = GetItemPosition(pos);
			view.UpdatePosition(pos);
			seq.Insert(0, view.RectTransform.DOLocalMove(nextPos, 0.2f));
		}
		_slots[pos] = view;
	}
}
