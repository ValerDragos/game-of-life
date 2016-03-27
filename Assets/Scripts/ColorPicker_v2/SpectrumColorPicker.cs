using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SpectrumColorPicker : AbstractColorPicker
{
	private PlaneGridCreator _backgroundPlaneCreator = new PlaneGridCreator (1, 6,
		new Color(1,0,0,1), new Color(1,1,0,1), new Color(0,1,0,1), new Color(0,1,1,1), new Color(0,0,1,1), new Color(1,0,1,1), new Color(1,0,0,1),
	    new Color(1,0,0,1), new Color(1,1,0,1), new Color(0,1,0,1), new Color(0,1,1,1), new Color(0,0,1,1), new Color(1,0,1,1), new Color(1,0,0,1));
	private PlaneGridCreator _foregroundPlaneCreator = new PlaneGridCreator (1, 1,
	    new Color(0,0,0,1), new Color(0,0,0,1),
	    new Color(0,0,0,0), new Color(0,0,0,0));

	private void CalculateVertices ()
	{
		Rect pixelAdjustedRect = this.GetPixelAdjustedRect ();
		_backgroundPlaneCreator.CalculateBasedOnRect (pixelAdjustedRect);
		_foregroundPlaneCreator.CalculateBasedOnRect (pixelAdjustedRect);
	}

	protected override void OnRectTransformDimensionsChange ()
	{
		CalculateVertices ();
		base.OnRectTransformDimensionsChange ();
	}

	protected override void OnPopulateMesh (VertexHelper vh)
	{
		vh.Clear ();
		if (!_backgroundPlaneCreator.WasCalculatedOnce || !_foregroundPlaneCreator.WasCalculatedOnce) CalculateVertices();

		_backgroundPlaneCreator.SetVerticesWithColorsAndUvs (vh, color);
		_foregroundPlaneCreator.SetVerticesWithColorsAndUvs (vh, color);

		_backgroundPlaneCreator.SetTriangles (vh);
		_foregroundPlaneCreator.SetTriangles (vh, _backgroundPlaneCreator.VerticesCount);
	}

	protected override Color GetColorFromNormalizedLocalPosition (Vector2 normalizedLocalPosition)
	{
		float a = normalizedLocalPosition.x * 6;
		int b = (int)a;
		float c = a - b;

		Color horizontalLerp = Color.Lerp (_backgroundPlaneCreator.Colors [b], _backgroundPlaneCreator.Colors [b + 1], c);
		return Color.Lerp (_foregroundPlaneCreator.Colors [0], horizontalLerp, normalizedLocalPosition.y);
	}
}
