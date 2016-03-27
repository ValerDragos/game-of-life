using UnityEngine;
using System.Collections;
using GOL;

[RequireComponent(typeof(Camera))]
public class RandomDrawerController : MonoBehaviour {

	[SerializeField] private RandomDrawer randomDrawer = null;
	[SerializeField] private Processor golProcessor = null;
	[SerializeField] private float zPos = 0;

	const float SIZE = 0.5f;
	const int COUNT = 20;
	
	private Camera _camera;
	
	void Awake () {
		_camera = GetComponent<Camera>();
		randomDrawer.ParticleSize = (_camera.orthographicSize*2)/512f;
		randomDrawer.Area = SIZE;
		randomDrawer.Count = COUNT;

		randomDrawer.AddRule(1);
		randomDrawer.AddRule(2);
		randomDrawer.AddRule(3);
		randomDrawer.AddRule(4);
		randomDrawer.AddRule(5);
		randomDrawer.AddRule(6);
	}

	void Start ()
	{
		GolDisplay.Instance.OnTouch += SetViewPortPosition;
	}

	public void SetViewPortPosition (Vector2 position)
	{
		float halfHeight = _camera.orthographicSize;
		float halfWidth = halfHeight * _camera.aspect;
		//golProcessor.OnBeforeDraw += delegate 
		//{
		//	randomDrawer.Show(new Vector3(position.x * halfWidth, position.y * halfHeight, zPos));
		//};
		//golProcessor.OnAfterDraw += randomDrawer.Hide;
	}
}
