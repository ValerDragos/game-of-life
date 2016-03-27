using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GeometryCreator
{
	public Vector3[] Vertices { get; private set; }
	public int[] Triangles { get; private set; }
	public Vector2[] Uvs { get; private set; }
	public Color[] Colors { get; private set; }

	public int VerticesCount { get{ return (Vertices == null)? 0 : Vertices.Length; } }
	public int TrianglesCount { get{ return (Triangles == null)? 0 : Triangles.Length / 3; } }
	public int UvsCount { get{ return (Uvs == null)? 0 : Uvs.Length; } }
	public int ColorsCount { get{ return (Colors == null)? 0 : Colors.Length; } }

	protected void CreateVerticesArray (int count, bool copyFromOld = false)
	{
		count = Mathf.Max (0, count);
		if (Vertices == null) 
		{
			Vertices = new Vector3[count];
		}
		else if (Vertices.Length != count)
		{
			if (copyFromOld)
			{
				var newVertices = new Vector3[count];
				int max = Mathf.Min(count, Vertices.Length);
				for (int i = 0; i < max; i++)
				{
					newVertices[i].x = Vertices[i].x;
					newVertices[i].y = Vertices[i].y;
					newVertices[i].z = Vertices[i].z;
				}
				Vertices = newVertices;
			}
			else
			{
				Vertices = new Vector3[count];
			}
		}
	}
	protected void CreateTrianglesArray (int count, bool copyFromOld = false)
	{
		count = Mathf.Max (0, count);
		count *= 3;
		if (Triangles == null) 
		{
			Triangles = new int[count];
		}
		else if (Triangles.Length != count)
		{
			if (copyFromOld)
			{
				var newTriangles = new int[count];
				int max = Mathf.Min(count, Triangles.Length);
				for (int i = 0; i < max; i++)
				{
					newTriangles[i] = Triangles[i];
				}
				Triangles = newTriangles;
			}
			else
			{
				Triangles = new int[count];
			}
		}
	}
	protected void CreateUvsArray (bool copyFromOld = false)
	{
		if (Vertices != null) 
		{
			int count = Vertices.Length;
			if (Uvs == null)
			{
				Uvs = new Vector2[count];
			}
			else if (Uvs.Length != count)
			{
				if (copyFromOld)
				{
					var newUvs = new Vector2[count];
					int max = Mathf.Min(count, Uvs.Length);
					for (int i = 0; i < max; i++)
					{
						newUvs[i].x = Uvs[i].x;
						newUvs[i].y = Uvs[i].y;
					}
					Uvs = newUvs;
				}
				else
				{
					Uvs = new Vector2[count];
				}
			}
		}
	}

	protected void CreateColorsArray (params Color[] newColors)
	{
		if (Vertices != null) 
		{
			if (Colors == null || Colors.Length != Vertices.Length)
			{
				if (newColors.Length != Vertices.Length)
				{
					Colors = new Color[Vertices.Length];
					CopyToColorsArray(ref newColors);
				}
				else
				{
					Colors = newColors;
				}
			}
			else if (Colors.Length != newColors.Length)
			{
				CopyToColorsArray(ref newColors);
			}
			else
			{
				Colors = newColors;
			}
		}
	}

	protected void CreateColorsArray (bool copyFromOld = false)
	{
		if (Vertices != null) 
		{
			int count = Vertices.Length;
			if (Colors == null)
			{
				Colors = new Color[count];
			}
			else if (Colors.Length != count)
			{
				if (copyFromOld)
				{
					var newColors = new Color[count];
					CopyFromColorsArray(ref newColors);
					Colors = newColors;
				}
				else
				{
					Colors = new Color[count];
				}
			}
		}
	}
	private void CopyToColorsArray (ref Color[] from)
	{
		int max = Mathf.Min(from.Length, Colors.Length);
		for (int i = 0; i < max; i++)
		{
			Colors[i].r = from[i].r;
			Colors[i].g = from[i].g;
			Colors[i].b = from[i].b;
			Colors[i].a = from[i].a;
		}
	}
	private void CopyFromColorsArray (ref Color[] to)
	{
		int max = Mathf.Min(Colors.Length, to.Length);
		for (int i = 0; i < max; i++)
		{
			to[i].r = Colors[i].r;
			to[i].g = Colors[i].g;
			to[i].b = Colors[i].b;
			to[i].a = Colors[i].a;		
		}
	}

	private delegate Color GetColorDelegate (int index);
	private delegate Vector2 GetUvDelegate (int index);

	public void SetVerticesWithColorsAndUvs (VertexHelper vh, Color color)
	{
		if (Vertices != null) 
		{
			GetColorDelegate getColor = CreateGetColorDelegate();
			GetUvDelegate getUv = CreateGetUvDelegate();
			for (int i = 0; i < Vertices.Length; i++)
			{
				vh.AddVert(Vertices[i], getColor(i) * color, getUv(i));
			}
		}

	}
	
	public void SetTriangles (VertexHelper vh, int positionOffset = 0)
	{
		for (int i = 0; i < TrianglesCount; i++)
		{
			int index = i*3;
			vh.AddTriangle(Triangles[index] + positionOffset, 
			               Triangles[index + 1] + positionOffset, 
			               Triangles[index + 2] + positionOffset);
		}
	}

	private GetColorDelegate CreateGetColorDelegate ()
	{
		GetColorDelegate getColor;
		if (Colors == null) getColor = delegate { return Color.clear; };
		else getColor = delegate (int index)
		{ 
			return (index < Colors.Length)? Colors[index] : Color.clear;
		};
		return getColor;
	}
	private GetUvDelegate CreateGetUvDelegate ()
	{
		GetUvDelegate getUv;
		if (Uvs == null) getUv = delegate { return Vector2.zero; };
		else getUv = delegate (int index)
		{ 
			return (index < Uvs.Length)? Uvs[index] : Vector2.zero;
		};
		return getUv;
	}
}

public class LineCreator : GeometryCreator
{

}

public class PlaneGridCreator : GeometryCreator
{
	public int Rows { get; private set;}
	public int Columns { get; private set;}

	public bool WasCalculatedOnce { get; private set;}

	public PlaneGridCreator (int rows, int columns, params Color[] colors)
	{
		SetRowsAndColumns (rows, columns);
		CreateColorsArray (colors);
	}

	public PlaneGridCreator (int rows, int columns)
	{
		SetRowsAndColumns (rows, columns);
	}

	public void SetRowsAndColumns (int rows, int columns)
	{
		Rows = Mathf.Max (1, rows);
		Columns = Mathf.Max (1, columns);

		CreateVerticesArray ((Rows + 1) * (Columns + 1));
		CreateTrianglesArray (Rows * Columns * 2);
		CreateUvsArray ();
		WasCalculatedOnce = false;
	}

	public void CalculateBasedOnRect (Rect rect)
	{
	// Refresh verts and uvs
		Vector2 vertsStep = new Vector2 (rect.width / Columns, rect.height / Rows);
		Vector2 uvsStep = new Vector2 (1f / Columns, 1f / Rows);
		for (int row=0; row <= Rows; row++)
		{
			int indexAdd = row * (Columns+1);
			for (int column=0; column <= Columns; column++)
			{
				int index = column+indexAdd;
				Vertices[index].x = rect.x + vertsStep.x * column;
				Vertices[index].y = rect.y + vertsStep.y * row;

				Uvs[index].x = uvsStep.x * column;
				Uvs[index].y = uvsStep.y * row;
			}
		}
	// Create triangles
		int arrayIndex = 0;
		for (int row=0; row < Rows; row++)
		{
			int vertexAddIndex = row * (Columns+1);
			int vertexTopAddIndex = (row+1) * (Columns+1); 

			for (int column=0; column < Columns; column++)
			{
				int vertexCurrentIndex = column+vertexAddIndex;
				int vertexTopIndex = column+vertexTopAddIndex;

				Triangles[arrayIndex] = vertexCurrentIndex;
				arrayIndex++;
				Triangles[arrayIndex] = vertexTopIndex;
				arrayIndex++;
				Triangles[arrayIndex] = vertexTopIndex+1;
				arrayIndex++;

				Triangles[arrayIndex] = vertexTopIndex+1;
				arrayIndex++;
				Triangles[arrayIndex] = vertexCurrentIndex+1;
				arrayIndex++;
				Triangles[arrayIndex] = vertexCurrentIndex;
				arrayIndex++;
			}
		}
		WasCalculatedOnce = true;
	}
}
