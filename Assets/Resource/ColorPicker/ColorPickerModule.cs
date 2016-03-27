using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.EventSystems;

public class ColorPickerModule : MonoBehaviour
{
	private const string RED_PREFIX = "<color=red>RED: </color>", GREEN_PREFIX = "<color=green>GREEN: </color>", BLUE_PREFIX = "<color=blue>BLUE: </color>";

	[SerializeField] private PickerLeftUIElement leftRawImage = null;
	[SerializeField] private PickerRightUIElement rightRawImage = null;
	[SerializeField] private Slider slider = null;
	[SerializeField] private Image sliderHandleImage = null;
	[SerializeField] private Text redText = null, greenText = null, blueText = null;
	private GameObject _gameObject;

	private Vector2 _leftChooserPosition = Vector2.one;
	
	void Awake () {
		_gameObject = gameObject;
		ApplyColorFromSlider (0f);

		Slider.SliderEvent ev = new Slider.SliderEvent();
		ev.AddListener(delegate (float factor)
		{
			ApplyColorFromSlider(factor);
		});
		slider.onValueChanged = ev;

		leftRawImage.OnHandleColorChanged += SetColorText;
	}

	public void Show ()
	{

	}

	private void ApplyColorFromSlider (float value)
	{
		Color color = rightRawImage.SampleColor(value);
		leftRawImage.TopRightColor = color;
		sliderHandleImage.color = color;
	}

	private void SetColorText (Color color)
	{
		redText.text = RED_PREFIX + (int)(color.r * 255);
		greenText.text = GREEN_PREFIX + (int)(color.g * 255);
		blueText.text = BLUE_PREFIX + (int)(color.b * 255);
	}
}
