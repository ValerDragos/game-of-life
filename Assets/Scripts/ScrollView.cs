using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;
using System.Collections.Generic;

public class ScrollView : ScrollRect {

	private enum RectPosition { Top, Bottom, Left, Right }

	private RectTransform _rectTransform;
	private IAdapter _adapter;
	private LinkedList<IContainer> _activeContainers = new LinkedList<IContainer>();
	private LinkedList<IContainer> _inactiveContainers = new LinkedList<IContainer>();

	private Vector2 _dragDistance;

	// Use this for initialization
	protected override void Awake () {
		_rectTransform = (RectTransform) transform;
		ScrollRect.ScrollRectEvent ev = new ScrollRect.ScrollRectEvent();
		ev.AddListener (delegate(Vector2 arg0) 
		{
			_dragDistance += arg0;
		});
		onValueChanged = ev;
	}

	public void AddAdapter (IAdapter adapter)
	{
		CleanListView ();
		_adapter = adapter;
	}

	private void PopulateList ()
	{
		_rectTransform.pivot = new Vector2(0.5f,1f);
		content.pivot = new Vector2(0.5f,1f);
		content.position = Vector3.zero;

		var listviewRect = _rectTransform.rect;
		float distance = 0;

		Vector3 containerPosition = Vector3.zero;
		for (int i=0;i<_adapter.Count;i++)
		{
			var container = GetInactiveContainer ();
			container.Show();

			var containerRect = container.rectTransform.rect;
			_activeContainers.AddLast(container);
			containerPosition.y = distance;
			container.rectTransform.position = containerPosition;

			distance += containerRect.height;
			if (distance>listviewRect.y) break;
		}

		distance = Mathf.Max(distance, listviewRect.height);
		//content. TODO set size.

	}

	private void GetContainerPositionAtRectPosition (RectPosition rectPos, ref Rect rect, ref Vector2 pivot, ref Vector3 localPos)
	{
		switch (rectPos)
		{
		case RectPosition.Top:
			localPos.x -= (0.5f - pivot.x) * rect.width;
			localPos.y -= (1f - pivot.y) * rect.height;
			break;
		case RectPosition.Bottom:
			localPos.x -= (0.5f - pivot.x) * rect.width;
			localPos.y -= pivot.y * rect.height;
			break;
		case RectPosition.Left:
			localPos.x -= pivot.x * rect.width;
			localPos.y -= (0.5f - pivot.y) * rect.height;
			break;
		case RectPosition.Right:
			localPos.x -= (1f - pivot.x) * rect.width;
			localPos.y -= (0.5f - pivot.y) * rect.height;
			break;
		}
	}

	private IContainer GetInactiveContainer ()
	{
		if (_inactiveContainers.Count>0)
		{
			var container = _inactiveContainers.Last;
			_inactiveContainers.RemoveLast();
			return container.Value;
		}
		return _adapter.CreateContainer(content);
	}

	private void CleanListView ()
	{
		_adapter = null;
		_dragDistance = Vector2.zero;
		foreach (var container in _activeContainers)
		{
			container.DestroyContent();
		}
		foreach (var container in _inactiveContainers)
		{
			container.DestroyContent();
		}
		_activeContainers.Clear();
		_inactiveContainers.Clear();
	}

	private void HideAllElements ()
	{
		foreach (var container in _activeContainers)
		{
			container.Hide();
			_inactiveContainers.AddLast(container);
		}
		_activeContainers.Clear();
	}
}

public abstract class Adapter<T,V> : IAdapter where T : MonoBehaviour
{
	private UnityEngine.Object _prefab;
	private List<V> _data;

	internal override int Count {get {return _data.Count;}}

	public Adapter (List<V> data, UnityEngine.Object prefab)
	{
		_prefab = prefab;
		_data = new List<V>(data);
	}

	internal override IContainer CreateContainer ( RectTransform parent)
	{
		if (_prefab != null && parent != null)
		{
			GameObject gameObject = (GameObject) GameObject.Instantiate(_prefab);
			var rectTransform = (RectTransform )gameObject.transform;
			if (rectTransform != null)
			{
				var script = gameObject.GetComponent<T>();
				if (script != null)
				{
					var container = new Container<T>(script, gameObject, rectTransform);
					container.OnBeforeElementVisible += OnBeforeElementVisible;
					container.OnBeforeElementHidden += OnBeforeElementHidden;
					container.OnBeforeElementDestroyed += OnBeforeElementDestroyed;
					return container;
				}
			}
		}
		return null;
	}

	public abstract void OnBeforeElementVisible (T script);
	public abstract void OnBeforeElementHidden (T script);
	public abstract void OnBeforeElementDestroyed (T script);

}

internal class Container<T> : IContainer where T : MonoBehaviour
{
	private T _script;
	private GameObject _gameObject;
	private RectTransform _rectTransform;

	public override RectTransform rectTransform {get {return _rectTransform;}}

	internal event Action<T> OnBeforeElementVisible;
	internal event Action<T> OnBeforeElementDestroyed;
	internal event Action<T> OnBeforeElementHidden;

	internal Container (T script, GameObject gameObject, RectTransform rectTransform)
	{
		_script = script;
		_gameObject = gameObject;
		_rectTransform = rectTransform;
	}

	public override void Show()
	{
		if (OnBeforeElementVisible != null) OnBeforeElementVisible(_script);
		_gameObject.SetActive(true);
	}

	public override void Hide()
	{
		if (OnBeforeElementHidden != null) OnBeforeElementHidden(_script);
		_gameObject.SetActive(false);
	}

	public override void DestroyContent ()
	{
		if (OnBeforeElementDestroyed != null) OnBeforeElementDestroyed(_script);
		_rectTransform = null;
		_script = null;
		GameObject.Destroy(_gameObject);
	}
}

public abstract class IAdapter
{
	internal abstract IContainer CreateContainer (RectTransform parent);
	internal abstract int Count {get;}
}

internal abstract class IContainer
{
	public abstract RectTransform rectTransform {get;}

	public abstract void Hide();
	public abstract void DestroyContent();
	public abstract void Show ();
}
