using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using System;

public class GolDisplay : RawImage //, IDragHandler, IPointerDownHandler, IPointerUpHandler
{
	public static GolDisplay Instance;
	private RectTransform _rectTransform;

	public event Action<Vector2> OnTouch;
	private Vector2 _halfSize;
	public bool inputEnabled {get; set;}
	private const int scale = 16; 

	private void Awake ()
	{
		if (Instance != null)
		{
			Destroy(this);
			return;

		}
		Instance = this;
		_rectTransform = rectTransform;

		//ScrollView sv = new ScrollView();
		//aaa bbb= new aaa();
		//sv.AddAdapter(bbb);
	}
	private void Start ()
	{
		Rect transformRect = _rectTransform.rect;
		_halfSize = new Vector2(transformRect.width/2f, transformRect.height/2f);
	}

	public void Resize ()
	{
		_rectTransform.sizeDelta = new Vector2(texture.width, texture.height);
	}
	public void Resize (Vector2 size)
	{
		_rectTransform.sizeDelta = size*scale;
	}
	/*
	public void OnDrag (PointerEventData data)
	{
		if (inputEnabled)
		{
			HandlerPositionChanged (ref data);
		}
		else
		{
			GetComponentInParent<ScrollRect>().OnDrag(data);
		}
	}

	public void OnPointerDown (PointerEventData data)
	{
		if (inputEnabled)
		{
			HandlerPositionChanged (ref data);
		}
		else
		{
			//GetComponentInParent<ScrollRect>().SendMessage("OnPointerDown", data);
		}
	}

	public void OnPointerUp (PointerEventData data)
	{
		if (inputEnabled)
		{
			HandlerPositionChanged (ref data);
		}
		else
		{
			//GetComponentInParent<ScrollRect>().SendMessage("OnPointerUp", data);
		}
	}
	*/
	private void HandlerPositionChanged (ref PointerEventData data)
	{
		Vector2 localPos;
		RectTransformUtility.ScreenPointToLocalPointInRectangle (_rectTransform, data.position, data.pressEventCamera, out localPos);

		localPos.x = localPos.x/_halfSize.x;
		localPos.y = localPos.y/_halfSize.y;

		if (OnTouch != null) OnTouch(localPos);
	}

	/*
	private class aaa : Adapter<PickerLeftUIElement, object>
	{
		public aaa () : base(new List<object>(), null)
		{

		}

		public override void OnBeforeElementDestroyed (PickerLeftUIElement script)
		{
			throw new NotImplementedException ();
		}
		public override void OnBeforeElementHidden (PickerLeftUIElement script)
		{
			throw new NotImplementedException ();
		}
		public override void OnBeforeElementVisible (PickerLeftUIElement script)
		{
			throw new NotImplementedException ();
		}
	}
	*/
}
