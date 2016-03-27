using UnityEngine;
using System.Collections;

public class BloomScript : MonoBehaviour 
{

	public enum Resolution {
		Low = 0,
		High = 1,
	}
	
	public enum BlurType {
		Standard = 0,
		Sgx = 1,
	}

	[SerializeField] private Material material = null;

	[Range(0.0f, 1.5f)]
	public float threshhold = 0.25f;
	[Range(0.0f, 2.5f)]
	public float intensity  = 0.75f;
	
	[Range(0.25f, 5.5f)]
	public float blurSize = 1.0f;
	
	Resolution resolution = Resolution.Low;
	[Range(1, 4)]
	public int blurIterations = 1;
	
	public BlurType blurType = BlurType.Standard;
	
	void Awake () {
		var divider = resolution == Resolution.Low ? 4 : 2;
		var widthMod = resolution == Resolution.Low ? 0.5f : 1.0f;
		
		material.SetVector ("_Parameter", new Vector4 (blurSize * widthMod, 0.0f, threshhold, intensity));
	}

	/*
	function OnRenderImage (source : RenderTexture, destination : RenderTexture) {	

		

		source.filterMode = FilterMode.Bilinear;
		
		var rtW = source.width/divider;
		var rtH = source.height/divider;
		
		// downsample
		var rt : RenderTexture = RenderTexture.GetTemporary (rtW, rtH, 0, source.format);
		rt.filterMode = FilterMode.Bilinear;
		Graphics.Blit (source, rt, fastBloomMaterial, 1);
		
		var passOffs = blurType == BlurType.Standard ? 0 : 2;
		
		for(var i : int = 0; i < blurIterations; i++) {
			fastBloomMaterial.SetVector ("_Parameter", Vector4 (blurSize * widthMod + (i*1.0f), 0.0f, threshhold, intensity));
			
			// vertical blur
			var rt2 : RenderTexture = RenderTexture.GetTemporary (rtW, rtH, 0, source.format);
			rt2.filterMode = FilterMode.Bilinear;
			Graphics.Blit (rt, rt2, fastBloomMaterial, 2 + passOffs);
			RenderTexture.ReleaseTemporary (rt);
			rt = rt2;
			
			// horizontal blur
			rt2 = RenderTexture.GetTemporary (rtW, rtH, 0, source.format);
			rt2.filterMode = FilterMode.Bilinear;
			Graphics.Blit (rt, rt2, fastBloomMaterial, 3 + passOffs);
			RenderTexture.ReleaseTemporary (rt);
			rt = rt2;
		}
		
		fastBloomMaterial.SetTexture ("_Bloom", rt);
		
		Graphics.Blit (source, destination, fastBloomMaterial, 0);
		
		RenderTexture.ReleaseTemporary (rt);
	}	
	*/
}
