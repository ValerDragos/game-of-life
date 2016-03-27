using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ColorPropertyDisplay : BaseMonoBehaviour 
{
	[SerializeField] private Image _image;
	[SerializeField] private InputField _redText;
	[SerializeField] private InputField _greenText;
	[SerializeField] private InputField _blueText;

	public void SetColor (Color color)
	{
		_image.color = color;
		_redText.text = string.Empty + (255 * color.r);
		_greenText.text = string.Empty + (255 * color.g);
		_blueText.text = string.Empty + (255 * color.b);
	}
}
