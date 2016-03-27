using UnityEngine;
using System.Collections;

public abstract class BaseMonoBehaviour : MonoBehaviour 
{
	public enum ComponentAddProperty
	{
		Added = 0,
		FoundAndCached = 1
	}

	private GameObject _cacheGameObject;
	private bool _gameObjectCached;
	public GameObject cacheGameObject
	{
		get 
		{
			if (!_gameObjectCached)
			{
				_cacheGameObject = gameObject;
				_gameObjectCached = true;
			}
			return _cacheGameObject;
		}
	}

	private Transform _cacheTransform;
	private bool _transformCached;
	public Transform cacheTransform
	{
		get 
		{
			if (!_transformCached)
			{
				_cacheTransform = transform;
				_transformCached = true;
			}
			return _cacheTransform;
		}
	}

	private RectTransform _cacheRectTransform;
	private bool _rectTransformCached;
	public RectTransform cacheRectTransform
	{
		get 
		{
			if (!_rectTransformCached)
			{
				_cacheRectTransform = (RectTransform) transform;
				_rectTransformCached = true;
			}
			return _cacheRectTransform;
		}
	}

	private Camera _cacheCamera;
	private bool _cameraCached;
	public Camera cacheCamera
	{
		get 
		{
			if (!_cameraCached)
			{
				CachComponent<Camera> (ref _cacheCamera, ref _cameraCached);
			}
			return _cacheCamera;
		}
	}
	public new Camera camera
	{
		get
		{
			CachComponent<Camera> (ref _cacheCamera, ref _cameraCached);
			return _cacheCamera;
		}
		private set 
		{
			_cacheCamera = value;
			_cameraCached = true;
		}
	}

	private MeshFilter _cacheMeshFilter;
	private bool _meshFilterCached;
	public MeshFilter cacheMeshFilter
	{
		get 
		{
			if (!_meshFilterCached)
			{
				CachComponent<MeshFilter> (ref _cacheMeshFilter, ref _meshFilterCached);
			}
			return _cacheMeshFilter;
		}
	}
	public MeshFilter meshFilter
	{
		get
		{
			CachComponent<MeshFilter> (ref _cacheMeshFilter, ref _meshFilterCached);
			return _cacheMeshFilter;
		}
		private set 
		{
			_cacheMeshFilter = value;
			_meshFilterCached = true;
		}
	}

	private MeshRenderer _cacheMeshRenderer;
	private bool _meshRendererCached;
	public MeshRenderer cacheMeshRenderer
	{
		get 
		{
			if (!_meshRendererCached)
			{
				CachComponent<MeshRenderer> (ref _cacheMeshRenderer, ref _meshRendererCached);
			}
			return _cacheMeshRenderer;
		}
	}
	public MeshRenderer meshRenderer
	{
		get
		{
			CachComponent<MeshRenderer> (ref _cacheMeshRenderer, ref _meshRendererCached);
			return _cacheMeshRenderer;
		}
	}
	private ComponentAddProperty CacheOrCreate<T> (ref T cachedVariable, ref bool cachedFlag) where T : Component
	{
		CachComponent<T> (ref cachedVariable, ref cachedFlag);
		if (cachedVariable == null)
		{
			cachedVariable = cacheGameObject.AddComponent<T>();
			return ComponentAddProperty.Added;
		}
		return ComponentAddProperty.FoundAndCached;
	}

	private void CachComponent<T> (ref T cacheVariable, ref bool cachedFlag) where T : Component
	{
		cacheVariable = cacheGameObject.GetComponent<T> ();
		cachedFlag = true;
	}

	protected ComponentAddProperty CacheOrCreateCamera () { return CacheOrCreate<Camera> (ref _cacheCamera, ref _cameraCached); }
	protected ComponentAddProperty CacheOrCreateMeshFilter () { return CacheOrCreate<MeshFilter> (ref _cacheMeshFilter, ref _meshFilterCached); }
	protected ComponentAddProperty CacheOrCreateMeshRenderer () { return CacheOrCreate<MeshRenderer> (ref _cacheMeshRenderer, ref _meshRendererCached); }

	virtual protected string LOG_TITLE {get { return "BaseMonoBehaviour"; } }
	protected bool LOGS_ENABLED = false;

	protected void Log (string message)
	{
		if (LOGS_ENABLED) Debug.Log(LOG_TITLE+": "+message);
	}
	protected void LogWarning (string message)
	{
		if (LOGS_ENABLED) Debug.LogWarning(LOG_TITLE+": "+message);
	}
	protected void LogError (string message)
	{
		if (LOGS_ENABLED) Debug.LogError(LOG_TITLE+": "+message);
	}
}
