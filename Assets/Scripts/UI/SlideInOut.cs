using UnityEngine;
using System.Collections;
using System;

[RequireComponent(typeof(RectTransform))]
public class SlideInOut : MonoBehaviour {

	public enum State { Open, Opening, Closed, Closing }
	public enum Direction {Down, Up, Left, Right}

	public Direction direction;
	public float duration;
	public State state {get; private set;}

	[SerializeField] private bool openOnStart = false;
	private RectTransform _rectTransform;
	private Action _updateAction;
	private float _timePassed;

	public EasingFormulas.Type openType;
	public EasingType.Easing openEasing;
	public EasingFormulas.Type closeType;
	public EasingType.Easing closeEasing;

	private EasingType.Formula _openFormula, _closeFormula;

	void Awake () {
		_rectTransform = (RectTransform) transform;

		_openFormula = EasingFormulas.GetFormula(openType, openEasing);
		_closeFormula = EasingFormulas.GetFormula(closeType, closeEasing);
	}

	void Start ()
	{
		if (openOnStart) OpenInstant ();
		else CloseInstant ();
	}
	
	// Update is called once per frame
	void Update () {
		_updateAction();
	}

	public void Open (/*Action done = null*/)
	{
		_updateAction = delegate 
		{
			_timePassed += Time.deltaTime;
			if (_timePassed<duration)
			{
				SetPosition (_openFormula(1-_timePassed/duration));
			}
			else
			{
				OpenInstant ();
				//if (done != null) done();
			}
		};
		enabled = true;
	}

	public void Close (/*Action done*/)
	{
		_updateAction = delegate 
		{
			_timePassed -= Time.deltaTime;
			if (_timePassed>0)
			{
				SetPosition (_closeFormula(1-_timePassed/duration));
			}
			else
			{
				CloseInstant ();
				//if (done != null) done();
			}
		};
		enabled = true;
	}

	public void OpenInstant ()
	{
		_timePassed = duration;
		SetPosition (0);
		enabled = false;
	}

	public void CloseInstant ()
	{
		_timePassed = 0;
		SetPosition (1);
		enabled = false;
	}
	
	Vector2 _sizeUsedForAnimation;
	private void SetPosition (float factor)
	{
		Rect transformRect = _rectTransform.rect;
		Vector3 localPos = _rectTransform.localPosition;
		localPos.x -= _sizeUsedForAnimation.x;
		localPos.y -= _sizeUsedForAnimation.y;
		switch (direction)
		{
		case Direction.Down:
			_sizeUsedForAnimation.x = 0;
			_sizeUsedForAnimation.y = -transformRect.height*factor;
			break;
		case Direction.Up:
			_sizeUsedForAnimation.x = 0;
			_sizeUsedForAnimation.y = transformRect.height*factor;
			break;
		case Direction.Left:
			_sizeUsedForAnimation.x = -transformRect.width*factor;
			_sizeUsedForAnimation.y = 0;
			break;
		case Direction.Right:
			_sizeUsedForAnimation.x = transformRect.width*factor;
			_sizeUsedForAnimation.y = 0;
			break;
		}
		localPos.x += _sizeUsedForAnimation.x;
		localPos.y += _sizeUsedForAnimation.y;
		_rectTransform.localPosition = localPos;
	}
}
