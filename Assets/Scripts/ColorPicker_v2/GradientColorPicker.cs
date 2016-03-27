using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class GradientColorPicker : AbstractColorPicker 
{
	private PlaneGridCreator _backgroundPlaneCreator = new PlaneGridCreator (1, 1, 
	                                                                         new Color(0,0,0,1), 
	                                                                         new Color(0,0,0,1), 
	                                                                         new Color(0,0,0,1), 
	                                                                         new Color(0,0,0,1));

	public void SetTopColor (Color color)
	{
		_backgroundPlaneCreator.Colors [2] = color;
		_backgroundPlaneCreator.Colors [3] = color;
		SetVerticesDirty ();
		RefreshColor ();
		CallOnSampleChanged ();
	}

	public void SetBottomColor (Color color)
	{
		_backgroundPlaneCreator.Colors [0] = color;
		_backgroundPlaneCreator.Colors [1] = color;
		SetVerticesDirty ();
		RefreshColor ();
		CallOnSampleChanged ();
	}

	private void CalculateVertices ()
	{
		Rect pixelAdjustedRect = this.GetPixelAdjustedRect ();
		_backgroundPlaneCreator.CalculateBasedOnRect (pixelAdjustedRect);
	}

	protected override void OnRectTransformDimensionsChange ()
	{
		CalculateVertices ();
		base.OnRectTransformDimensionsChange ();
	}

	protected override void OnPopulateMesh (VertexHelper vh)
	{
		vh.Clear ();
		if (!_backgroundPlaneCreator.WasCalculatedOnce) CalculateVertices();
		_backgroundPlaneCreator.SetVerticesWithColorsAndUvs (vh, color);
		_backgroundPlaneCreator.SetTriangles (vh);
	}

	protected override Color GetColorFromNormalizedLocalPosition (Vector2 normalizedLocalPosition)
	{
		return Color.Lerp (_backgroundPlaneCreator.Colors [0], _backgroundPlaneCreator.Colors [2], normalizedLocalPosition.y);
	}
}
