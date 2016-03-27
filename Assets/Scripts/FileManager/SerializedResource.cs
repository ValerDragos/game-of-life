using UnityEngine;
using System.Collections;

namespace FileManager
{
	[System.Serializable]
	public class SerializedResource
	{
		[SerializeField] private Object _resource;
		public Object resource { get { return _resource; } }

		public T CreateInstanceAndGetComponent<T> () where T : Component
		{
			var instance =  GetInstance ();
			if (instance != null)
			{
				var component = instance.GetComponent<T>();
				if (component == null)
				{
					Debug.Log("Component \"" + typeof(T).Name + "\" does not exist on: " + instance.name);
					Object.Destroy(instance);
				}
				return component;
			}
			else
			{
				Debug.Log("Unable to create instance!");
				return null;
			}
		}

		public GameObject GetInstance ()
		{
			return GameObject.Instantiate (_resource) as GameObject;
		}
	}
}
