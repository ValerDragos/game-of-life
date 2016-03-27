using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using System;

public class PickerLeftUIElement : MaskableGraphic , IPointerDownHandler, IDragHandler  
{
	private static readonly Color bottomLeftColor = new Color(0,0,0,1);
	private static readonly Color topLeftColor = new Color(1,1,1,1);
	private static readonly Color bottomRightColor = new Color(0,0,0,1);

	[SerializeField] private Image handle = null;
	[SerializeField] private Color topRightColor = Color.white;

	public Color TopRightColor
	{
		get
		{
			return topRightColor;
		}
		set
		{
			topRightColor = new Color(value.r, value.g, value.b, 1);
			SetHandleColor ();
			SetAllDirty();
		}
	}

	private Vector2 _handlePos = Vector2.one;
	private RectTransform _rectTransform;

	public event Action<Color> OnHandleColorChanged;

	private void Awake ()
	{
		_rectTransform = rectTransform;
	}

	protected override void OnFillVBO (List<UIVertex> vbo)
	{
		int size = vbo.Count;

		Rect pixelAdjustedRect = this.GetPixelAdjustedRect ();
		Vector4 vector = new Vector4 (pixelAdjustedRect.x, pixelAdjustedRect.y, pixelAdjustedRect.x + pixelAdjustedRect.width, pixelAdjustedRect.y + pixelAdjustedRect.height);
		UIVertex simpleVert = UIVertex.simpleVert;

		simpleVert.position = new Vector3 (vector.x, vector.y);
		//simpleVert.uv0 = new Vector2 (0f, 0f);
		simpleVert.color = bottomLeftColor;
		vbo.Add (simpleVert);
		simpleVert.position = new Vector3 (vector.x, vector.w);
		//simpleVert.uv0 = new Vector2 (0f, 1f);
		simpleVert.color = topLeftColor;
		vbo.Add (simpleVert);
		simpleVert.position = new Vector3 (vector.z, vector.w);
		//simpleVert.uv0 = new Vector2 (1f, 1f);
		simpleVert.color = topRightColor;
		vbo.Add (simpleVert);
		simpleVert.position = new Vector3 (vector.z, vector.y);
		//simpleVert.uv0 = new Vector2 (1f, 0f);
		simpleVert.color = bottomRightColor;
		vbo.Add (simpleVert);
	}

	public void OnPointerDown (PointerEventData data) 
	{
		HandlerPositionChanged (ref data);
	}

	public void OnDrag (PointerEventData data)
	{
		HandlerPositionChanged (ref data);
	}

	private void HandlerPositionChanged (ref PointerEventData data)
	{
		Vector2 localPos;
		RectTransformUtility.ScreenPointToLocalPointInRectangle (_rectTransform, data.position, data.pressEventCamera, out localPos);
		Rect transformRect = _rectTransform.rect;
		localPos.x = Mathf.Clamp(localPos.x, 0, transformRect.width);
		localPos.y = Mathf.Clamp(localPos.y, 0, transformRect.height);
		handle.rectTransform.localPosition = localPos;
		_handlePos.x = localPos.x / transformRect.width;
		_handlePos.y = localPos.y / transformRect.height;
		SetHandleColor ();
	}

	private void SetHandleColor ()
	{
		Color handleColor = Color.Lerp(Color.Lerp(bottomLeftColor, topLeftColor, _handlePos.y), Color.Lerp(bottomRightColor, topRightColor, _handlePos.y), _handlePos.x);
		handle.color = handleColor;
		if (OnHandleColorChanged != null) OnHandleColorChanged(handleColor);
	}
}
