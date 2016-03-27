using UnityEngine;
using System.Collections;
using System;

public abstract class SingletonBaseMonoBehaviour<T> : BaseMonoBehaviour where T : BaseMonoBehaviour 
{
	private static T _instance;
	private static bool _instantiated;

	public static T Instance {
		get {
			if (!_instantiated) FindOrCreateInstance ();
			return _instance;
		}
	}

	public static void Init (string gameObjectName = null)
	{
		if (!_instantiated)
		{
			FindOrCreateInstance(gameObjectName);
		}
		else
		{
			Debug.LogWarning("Init allready called!");
		}
	}

	private static void FindOrCreateInstance (string gameObjectName = null)
	{
		var typeName = typeof(T).Name;
		gameObjectName = (string.IsNullOrEmpty(gameObjectName))? typeName : gameObjectName;

		// Find object inside scene.
		var objects = FindObjectsOfType<T>();
		if (objects.Length > 0) 
		{
			_instance = objects[0];
			_instance.cacheTransform.parent = null;
			//_instance.cacheGameObject.name = gameObjectName;
			DontDestroyOnLoad(_instance.cacheGameObject);
			if (objects.Length > 1) 
			{
				Debug.LogWarning("There is more than one instance of Singleton of type \"" + typeName + "\". Keeping the first. Destroying the others.");
				for (var i = 1; i < objects.Length; i++) DestroyImmediate(objects[i].gameObject);
			}
		}
		else
		{
			// Create new instance
			GameObject go = new GameObject(gameObjectName);
			_instance = go.AddComponent<T>();

			DontDestroyOnLoad(go);
		}
		_instantiated = true;
	}

	protected virtual void Awake () 
	{
		if (_instantiated && _instance != this)
		{
			Destroy (this);
			Debug.LogError ("Instance of \"" + typeof(T) + "\" allready exists!.");
		}
	}

	protected virtual void OnDestroy() 
	{ 
		if (_instance == this) 
		{
			_instance = null;
			_instantiated = false; 
		}
	}
}
