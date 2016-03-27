using UnityEngine; 

public class ViewCreator : MonoBehaviour {

	private const int TEX_SIZE = 1024;

	private MeshFilter _meshFilter;
	private MeshRenderer _meshRenderer;
	private GameObject _scrolableGameObject;
	private Transform _scrolableGameObjectTransform;
	private Camera _camera;

	[SerializeField] private Material material = null;
	public Material ViewCreatorMaterial;

	private Transform _transform;

	string _aaa = "";

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
		Application.targetFrameRate = 60;

		RenderTextureSupportCheck ();
		_transform = transform;
		_camera = GetComponent<Camera>();
		_scrolableGameObject = new GameObject ("Grid");
		_scrolableGameObjectTransform = _scrolableGameObject.transform;
		_scrolableGameObjectTransform.parent = transform;
		_scrolableGameObjectTransform.rotation = Quaternion.Inverse (transform.rotation);
		_scrolableGameObjectTransform.localPosition = new Vector3 (0, 0, 1);
		_scrolableGameObjectTransform.Rotate (new Vector3 (0, 0, 180));

		_meshFilter = _scrolableGameObject.AddComponent<MeshFilter> ();
		_meshRenderer = _scrolableGameObject.AddComponent<MeshRenderer> ();

		_meshRenderer.material = material;
		ViewCreatorMaterial = _meshRenderer.material;
	}

	void Start ()
	{
		float size = ((TEX_SIZE * _camera.orthographicSize * 2.0f) / Screen.height)/2.0f;
		Mesh mesh = new Mesh (); 
		mesh.vertices = new Vector3[] {new Vector3(-size,size,0),new Vector3(size,size,0),new Vector3(size,-size,0),new Vector3(-size,-size,0)};
		mesh.triangles = new int[] {0,1,3,3,1,2};
		mesh.uv = new Vector2[] {new Vector2 (0,1),new Vector2 (1,1),new Vector2 (1,0),new Vector2 (0,0)};
		mesh.RecalculateBounds ();
		_meshFilter.mesh = mesh;

	}

	// Update is called once per frame
	void Update () {
		if (Input.touchCount > 0 && 
		    Input.GetTouch(0).phase == TouchPhase.Moved) {
			
			// Get movement of the finger since last frame
			Vector2 delta = Input.GetTouch(0).deltaPosition;
			
			_scrolableGameObjectTransform.localPosition += new Vector3((delta.x/Screen.width) * 20,(delta.y/Screen.height) * 20,0);
		}
	}

	void OnGUI()
	{
		GUI.TextArea(new Rect(Screen.width*0.4f, 0, Screen.width*0.2f, Screen.height*0.6f ), _aaa);

		if (GUI.Button (new Rect (0.0f, 0.0f, Screen.width * 0.3f, Screen.height * 0.2f), "ZOOM OUT")) 
		{
			if (_scrolableGameObjectTransform.localScale.x+_scrolableGameObjectTransform.localScale.y+_scrolableGameObjectTransform.localScale.z > 3)
				_scrolableGameObjectTransform.localScale -= new Vector3(1,1,1);
		}
		if (GUI.Button (new Rect (Screen.width * 0.7f, 0.0f, Screen.width * 0.3f, Screen.height * 0.2f), "ZOOM IN")) 
		{
			_scrolableGameObjectTransform.localScale += new Vector3(1,1,1);
		}
	}
}
