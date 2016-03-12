using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Screwtape
{
	[RequireComponent(typeof(MeshFilter))]
	[RequireComponent(typeof(MeshRenderer))]
	[ExecuteInEditMode]
	public class TextureStrip : MonoBehaviour 
	{
		[SerializeField]
		private List<Vector3> m_points;

		[SerializeField]
		private float m_height = 1f;

		[SerializeField]
		private float m_scale = 1f;

        public Material StripMaterial;

		private MeshFilter m_meshFilter;
		private MeshRenderer m_meshRenderer;

		public List<Vector3> Points
		{
			get
			{
				return m_points;
			}
		}

		public float Height
		{
			get
			{
				return m_height;
			}

			set
			{
				if (value != m_height)
				{
					m_height = value;
					m_height = Mathf.Clamp(m_height, 0f, 10f);
					UpdateMesh();
				}
			}
		}

		public float Scale
		{
			get
			{
				return m_scale;
			}

			set
			{
				if (value != m_scale)
				{
					m_scale = value;
					UpdateMesh();
				}
			}
		}

		void Awake()
		{
			m_meshFilter = GetComponent<MeshFilter>();
			m_meshRenderer = GetComponent<MeshRenderer>();

			if (m_points == null)
			{
				ResetMesh();
			}

			if (!Application.isPlaying)
			{
				UpdateMesh();
			}

			//
		}

		public void UpdateMesh()
		{
			var verts = new List<Vector3>();
			var uv = new List<Vector2>();
			var trianges = new List<int>();
			var distances = new List<float>();


			distances.Add(0f);
			// Generate Vertices
			for (int i = 0; i < m_points.Count; i++)
			{
				verts.Add(m_points[i] + new Vector3(0f, m_height * 0.5f, 0f));
				verts.Add(m_points[i] + new Vector3(0f, -m_height * 0.5f, 0f));

				if (i < m_points.Count - 1)
				{
					var dist = Vector2.Distance(m_points[i], m_points[i + 1]);
					distances.Add(dist * m_scale);
				}
			}

			int index = 0;
			float distance = 0;
			// Generate UV's
			for (int i = 0; i < verts.Count; i += 2)
			{
				distance += distances[index];
				index++;

				uv.Add(new Vector2(distance, 1f));
				uv.Add(new Vector2(distance, 0f));
			}

			index = 0;
			// Generate Triangles
			for (int i = 0; i < verts.Count - 2; i += 2)
			{
				trianges.Add(i);
				trianges.Add(i + 2);
				trianges.Add(i + 1);

				trianges.Add(i + 1);
				trianges.Add(i + 2);
				trianges.Add(i + 3);

				index++;
			}

			Mesh mesh = new Mesh();

			mesh.vertices = verts.ToArray();
			mesh.uv = uv.ToArray();
			mesh.triangles = trianges.ToArray();
			mesh.RecalculateNormals();

			m_meshFilter.mesh = mesh;
			m_meshRenderer.material = StripMaterial;
		}

		public void ResetMesh()
		{
			m_points = new List<Vector3>();

			m_points.Add(new Vector3(-3f, 0f, 0f));
			m_points.Add(new Vector3(3f, 0f, 0f));

			UpdateMesh();
		}
	}
}
