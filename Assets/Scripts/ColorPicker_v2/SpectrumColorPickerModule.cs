using UnityEngine;
using System.Collections;

public class SpectrumColorPickerModule : UIModule<SpectrumColorPickerModule.SpectrumColorPickerData> 
{
	[SerializeField] private SpectrumColorPicker _spectrumColorPicker;
	[SerializeField] private GradientColorPicker _gradientColorPicker;
	[SerializeField] private ColorPickerSampler _colorPickerSampler;
	[SerializeField] private ColorPickerSampler _gradientColorPickerSampler;
	[SerializeField] private ColorPropertyDisplay _colorPropertyDisplay;

	public override void Init ()
	{
		_gradientColorPicker.SetBottomColor (new Color (1, 1, 1, 1));
		_gradientColorPicker.SetSamplePosition (new Vector2 (0.5f, 1f));

		_spectrumColorPicker.OnSampleChanged += delegate(Vector2 normalizedPosition, Color color)
		{
			_gradientColorPicker.SetTopColor(color);
			_colorPickerSampler.cacheRectTransform.localPosition = GetLocalPositionFromNormalizedPosition(_spectrumColorPicker.rectTransform, normalizedPosition);
			_colorPickerSampler.SamplerColor = color;
		};

		_gradientColorPicker.OnSampleChanged += delegate(Vector2 normalizedPosition, Color color)
		{
			normalizedPosition.x = 0.5f;
			_gradientColorPickerSampler.cacheRectTransform.localPosition = GetLocalPositionFromNormalizedPosition(_gradientColorPicker.rectTransform, normalizedPosition);
			_gradientColorPickerSampler.SamplerColor = color;
			_colorPropertyDisplay.SetColor(color);
		};

		_spectrumColorPicker.SetSamplePosition (new Vector2 (0.5f, 0.5f));
	}

	public override void Show (SpectrumColorPickerData data)
	{
		base.Show (data);
	}

	public class SpectrumColorPickerData : UIModuleData
	{
		
	}

	private Vector3 GetLocalPositionFromNormalizedPosition (RectTransform RectTransform, Vector2 normalizedPosition)
	{
		var rect = RectTransform.rect;
		return new Vector3 (rect.x + rect.width * normalizedPosition.x, rect.y + rect.height * normalizedPosition.y, 0);
	}
}
