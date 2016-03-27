using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;
using GOL;

public class Screen1 : MonoBehaviour {

	const int SIZE = 512;

	[SerializeField] private Button startButton;
	[SerializeField] private Button startBlankButton;

	[SerializeField] private GolDisplay golDisplay;

	[SerializeField] private Texture2D _startTexture;
	[SerializeField] private Processor _golProcessor;
	
	void Start () {
		Screen.sleepTimeout = SleepTimeout.NeverSleep;
		Application.targetFrameRate = -1;

		//_golProcessor.OnRenderTextureChanged += delegate (RenderTexture rt)
		//{
		//	if (PostProcessorImage.Instance != null) PostProcessorImage.Instance.texture = rt;
		//	else golDisplay.texture = rt;
		//};
		startButton.onClick = new Button.ButtonClickedEvent();
		startButton.onClick.AddListener(delegate
		{
			if (PostProcessorImage.Instance != null) {
				golDisplay.texture = PostProcessorImage.Instance.renderTexture;
				PostProcessorImage.Instance.Play();
			}

			Texture2D tex = CreateTex ();
			golDisplay.Resize(new Vector2(tex.width, tex.height));
			//_golProcessor.Play(tex);

			//#if UNITY_EDITOR
			//UnityEditor.EditorApplication.isPaused = true;
			//#endif
		});

		startBlankButton.onClick = new Button.ButtonClickedEvent();
		startBlankButton.onClick.AddListener(delegate
		{
			if (PostProcessorImage.Instance != null) {
				golDisplay.texture = PostProcessorImage.Instance.renderTexture;
				PostProcessorImage.Instance.Play();
			}

			var tex = CreateBlankTex();
			golDisplay.Resize(new Vector2(tex.width, tex.height));
			//_golProcessor.Play(tex);
		});
		// If post processing

	}
	
	// Update is called once per frame
	void Update () {
	
	}

	private Texture2D CreateBlankTex ()
	{
		if (_startTexture == null)
		{
			Texture2D tex = new Texture2D(SIZE, SIZE, TextureFormat.ARGB32, false);
			for (int i=0;i<tex.width;i++)
			{
				for (int j=0;j<tex.height; j++)
				{
					tex.SetPixel(i,j,Color.black);
				}
			}
			tex.Apply();
			return tex;
		}
		return _startTexture;
	}

	private Texture2D CreateTex ()
	{
		if (_startTexture == null)
		{
			Texture2D tex = new Texture2D(SIZE, SIZE, TextureFormat.ARGB32, false);
			for (int i=0;i<tex.width;i++)
			{
				for (int j=0;j<tex.height; j++)
				{
					
					//tex.SetPixel(i,j,new Color32(
					//	BitConverter.GetBytes(UnityEngine.Random.Range(0,2) * 255)[0],
					//	BitConverter.GetBytes(UnityEngine.Random.Range(0,2))[0],
					//	BitConverter.GetBytes(255)[0],
					//	BitConverter.GetBytes(255)[0]));
					
					tex.SetPixel(i,j,new Color(
						(int)UnityEngine.Random.Range(0,2),
						UnityEngine.Random.Range(0,3)/256f,
						0,
						0));
				}
			}
			tex.Apply();
			
			return tex;
		}
		return _startTexture;
	}
}
