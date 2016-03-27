using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System;

namespace RenderTextureDrawer
{
	[RequireComponent(typeof(Camera))]
	public class CameraDrawer : BaseMonoBehaviour 
	{
		private int _startRenderQueue = 0;
		private List<Layer> _layers = new List<Layer>();
		private int _renderQueweCount = 0;
		private float _distanceFromCamera = 0;

		public event Action OnBeforeDraw;
		public event Action OnAfterDraw;
		public event Action<RenderTexture> OnRenderTextureChanged;

		public float RenderDistance { get; private set; }
		public ReadOnlyCollection<Layer> layers
		{
			get { return _layers.AsReadOnly(); }
		}

		protected virtual void Awake ()
		{
			if (!SystemInfo.supportsRenderTextures) 
			{
				LogWarning("RenderTextures not supported!");
				enabled = false;
			}
			else if (cacheCamera == null) 
			{
				LogError("Missing Camera Component!");
				enabled = false;
			}
			else
			{
				cacheCamera.enabled = false;
			}
		}

		public void SetRenderTexture (RenderTexture rt)
		{
			if (rt != null)
			{
				cacheCamera.targetTexture = rt;
				if (cacheCamera.orthographic)
				{
					_distanceFromCamera = 1;
					cacheCamera.aspect = (float)rt.width / rt.height;
					cacheCamera.orthographicSize = rt.height * 0.5f;
				}
				else
				{
					// TODO implement calculation for prespective camera
				}
				if (OnRenderTextureChanged != null) OnRenderTextureChanged(rt);
			}
			else
			{
				Log ("RenderTexture must not be null when setting!");
			}

		}

		public RenderTexture GetRenderTexture (RenderTexture rt)
		{
			return cacheCamera.targetTexture;
		}

		public void AddLayer (Layer layer)
		{
			AddLayer (layer, Vector3.zero, Quaternion.identity);
		}

		public void AddLayer (Layer layer, Vector3 localPosition, Quaternion rotation)
		{
			if (layer != null) 
			{
				_layers.Add(layer);
				layer.material.renderQueue = _startRenderQueue + _renderQueweCount;
				_renderQueweCount++;

				layer.cacheTransform.parent = cacheTransform;
				layer.cacheTransform.localPosition = new Vector3(localPosition.x, localPosition.y, localPosition.z + _distanceFromCamera);
				layer.cacheTransform.localRotation = rotation;
			}
		}

		public bool removeLayer (Layer layer)
		{
			return _layers.Remove(layer);
		}

		public void Render ()
		{
			if (OnBeforeDraw != null) OnBeforeDraw();
			cacheCamera.Render ();
			if (OnAfterDraw != null) OnAfterDraw();
		}

		protected override string LOG_TITLE { get { return "RenderTextureDrawer"; } }
		CameraDrawer () : base()
		{
			#if CAMERA_DRAWER_LOGS
			LOGS_ENABLED = true;
			#endif
		}
	}
}
