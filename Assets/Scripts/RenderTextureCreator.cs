using UnityEngine;
using System.Collections;

public abstract class AbstractTextureContainer<T> where T : Texture
{	
	public T texture { get; protected set; }
}

public class Texture2DContainer : AbstractTextureContainer<Texture2D>
{
	private float _allivePercentage;

	public Texture2DContainer (int width, int height, float allivePercentage)
	{
		texture = new Texture2D (width, height, TextureFormat.ARGB32, false);
		texture.anisoLevel = 1;
		texture.filterMode = FilterMode.Point;
		texture.wrapMode = TextureWrapMode.Clamp;

		_allivePercentage = Mathf.Clamp01 (allivePercentage);
	}

	public void PopulateWithRandomPixels (int yMax)
	{
		yMax = Mathf.Clamp (yMax, 0, 255) + 1;
		Color color = Color.clear;
		for (int i = 0; i < texture.width; i++)
		{
			for (int j = 0; j < texture.height; j++)
			{
				float percentage = Random.Range(0f, 1f);
				if (percentage < _allivePercentage)
				{
					color.r = 1;
					color.g = Random.Range(0, yMax) / 255f;
				}
				else
				{
					color.r = 0;
				}
				texture.SetPixel(i, j, color);
			}
		}
		texture.Apply ();
	}
}

public class RenderTextureContainer : AbstractTextureContainer<RenderTexture>
{
	public RenderTextureContainer (int width, int height)
	{
		if (SystemInfo.supportsRenderTextures && SystemInfo.SupportsRenderTextureFormat (RenderTextureFormat.ARGB32))
		{
			texture = new RenderTexture (width, height, 0, RenderTextureFormat.ARGB32);
			texture.anisoLevel = 0;
			texture.antiAliasing = 1;
			texture.filterMode = FilterMode.Point;
			texture.wrapMode = TextureWrapMode.Clamp;
			texture.generateMips = false;
			texture.isPowerOfTwo = Mathf.IsPowerOfTwo(width) && Mathf.IsPowerOfTwo(height);
			texture.useMipMap = false;
			texture.isVolume = false;
			texture.isCubemap = false;
		}
	}
}
