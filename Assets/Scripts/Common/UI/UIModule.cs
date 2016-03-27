using UnityEngine;
using System.Collections;

public abstract class UIModule<T> : BaseMonoBehaviour where T : UIModuleData, new()
{
	public virtual void Init () { }

	public virtual void Show (T data) 
	{
		cacheGameObject.SetActive(true);
	}

	public virtual void Hide () 
	{
		cacheGameObject.SetActive(false);
	}

	public static V CreateUIModule<V> (Object resource, RectTransform parent) where V : UIModule<T>
	{
		if (resource != null && parent != null) 
		{
			GameObject go = Instantiate(resource) as GameObject;
			var script = go.GetComponent<V>();
			if (script != null && script.cacheRectTransform != null)
			{
				script.cacheRectTransform.SetParent(parent);
				script.cacheRectTransform.localPosition = Vector3.zero;
				script.cacheRectTransform.localRotation = Quaternion.identity;

				return script;
			}
			else
			{
				Destroy(go);
				// TODO log nonexistent uimodule
			}
		}
		// TODO log invalid parameters
		return null;
	}
}
public abstract class UIModuleData {}

