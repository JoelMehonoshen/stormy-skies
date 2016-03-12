using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Screwtape
{
    [RequireComponent(typeof(MeshFilter))]
    [RequireComponent(typeof(MeshRenderer))]
    [RequireComponent(typeof(PolygonCollider2D))]
    [ExecuteInEditMode]
    public class Catwalk : MonoBehaviour
    {
        [SerializeField]
        private List<Vector3> m_points;

        [SerializeField]
        private float m_height = 1f;

        [SerializeField]
        private float m_depth = 2f;

        [SerializeField]
        private float m_scale = 1f;

        [SerializeField]
        private bool m_movingPlatform = false;

        public Material StripMaterial;

        private MeshFilter m_meshFilter;
        private MeshRenderer m_meshRenderer;

        private List<Transform> m_oldSurfaces;

        private PolygonCollider2D m_collider;

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

        public float Depth
        {
            get
            {
                return m_depth;
            }

            set
            {
                if (value != m_depth)
                {
                    m_depth = value;
                    UpdateMesh();
                }
            }
        }

        public bool MovingPlatform
        {
            get
            {
                return m_movingPlatform;
            }

            set
            {
                if (value != m_movingPlatform)
                {
                    m_movingPlatform = value;
                }
            }
        }

        void Awake()
        {
            m_meshFilter = GetComponent<MeshFilter>();
            m_meshRenderer = GetComponent<MeshRenderer>();
            m_collider = GetComponent<PolygonCollider2D>();
            m_oldSurfaces = new List<Transform>();

            gameObject.layer = 8;

            if (m_points == null)
            {
                ResetMesh();
            }

            if (!Application.isPlaying)
            {
                UpdateMesh();
            }
        }

        void Update()
        {
#if UNITY_EDITOR
            if (!Application.isPlaying)
            {
                while (m_oldSurfaces.Count > 0)
                {
                    var surface = m_oldSurfaces[0];
                    m_oldSurfaces.RemoveAt(0);

                    if (surface != null)
                    {
                        DestroyImmediate(surface.gameObject);
                    }
                }
            }
#endif
        }

        public void UpdateMesh()
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                var child = transform.GetChild(i);
                if (child.name.Equals("Surface"))
                {
                    m_oldSurfaces.Add(child);
                }
            }

            UpdateFrontMesh();
            UpdateSurfaceMesh();
        }

        public void UpdateFrontMesh()
        {
            var verts = new List<Vector3>();
            var uv = new List<Vector2>();
            var trianges = new List<int>();
            var distances = new List<float>();

            distances.Add(0f);
            // Generate Vertices for front
            for (int i = 0; i < m_points.Count; i++)
            {
                verts.Add(m_points[i] + new Vector3(0f, 0f, -m_depth * 0.5f));
                verts.Add(m_points[i] + new Vector3(0f, -m_height, -m_depth * 0.5f));

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

        void UpdateSurfaceMesh()
        {
            var refPoints = new List<Vector3>();

            for (int i = 0; i < m_points.Count; i++)
            {
                refPoints.Add(m_points[i]);
            }

            for (int i = m_points.Count - 1; i >= 0; i--)
            {
                refPoints.Add(m_points[i] + new Vector3(0f, -m_height, 0f));
            }

            var colliderPoints = new List<Vector2>();

            for (int i = 0; i < refPoints.Count; i++)
            {
                colliderPoints.Add(refPoints[i].ToVector2());
            }

            m_collider.points = colliderPoints.ToArray();

            refPoints.Add(m_points[0]);

            var verts = new List<Vector3>();
            var uv = new List<Vector2>();
            var trianges = new List<int>();
            var distances = new List<float>();

            distances.Add(0f);
            // Generate Vertices along top
            for (int i = 0; i < refPoints.Count; i++)
            {
                verts.Add(refPoints[i] + new Vector3(0f, 0f, m_depth * 0.5f));
                verts.Add(refPoints[i] + new Vector3(0f, 0f, -m_depth * 0.5f));

                if (i < refPoints.Count - 1)
                {
                    var dist = Vector2.Distance(refPoints[i], refPoints[i + 1]);
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

            var go = new GameObject("Surface");
            go.transform.parent = transform;
            go.transform.position = transform.position;
            var mr = go.AddComponent<MeshRenderer>();
            var mf = go.AddComponent<MeshFilter>();

            Mesh mesh = new Mesh();

            mesh.vertices = verts.ToArray();
            mesh.uv = uv.ToArray();
            mesh.triangles = trianges.ToArray();
            mesh.RecalculateNormals();

            mf.mesh = mesh;
            mr.material = StripMaterial;
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
