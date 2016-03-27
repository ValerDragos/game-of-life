using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class PickerRightUIElement : MaskableGraphic 
{
	[SerializeField] private List<Color> _colors = null;

	protected override void OnPopulateMesh (Mesh m)
	{
		if (_colors == null || _colors.Count < 2)
		{
			this.color = (_colors.Count == 1)? _colors[0] : this.color;
			base.OnPopulateMesh(m);
		}
		else
		{
			//int size = _colors.Count-1;
			//Rect pixelAdjustedRect = this.GetPixelAdjustedRect ();
			//float step = pixelAdjustedRect.height / size;
			//
			//UIVertex simpleVert = UIVertex.simpleVert;
			//Vector4 vector = new Vector4 (pixelAdjustedRect.x, pixelAdjustedRect.y, pixelAdjustedRect.x + pixelAdjustedRect.width, pixelAdjustedRect.y + step);
			//DrawSmallRect(ref m, ref simpleVert, ref vector, _colors[0], _colors[1]);
			//
			//for (int i=1;i<size;i++)
			//{
			//	vector.y = vector.w;
			//	vector.w += step;
			//	DrawSmallRect(ref vbo, ref simpleVert, ref vector, _colors[i], _colors[i+1]);
			//}
		}
	}

	private void DrawSmallRect (ref List<UIVertex> vbo, ref UIVertex simpleVert, ref Vector4 vector, Color bootmColor, Color topColor)
	{
		simpleVert.color = bootmColor;
		simpleVert.position = new Vector3 (vector.x, vector.y);
		//simpleVert.uv0 = new Vector2 (0f, 0f);
		vbo.Add (simpleVert);
		simpleVert.color = topColor;
		simpleVert.position = new Vector3 (vector.x, vector.w);
		//simpleVert.uv0 = new Vector2 (0f, 1f);
		vbo.Add (simpleVert);
		simpleVert.color = topColor;
		simpleVert.position = new Vector3 (vector.z, vector.w);
		//simpleVert.uv0 = new Vector2 (1f, 1f);
		vbo.Add (simpleVert);
		simpleVert.color = bootmColor;
		simpleVert.position = new Vector3 (vector.z, vector.y);
		//simpleVert.uv0 = new Vector2 (1f, 0f);
		vbo.Add (simpleVert);
	}
	
	public Color SampleColor (float factor)
	{
		if (_colors == null || _colors.Count == 0)
		{
			return color;
		}
		else if (_colors.Count == 1)
		{
			return _colors[0];
		}
		factor = Mathf.Clamp01(factor);

		float colorsFactor = (_colors.Count-1)*factor;
		int pos = (int)colorsFactor;
		int nextPos = (pos+1 == _colors.Count)? pos : pos+1;

		return Color.Lerp(_colors[pos], _colors[nextPos], colorsFactor-pos);
	}
}
