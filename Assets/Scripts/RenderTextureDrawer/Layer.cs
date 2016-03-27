using UnityEngine;
using System.Collections;

namespace RenderTextureDrawer
{
	public class Layer : BaseMonoBehaviour
	{
		[SerializeField] private Material _material;
		public Material material { get { return _material; } }

		private Mesh _mesh;

		protected virtual void Awake ()
		{
			CreateComponentsIfNull ();
		}

		public void Setup (GeometryCreator creator)
		{
			if (creator != null) 
			{
				SetMesh (new Mesh());
				_mesh.vertices = creator.Vertices;
				_mesh.triangles = creator.Triangles;
				if (creator.UvsCount == creator.VerticesCount) _mesh.uv = creator.Uvs;
				if (creator.ColorsCount == creator.VerticesCount) _mesh.colors = creator.Colors;
			}
		}

		public void Clear ()
		{
			SetMesh (null);
		}

		private void SetMesh (Mesh mesh)
		{
			cacheMeshFilter.mesh = mesh;
			_mesh = mesh;
		}

		private void CreateComponentsIfNull ()
		{
			CacheOrCreateMeshFilter();
			CacheOrCreateMeshRenderer();
		}

		public void SetActive (bool active)
		{
			cacheGameObject.SetActive (active);
		}
	}
}
