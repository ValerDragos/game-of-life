using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using RenderTextureDrawer;
using FileManager;

namespace GOL
{
	[Flags]
	public enum GOL_Neighbours
	{
		Top = 1,
		TopRight = 2,
		Right = 4,
		BottomRight = 8,
		Bottom = 16,
		BottomLeft = 32,
		Left = 64,
		TopLeft = 128
	}
	[RequireComponent(typeof(CameraDrawer))]
	public sealed class Processor : BaseMonoBehaviour 
	{
		const string COROUTINE_NAME = "EveryFrame";

		[SerializeField] private SerializedResource _layerResource;

		public event Action<int,int> OnRenderTextureSizeChanged;

		private int _amountOfSamples = 8;
		private Layer _processorLayer;
		private PlaneGridCreator _planeCreator;
		private RenderTextureContainer _frameOne, _frameTwo;
		private CameraDrawer _drawer;
		private MaterialMultyCompileFlags _multyCompleFlags;

		private Func<float> _deltaTimeGetter = delegate { return Time.deltaTime; };
		public Func<float> DeltaTimeGetter
		{
			get {return _deltaTimeGetter;}
			set {_deltaTimeGetter = value?? delegate { return Time.deltaTime; };}
		}

		private float _processSpeedSeconds = 0, _zSpeedSeconds = 0, _wSpeedSeconds = 0;
		private float _processSpeedFps = 0, _zSpeedFps = 0, _wSpeedFps = 0;
		public float zSpeedFps
		{
			get { return _zSpeedFps; }
			set 
			{ 
				_zSpeedFps = Mathf.Max(0, value);
				if (_zSpeedFps > 0)
				{
					_zSpeedSeconds = 1 / _zSpeedFps;
				}
				else
				{
					_zSpeedSeconds = 0;
				}
			}
		}
		public float wSpeedFps
		{
			get { return _wSpeedFps; }
			set 
			{ 
				_wSpeedFps = Mathf.Max(0, value);
				if (_zSpeedFps > 0)
				{
					_wSpeedSeconds = 1 / _wSpeedFps;
				}
				else
				{
					_wSpeedSeconds = 0;
				}
			}
		}
		public float processSpeedFps
		{
			get { return _processSpeedFps; }
			set 
			{ 
				_processSpeedFps = Mathf.Max(0, value);
				if (_processSpeedFps > 0)
				{
					_processSpeedSeconds = 1 / _processSpeedFps;
				}
				else
				{
					_processSpeedSeconds = 0;
				}
			}
		}
		public bool CellsDie = false;
		public bool UpdateAnimation = false;

		void Awake ()
		{
			_processorLayer = _layerResource.CreateInstanceAndGetComponent<Layer>();
			if (_processorLayer != null)
			{
				_multyCompleFlags = new MaterialMultyCompileFlags(_processorLayer.material);
				_drawer.AddLayer(_processorLayer);
			}
			_planeCreator = new PlaneGridCreator (1,1);
		}

		public void SetUp (int width, int height)
		{
			width = Mathf.Max (1, width);
			height = Mathf.Max (1, height);

			_frameOne = new RenderTextureContainer (width, height);
			_frameTwo = new RenderTextureContainer (width, height);

			_planeCreator.CalculateBasedOnRect (new Rect (-width * 0.5f, -height * 0.5f, width, height));
			_processorLayer.Setup (_planeCreator);

			if (OnRenderTextureSizeChanged != null)	OnRenderTextureSizeChanged(width, height);
		}

		private void SetVectorValues (ref Vector4[] vectors, ref int amountOfSamples, int index, float firstValue, float secondValue)
		{
			if (amountOfSamples % 2 == 0)
			{
				vectors[index].x = firstValue;
				vectors[index].y = secondValue;
			}
			else
			{
				vectors[index].z = firstValue;
				vectors[index].w = secondValue;
			}
			amountOfSamples++;
		}

		public void SetNeighbours (GOL_Neighbours flags)
		{
			Vector2 offsets = new Vector2 (1f / _frameOne.texture.width, 1f / _frameOne.texture.height);
			Vector4[] vectors = new Vector4[4];
			_amountOfSamples = 0;

			if ((flags & GOL_Neighbours.Top) == GOL_Neighbours.Top)
			{
				SetVectorValues (ref vectors, ref _amountOfSamples, _amountOfSamples / 2, 0, offsets.y);
			}
			if ((flags & GOL_Neighbours.TopRight) == GOL_Neighbours.TopRight)
			{
				SetVectorValues (ref vectors, ref _amountOfSamples, _amountOfSamples / 2, offsets.x, offsets.y);
			}
			if ((flags & GOL_Neighbours.Right) == GOL_Neighbours.Right)
			{
				SetVectorValues (ref vectors, ref _amountOfSamples, _amountOfSamples / 2, offsets.x, 0);
			}
			if ((flags & GOL_Neighbours.BottomRight) == GOL_Neighbours.BottomRight)
			{
				SetVectorValues (ref vectors, ref _amountOfSamples, _amountOfSamples / 2, offsets.x, -offsets.y);
			}
			if ((flags & GOL_Neighbours.Bottom) == GOL_Neighbours.Bottom)
			{
				SetVectorValues (ref vectors, ref _amountOfSamples, _amountOfSamples / 2, 0, -offsets.y);
			}
			if ((flags & GOL_Neighbours.BottomLeft) == GOL_Neighbours.BottomLeft)
			{
				SetVectorValues (ref vectors, ref _amountOfSamples, _amountOfSamples / 2, -offsets.x, -offsets.y);
			}
			if ((flags & GOL_Neighbours.Left) == GOL_Neighbours.Left)
			{
				SetVectorValues (ref vectors, ref _amountOfSamples, _amountOfSamples / 2, -offsets.x, 0);
			}
			if ((flags & GOL_Neighbours.TopLeft) == GOL_Neighbours.TopLeft)
			{
				SetVectorValues (ref vectors, ref _amountOfSamples, _amountOfSamples / 2, -offsets.x, offsets.y);
			}
			_processorLayer.material.SetVector ("firstVector", vectors [0]);
			_processorLayer.material.SetVector ("secondVector", vectors [1]);
			_processorLayer.material.SetVector ("thirdVector", vectors [2]);
			_processorLayer.material.SetVector ("forthVector", vectors [3]);
		}

		public void Play ()
		{
			if (_frameOne == null || _frameTwo == null)
			{
				LogError("Call SetUp first!");
				return;
			}
				Stop ();
				StartCoroutine(COROUTINE_NAME);
		}
		public void Stop ()
		{
			StopCoroutine (COROUTINE_NAME);
		}

		private int GetTimesToProcess (ref float deltaTime, ref float timeInSeconds)
		{
			if (timeInSeconds > 0)
			{
				int times = (int)(deltaTime / timeInSeconds);
				deltaTime %= timeInSeconds;
				return times;
			} 
			else 
			{
				deltaTime = 0;
				return 0;
			}
		}

		private IEnumerator EveryFrame ()
		{
			float processorDeltaTime = 0;
			float zComponentDeltaTime = 0;
			float wComponentDeltaTime = 0;

			while (true)
			{
				processorDeltaTime += DeltaTimeGetter();
				zComponentDeltaTime += DeltaTimeGetter();
				wComponentDeltaTime += DeltaTimeGetter();

				int processorTimes = GetTimesToProcess(ref processorDeltaTime, ref _processSpeedSeconds);
				int zComponentTimes = GetTimesToProcess(ref zComponentDeltaTime, ref _zSpeedSeconds);
				int wComponentTimes = GetTimesToProcess(ref wComponentDeltaTime, ref _wSpeedSeconds);

				while (processorTimes>0 || zComponentTimes > 0 || wComponentTimes  > 0)
				{
					RenderTextureContainer aux = _frameOne;
					_frameOne = _frameTwo;
					_frameTwo = aux;

					_processorLayer.material.mainTexture = _frameOne.texture;
					_drawer.SetRenderTexture(_frameTwo.texture);

					if (processorTimes>0)
					{
						processorTimes--;
						_multyCompleFlags.SetSampleAmountFlag(_amountOfSamples);
					}
					else
					{
						_multyCompleFlags.SetSampleAmountFlag(0);
					}

					if (zComponentTimes>0)
					{
						zComponentTimes--;
						_multyCompleFlags.SetLifeTimeFlag((CellsDie)? MaterialMultyCompileFlags.LifeTime.LifeTimeAndDie : MaterialMultyCompileFlags.LifeTime.LifeTime);
					}
					else
					{
						_multyCompleFlags.SetLifeTimeFlag(MaterialMultyCompileFlags.LifeTime.NoLifeTime);
					}

					if (wComponentDeltaTime>0)
					{
						wComponentDeltaTime--;
						_multyCompleFlags.SetAnimationFlag((UpdateAnimation)? MaterialMultyCompileFlags.Animation.IncrementWComponent : MaterialMultyCompileFlags.Animation.DontIncrementWComponent);
					}
					else
					{
						_multyCompleFlags.SetAnimationFlag(MaterialMultyCompileFlags.Animation.DontIncrementWComponent);
					}
					_drawer.Render();
				}
				yield return null;
			}
		}
	}
}
