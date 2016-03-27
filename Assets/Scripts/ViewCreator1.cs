using UnityEngine; 

public class ViewCreator1 : MonoBehaviour {

	private const int TEX_SIZE = 512;

	private MeshFilter _meshFilter;
	private MeshRenderer _meshRenderer;
	private GameObject _gameObject;
	private Transform _transform;
	private Camera _camera;

	[SerializeField] private Material material = null;
	public Material ViewCreatorMaterial;

	string _aaa = "";
	//[SerializeField] private FastBloom fastBloom;

	void RenderTextureSupportCheck ()
	{
		_aaa = RenderTextureFormat.ARGB1555.ToString() + " : " + SystemInfo.SupportsRenderTextureFormat (RenderTextureFormat.ARGB1555)+"\n"+
			RenderTextureFormat.ARGB32.ToString() + " : " + SystemInfo.SupportsRenderTextureFormat (RenderTextureFormat.ARGB32)+"\n"+
			RenderTextureFormat.ARGB4444.ToString() + " : " + SystemInfo.SupportsRenderTextureFormat (RenderTextureFormat.ARGB4444)+"\n"+
			RenderTextureFormat.ARGBFloat.ToString() + " : " + SystemInfo.SupportsRenderTextureFormat (RenderTextureFormat.ARGBFloat)+"\n"+
			RenderTextureFormat.ARGBHalf.ToString() + " : " + SystemInfo.SupportsRenderTextureFormat (RenderTextureFormat.ARGBHalf)+"\n"+
			RenderTextureFormat.ARGBInt.ToString() + " : " + SystemInfo.SupportsRenderTextureFormat (RenderTextureFormat.ARGBInt)+"\n"+
			RenderTextureFormat.Default.ToString() + " : " + SystemInfo.SupportsRenderTextureFormat (RenderTextureFormat.Default)+"\n"+
			RenderTextureFormat.DefaultHDR.ToString() + " : " + SystemInfo.SupportsRenderTextureFormat (RenderTextureFormat.DefaultHDR)+"\n"+
			RenderTextureFormat.Depth.ToString() + " : " + SystemInfo.SupportsRenderTextureFormat (RenderTextureFormat.Depth)+"\n"+
			RenderTextureFormat.R8.ToString() + " : " + SystemInfo.SupportsRenderTextureFormat (RenderTextureFormat.R8)+"\n"+
			RenderTextureFormat.RFloat.ToString() + " : " + SystemInfo.SupportsRenderTextureFormat (RenderTextureFormat.RFloat)+"\n"+
			RenderTextureFormat.RGB565.ToString() + " : " + SystemInfo.SupportsRenderTextureFormat (RenderTextureFormat.RGB565)+"\n"+
			RenderTextureFormat.RGFloat.ToString() + " : " + SystemInfo.SupportsRenderTextureFormat (RenderTextureFormat.RGFloat)+"\n"+
			RenderTextureFormat.RGHalf.ToString() + " : " + SystemInfo.SupportsRenderTextureFormat (RenderTextureFormat.RGHalf)+"\n"+
			RenderTextureFormat.RGInt.ToString() + " : " + SystemInfo.SupportsRenderTextureFormat (RenderTextureFormat.RGInt)+"\n"+
			RenderTextureFormat.RHalf.ToString() + " : " + SystemInfo.SupportsRenderTextureFormat (RenderTextureFormat.RHalf)+"\n"+
			RenderTextureFormat.RInt.ToString() + " : " + SystemInfo.SupportsRenderTextureFormat (RenderTextureFormat.RInt);
	}

	void Awake () {
		Application.targetFrameRate = 30;
		Screen.sleepTimeout = SleepTimeout.NeverSleep;

		//RenderTextureSupportCheck ();
		_gameObject = gameObject;
		_transform = transform;
		_camera = GetComponent<Camera>();

		_meshFilter = _gameObject.AddComponent<MeshFilter> ();
		_meshRenderer = _gameObject.AddComponent<MeshRenderer> ();

		_meshRenderer.material = material;
		ViewCreatorMaterial = _meshRenderer.material;
	}

	private Mesh _mesh;

	Vector2 screenSizeInMeters;

	public void Render ()
	{
		_camera.Render ();
	}

	void Start ()
	{
		_mesh = new Mesh (); 

		screenSizeInMeters = new Vector2(_camera.orthographicSize * _camera.aspect, _camera.orthographicSize);
		_mesh.vertices = new Vector3[] {
			new Vector3(-screenSizeInMeters.x,-screenSizeInMeters.y,1),
			new Vector3(-screenSizeInMeters.x,screenSizeInMeters.y,1),
			new Vector3(screenSizeInMeters.x,screenSizeInMeters.y,1),
			new Vector3(screenSizeInMeters.x,-screenSizeInMeters.y,1)
		};

		_mesh.triangles = new int[] {0,1,2,0,2,3};
		Debug.Log ("SCREEN TEX_SIZE: " + Screen.width + " - " + Screen.height);
		Vector2 textureSizeForUV = new Vector2 ((float)Screen.width/TEX_SIZE, (float)Screen.height/TEX_SIZE);
		_mesh.uv = new Vector2[] {
			new Vector2 ((1f-textureSizeForUV.x)*0.5f,(1f-textureSizeForUV.y)*0.5f),
			new Vector2 ((1f-textureSizeForUV.x)*0.5f,(1f+textureSizeForUV.y)*0.5f),
			new Vector2 ((1f+textureSizeForUV.x)*0.5f,(1f+textureSizeForUV.y)*0.5f),
			new Vector2 ((1f+textureSizeForUV.x)*0.5f,(1f-textureSizeForUV.y)*0.5f)
		};

		_mesh.colors = new Color[] {new Color(0,0,0,0), new Color(0,1,0,0), new Color(1,1,0,0), new Color(1,0,0,0)};

		_mesh.RecalculateBounds ();
		_meshFilter.mesh = _mesh;

	}

	public System.Action<bool> OnScreenPress;

	// Update is called once per frame
	void Update () {
		if (Input.touchCount > 0)
		{
			if(Input.GetTouch(0).phase == TouchPhase.Began || Input.GetTouch(0).phase == TouchPhase.Ended)
			{
				if (OnScreenPress != null) OnScreenPress(Input.GetTouch(0).phase == TouchPhase.Began);
			}
			if (Input.GetTouch(0).phase == TouchPhase.Moved) 
			{
				
				// Get movement of the finger since last frame
				Vector2 delta = Input.GetTouch(0).deltaPosition * -0.02f;
				Scroll (delta);
			}
		}
		const float SPEED = 0.3f;
		if (Input.GetKey (KeyCode.UpArrow)) {
			Scroll (new Vector2(0, -SPEED)*Time.deltaTime);
		}
		if ( Input.GetKey(KeyCode.DownArrow) ) {
			Scroll (new Vector2(0, SPEED)*Time.deltaTime);
		}
		if ( Input.GetKey(KeyCode.RightArrow) ) {
			Scroll (new Vector2(-SPEED, 0)*Time.deltaTime);
		}
		if ( Input.GetKey(KeyCode.LeftArrow) ) {
			Scroll (new Vector2(SPEED, 0)*Time.deltaTime);
		}
	}
	void Scroll (Vector2 delta)
	{
		_mesh.uv = new Vector2[] {
			new Vector2 (_mesh.uv[0].x+delta.x,_mesh.uv[0].y+delta.y),
			new Vector2 (_mesh.uv[1].x+delta.x,_mesh.uv[1].y+delta.y),
			new Vector2 (_mesh.uv[2].x+delta.x,_mesh.uv[2].y+delta.y),
			new Vector2 (_mesh.uv[3].x+delta.x,_mesh.uv[3].y+delta.y)
		};
	}

	float zoomDistance = 1;

	void Zoom (bool zoomIn)
	{

		Vector2 offset = (zoomIn)? new Vector2(0.1f * _camera.aspect, 0.1f) : new Vector2(-0.1f * _camera.aspect, -0.1f);
		_mesh.uv = new Vector2[] {
			new Vector2 (_mesh.uv[0].x+offset.x,_mesh.uv[0].y+offset.y),
			new Vector2 (_mesh.uv[1].x+offset.x,_mesh.uv[1].y-offset.y),
			new Vector2 (_mesh.uv[2].x-offset.x,_mesh.uv[2].y-offset.y),
			new Vector2 (_mesh.uv[3].x-offset.x,_mesh.uv[3].y+offset.y)
		};
	}

	void OnGUI()
	{
		//GUI.TextArea(new Rect(Screen.width*0.4f, 0, Screen.width*0.2f, Screen.height*0.6f ), _aaa);

		if (GUI.Button (new Rect (0.0f, 0.0f, Screen.width * 0.3f, Screen.height * 0.2f), "ZOOM OUT")) 
		{
			Zoom (false);
		}
		if (GUI.Button (new Rect (Screen.width * 0.7f, 0.0f, Screen.width * 0.3f, Screen.height * 0.2f), "ZOOM IN")) 
		{
			Zoom (true);
		}

		if (GUI.Button (new Rect (Screen.width * 0.7f, Screen.height * 0.2f, Screen.width * 0.3f, Screen.height * 0.2f), "BLOOM +fastBloom.enabled")) 
		{
			//fastBloom.enabled = !fastBloom.enabled;
		}

	}
}
