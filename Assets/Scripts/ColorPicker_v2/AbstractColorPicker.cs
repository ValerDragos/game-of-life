using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public abstract class AbstractColorPicker : MaskableGraphic , IPointerDownHandler, IDragHandler  
{
	public event Action<Vector2, Color> OnSampleChanged;
	public Color sampleColor { get; private set;}
	public Vector2 samplePosition { get; private set;}

	public override Texture mainTexture
	{
		get
		{
			if (!_texture == null)
			{
				return _texture;
			}
			if (this.material != null && this.material.mainTexture != null)
			{
				return this.material.mainTexture;
			}
			return Graphic.s_WhiteTexture;
		}
	}
	
	private Texture _texture;
	public Texture Texture
	{
		get
		{
			return _texture;
		}
		set
		{
			if (_texture == value)
			{
				return;
			}
			_texture = value;
			SetVerticesDirty ();
			SetMaterialDirty ();
		}
	}

	public void SetSamplePosition (Vector2 samplePosition)
	{
		samplePosition.x = Mathf.Clamp01 (samplePosition.x);
		samplePosition.y = Mathf.Clamp01 (samplePosition.y);
		this.samplePosition = samplePosition;
		RefreshColor ();
		CallOnSampleChanged ();
	}

	protected void RefreshColor ()
	{
		sampleColor = GetColorFromNormalizedLocalPosition (samplePosition);
	}

	protected void CallOnSampleChanged ()
	{
		if (OnSampleChanged != null) OnSampleChanged(samplePosition, sampleColor);
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
		if (data != null) 
		{
			Vector2 localPos;
			if (RectTransformUtility.ScreenPointToLocalPointInRectangle (rectTransform, data.position, data.pressEventCamera, out localPos))
			{
				Rect transformRect = rectTransform.rect;
				//localPos.x = Mathf.Clamp(localPos.x, transformRect.x, transformRect.x + transformRect.width);
				//localPos.y = Mathf.Clamp(localPos.y, transformRect.y, transformRect.y + transformRect.height);
				Vector2 normalizedLocalPosition = new Vector2((localPos.x - transformRect.x) / transformRect.width, (localPos.y - transformRect.y) / transformRect.height);
				SetSamplePosition(normalizedLocalPosition);
			}
		}
	}
	protected abstract Color GetColorFromNormalizedLocalPosition (Vector2 normalizedLocalPosition);
}
