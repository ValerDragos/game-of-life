using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RandomDrawer : MonoBehaviour {

	public enum DrawType {}

	private static readonly Vector2 FIRST_PARTICLE_VERTEX_DIRECTION = new Vector3(Mathf.Sin(240*Mathf.Deg2Rad),Mathf.Cos(240*Mathf.Deg2Rad));
	private static readonly Vector2 SECOND_PARTICLE_VERTEX_DIRECTION = new Vector3(0,1,0);
	private static readonly Vector2 THIRD_PARTICLE_VERTEX_DIRECTION = new Vector3(Mathf.Sin(120*Mathf.Deg2Rad),Mathf.Cos(120*Mathf.Deg2Rad));

	[SerializeField] private Material material = null;

	private Transform _transform;
	private GameObject _gameObject;
	private MeshFilter _meshFilter;
	private MeshRenderer _meshRenderer;
	private Mesh _mesh;

	private Vector3[] _meshVertices;
	private int[] _meshTriangles;
	private Color[] _meshColors;

	private List<RuleValue> _rulesList = new List<RuleValue>();
	public void AddRule (byte rule)
	{
		if (!DoesRuleExist(rule))
		{
			_rulesList.Add(new RuleValue(rule));
		}
	}
	public void RemoveRule (byte rule)
	{
		if (DoesRuleExist(rule))
		{
			_rulesList.Remove(_rulesList.Find(x => x.rule == rule));
		}
	}
	private int _count = 0;
	public int Count
	{
		get {return _count;}
		set
		{
			value = Mathf.Max(0,value);
			if (_count != value)
			{
				_count = value;
				CreateMeshArrays ();
			}
		}
	}
	private float _area;
	public float Area
	{
		get {return _area;}
		set
		{
			_area = Mathf.Max(0,value);
		}
	}
	private float _particleSize;
	private Vector2 _firstParticleVertexOffset;
	private Vector2 _secondParticleVertexOffset;
	private Vector2 _thirdParticleVertexOffset;
	public float ParticleSize
	{
		get {return _particleSize;}
		set
		{
			_particleSize = Mathf.Max(0,value);

			_firstParticleVertexOffset.x = FIRST_PARTICLE_VERTEX_DIRECTION.x * _particleSize;
			_firstParticleVertexOffset.y = FIRST_PARTICLE_VERTEX_DIRECTION.y * _particleSize;
			_secondParticleVertexOffset.x = SECOND_PARTICLE_VERTEX_DIRECTION.x * _particleSize;
			_secondParticleVertexOffset.y = SECOND_PARTICLE_VERTEX_DIRECTION.y * _particleSize;
			_thirdParticleVertexOffset.x = THIRD_PARTICLE_VERTEX_DIRECTION.x * _particleSize;
			_thirdParticleVertexOffset.y = THIRD_PARTICLE_VERTEX_DIRECTION.y * _particleSize;
		}
	}
	void Awake () {
		_transform = transform;
		_gameObject = gameObject;

		_meshFilter = _gameObject.GetComponent<MeshFilter>();
		if (_meshFilter==null) _meshFilter = _gameObject.AddComponent<MeshFilter>();

		_mesh = new Mesh();
		_meshFilter.sharedMesh = _mesh;

		_meshRenderer = _gameObject.GetComponent<MeshRenderer>();
		if (_meshRenderer==null) _meshRenderer = _gameObject.AddComponent<MeshRenderer>();

		_meshRenderer.sharedMaterial = material;

		_gameObject.SetActive(false);
	}

	public void Show (Vector3 localPosition)
	{
		if (_count>0)
		{
			_transform.localPosition = localPosition;
			_mesh.MarkDynamic();
			if (_count>1)
			{
				SetRandomPositionsForParticles();
				_mesh.vertices = _meshVertices;
			}
			SetRandomRulesForParticles();
			_mesh.colors = _meshColors;
			_gameObject.SetActive(true);
		}
	}

	public void Hide ()
	{
		_gameObject.SetActive(false);
	}

	private void SetRandomPositionsForParticles()
	{
		Vector2 pos;
		for (int i=0;i<_count;i++)
		{
			pos = Random.insideUnitCircle * _area;
			SetParticlePostion(ref pos, i);
		}

	}
	private void SetRandomRulesForParticles()
	{
		for (int i=0;i<_meshColors.Length;i++)
		{
			_meshColors[i].g = _rulesList[Random.Range(0,_rulesList.Count)].ruleColorFloatValue;
		}
	}

	private void CreateMeshArrays ()
	{
		int length = _count*3;
		_meshVertices = new Vector3[length];
		_meshTriangles = new int[length];
		_meshColors = new Color[length];

		for(int i = 0; i < _count; i++)
		{
			int pos = i*3;
			_meshTriangles[pos] = pos;
			pos++;
			_meshTriangles[pos] = pos;
			pos++;
			_meshTriangles[pos] = pos;
		}
		for(int i = 0; i < length; i++)
		{
			_meshColors[i].r = 1;
		}
		if (_count == 1)
		{
			var pos = Vector2.zero;
			SetParticlePostion(ref pos, 0);
		}

		_mesh.vertices = _meshVertices;
		_mesh.triangles = _meshTriangles;
	}

	private void SetParticlePostion (ref Vector2 position, int particle)
	{
		particle *= 3;
		_meshVertices[particle].x = position.x+_firstParticleVertexOffset.x;
		_meshVertices[particle].y = position.y+_firstParticleVertexOffset.y;
		particle++;
		_meshVertices[particle].x = position.x+_secondParticleVertexOffset.x;
		_meshVertices[particle].y = position.y+_secondParticleVertexOffset.y;
		particle++;
		_meshVertices[particle].x = position.x+_thirdParticleVertexOffset.x;
		_meshVertices[particle].y = position.y+_thirdParticleVertexOffset.y;
	}

	private class RuleValue
	{
		public byte rule {get; private set;}
		public float ruleColorFloatValue {get; private set;}

		public RuleValue (byte rule)
		{
			this.rule = rule;
			this.ruleColorFloatValue = rule / 255f;
		}
	}

	private bool DoesRuleExist (byte rule)
	{
		return _rulesList.Find(x => x.rule == rule) != null;
	}
}
