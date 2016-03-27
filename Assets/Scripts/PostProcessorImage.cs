using UnityEngine;
using UnityEngine.UI;
using System.Collections;

[RequireComponent(typeof(Camera))]
public class PostProcessorImage : MonoBehaviour 
{
	private Camera _camera;
	private RenderTexture _renderTexture;
	[SerializeField] private RawImage rawImage;

	public static PostProcessorImage Instance;
	public RenderTexture renderTexture 
	{
		get
		{
			if (_renderTexture == null) CreateRenderTexture();
			return _renderTexture;
		}
	}
	public Texture texture
	{
		set
		{
			rawImage.texture = value;
		}
	}

	void Awake () {
		if (Instance != null)
		{
			Destroy(this);
			return;
		}
		Instance = this;

		_camera = GetComponent<Camera>();
		_camera.enabled = false;

	}

	const string COROUTINE_NAME = "OnUpdate";
	public void Play ()
	{
		StopCoroutine(COROUTINE_NAME);
		StartCoroutine(COROUTINE_NAME);
	}

	private void CreateRenderTexture ()
	{
		_renderTexture = new RenderTexture(Screen.width, Screen.height, 0, RenderTextureFormat.ARGB32);

		_renderTexture.anisoLevel = 0;
		_renderTexture.antiAliasing = 1;
		_renderTexture.filterMode = FilterMode.Bilinear;
		_renderTexture.wrapMode = TextureWrapMode.Clamp;
		_renderTexture.generateMips = false;
		_renderTexture.isPowerOfTwo = false;
		_renderTexture.useMipMap = false;
		_renderTexture.isVolume = false;
		_renderTexture.isCubemap = false;

		_camera.targetTexture = _renderTexture;
	}

	private IEnumerator OnUpdate ()
	{
		while (true)
		{
			yield return new WaitForEndOfFrame();
			_camera.Render();
		}
		
	}
}
