using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ColorPickerSampler : BaseMonoBehaviour 
{
	[SerializeField] private Image[] _images;
	public Color SamplerColor
	{
		get { return (_images.Length > 0)? _images[0].color : Color.black; }
		set 
		{ 
			foreach (var image in _images)
			{
				image.color = value;
			}
		}
	}

	public void SetLocalPosition (Vector2 localPos)
	{
		cacheRectTransform.localPosition = new Vector3 (localPos.x, localPos.y, 0);
	}
}
