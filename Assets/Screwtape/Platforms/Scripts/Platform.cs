using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Screwtape
{
    [RequireComponent(typeof(MeshFilter))]
    [RequireComponent(typeof(MeshRenderer))]
    [RequireComponent(typeof(PolygonCollider2D))]
    [ExecuteInEditMode]
    public class Platform : MonoBehaviour
    {
        #region Fields
        [SerializeField]
        private List<Vector3> m_terrainPoints;

        [SerializeField]
        private bool m_isSmooth;

        public bool IsSmooth
        {
            get
            {
                return m_isSmooth;
            }

            set
            {
                if (value != m_isSmooth)
                {
                    m_isSmooth = value;
                    UpdateMesh();
                }
            }
        }

        [SerializeField]
        private float m_depth = 4f;

        [SerializeField]
        private float m_taper = 1f;

        [SerializeField]
        private int m_subDivisions = 0;

        [SerializeField]
        private float m_faceRotation = 0f;
        public float FaceRotation
        {
            get
            {
                return m_faceRotation;
            }

            set
            {
                if (value != m_faceRotation)
                {
                    m_faceRotation = value;
                    UpdateMesh();
                }
            }
        }

        [SerializeField]
        private float m_faceScale = 1f;
        public float FaceScale
        {
            get { return m_faceScale; }
            set
            {
                if (value != m_faceScale)
                {
                    m_faceScale = value;
                    UpdateMesh();
                }
            }
        }

        [SerializeField]
        private Vector2 m_faceOffset;
        public Vector2 FaceOffset
        {
            get { return m_faceOffset; }
            set
            {
                if (value != m_faceOffset)
                {
                    m_faceOffset = value;
                    UpdateMesh();
                }
            }
        }

        [SerializeField]
        private float m_floorScale = 1f;
        public float FloorScale
        {
            get { return m_floorScale; }
            set
            {
                if (value != m_floorScale)
                {
                    m_floorScale = value;
                    UpdateMesh();
                }
            }
        }

        [SerializeField]
        private Vector2 m_floorOffset;
        public Vector2 FloorOffset
        {
            get { return m_floorOffset; }
            set
            {
                if (value != m_floorOffset)
                {
                    m_floorOffset = value;
                    UpdateMesh();
                }
            }
        }

        [SerializeField]
        private float m_ceilingScale = 1f;
        public float CeilingScale
        {
            get { return m_ceilingScale; }
            set
            {
                if (value != m_ceilingScale)
                {
                    m_ceilingScale = value;
                    UpdateMesh();
                }
            }
        }

        [SerializeField]
        private Vector2 m_ceilingOffset;
        public Vector2 CeilingOffset
        {
            get { return m_ceilingOffset; }
            set
            {
                if (value != m_ceilingOffset)
                {
                    m_ceilingOffset = value;
                    UpdateMesh();
                }
            }
        }

        [SerializeField]
        private float m_wallScale = 1f;
        public float WallScale
        {
            get { return m_wallScale; }
            set
            {
                if (value != m_wallScale)
                {
                    m_wallScale = value;
                    UpdateMesh();
                }
            }
        }

        [SerializeField]
        private Vector2 m_wallOffset;
        public Vector2 WallOffset
        {
            get { return m_wallOffset; }
            set
            {
                if (value != m_wallOffset)
                {
                    m_wallOffset = value;
                    UpdateMesh();
                }
            }
        }

        [SerializeField]
        private string m_trimSortingLayer = "Default";

        [SerializeField]
        private int m_trimSortingOrder = 0;

        public string TrimSortingLayer
        {
            get { return m_trimSortingLayer; }
            set
            {
                m_trimSortingLayer = value;
                UpdateMesh();
            }
        }

        public int TrimSortingOrder
        {
            get { return m_trimSortingOrder; }
            set
            {
                m_trimSortingOrder = value;
                UpdateMesh();
            }
        }


        public Material WallMaterial;
        public Material FloorMaterial;
        public Material CeilingMaterial;
        public Material FaceMaterial;

        public Material FloorTrimMaterial;
        public Material CeilingTrimMaterial;
        public Material WallTrimMaterial;

        public bool DisplayTrim = false;

        private MeshFilter m_frontMeshFilter;
        private MeshRenderer m_frontMeshRenderer;

        private List<Vector3> m_smoothPoints;

        private List<Transform> m_oldSurfaces;

        private PolygonCollider2D m_collider;

        [SerializeField]
        private float m_trimOffset = 0f;
        public float TrimOffset
        {
            get
            {
                return m_trimOffset;
            }

            set
            {
                if (value != m_trimOffset)
                {
                    m_trimOffset = value;
                    UpdateMesh();
                }
            }
        }

        [SerializeField]
        private float m_trimSize = 1f;
        public float TrimSize
        {
            get
            {
                return m_trimSize;
            }

            set
            {
                if (m_trimSize != value)
                {
                    m_trimSize = value;
                    UpdateMesh();
                }
            }
        }
        #endregion

        #region Properties
        public bool GenerateCollider
        {
            get
            {
                if (m_collider == null)
                {
                    m_collider = GetComponent<PolygonCollider2D>();
                }

                return m_collider.enabled;
            }

            set
            {
                if (m_collider == null)
                {
                    m_collider = GetComponent<PolygonCollider2D>();
                }

                m_collider.enabled = value;
            }
        }

        public int SubDivisions
        {
            get
            {
                return m_subDivisions;
            }

            set
            {
                m_subDivisions = value;

                m_subDivisions = Mathf.Clamp(m_subDivisions, 0, 10);
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
                if (m_depth != value)
                {
                    m_depth = value;
                    UpdateMesh();
                }
            }
        }

        public List<Vector3> Points
        {
            get
            {
                return m_terrainPoints;
            }
        }
        #endregion

        #region Unity Callbacks
        void Awake()
        {
            m_frontMeshFilter = GetComponent<MeshFilter>();
            m_frontMeshRenderer = GetComponent<MeshRenderer>();
            m_collider = GetComponent<PolygonCollider2D>();

            m_oldSurfaces = new List<Transform>();
            m_smoothPoints = new List<Vector3>();

            this.gameObject.layer = 8;

            if (m_terrainPoints == null)
            {
                ResetPlatform();
            }
        }

        void Update()
        {

#if UNITY_EDITOR
            if(!Application.isPlaying)
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
        #endregion

        #region Public Methods
        public void UpdateFrontMesh()
        {
            // Get an array of points to play with
			Vector3[] points = m_smoothPoints.ToArray();

			Vector2[] points2D = Get2DPoints(points);

			Vector3 centerPoint = GetCenterPoint(points);

            for (int i = 0; i < points.Length; i++)
            {
				points[i] -= centerPoint;
				points[i] *= m_taper;
				points[i] += centerPoint;
                
				points[i].z -= m_depth * 0.5f;
            }

			Triangulator t = new Triangulator(points2D);
			int[] triangles = t.Triangulate();

            // Create a list of UV's
            Vector2[] uv = new Vector2[points.Length];

            for (int i = 0; i < uv.Length; i++)
            {
                uv[i] = Rotate(points[i], m_faceRotation) * m_faceScale;
            }

            // Assign them to the mesh
            var mesh = new Mesh();
            mesh.vertices = points;
            mesh.uv = uv;
            mesh.triangles = triangles;
			mesh.RecalculateNormals();

            if (m_frontMeshFilter == null)
            {
                m_frontMeshFilter = GetComponent<MeshFilter>();
                m_frontMeshRenderer = GetComponent<MeshRenderer>();
            }

            m_frontMeshFilter.mesh = mesh;
            m_frontMeshRenderer.material = FaceMaterial;
        }

        void UpdateSurfaceMesh()
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                var child = transform.GetChild(i);
                if (child.name.Equals("Surface") || child.name.Equals("Trim"))
                {
                    m_oldSurfaces.Add(child);
                }
            }

            List<List<Vector3>> pointLists = new List<List<Vector3>>();
            List<Vector3> normals = new List<Vector3>();

            int listIndex = 0;
            pointLists.Add(new List<Vector3>());

            for (int i = 0; i < m_smoothPoints.Count; i++)
            {
                var p1 = m_smoothPoints[i];
				p1.z += m_depth * 0.5f;
                
                var p2 = m_smoothPoints[i];
                p2.z -= m_depth * 0.5f;

                int nextIndex = i + 1;
                nextIndex %= m_smoothPoints.Count;

                var cross = -GetNormal(m_smoothPoints[nextIndex], p1, p2);

                normals.Add(cross.normalized);

                //var angle = Vector2.Angle(m_terrainPoints[i], m_terrainPoints[nextIndex]);

                //Debug.Log(cross.normalized);

                pointLists[listIndex].Add(p1);
                pointLists[listIndex].Add(p2);

                if (i > 0)
                {
                    if (Vector2.Angle(normals[i], normals[i - 1]) > 35f)
                    {
                        listIndex++;
                        pointLists.Add(new List<Vector3>());

                        pointLists[listIndex].Add(p1);
                        pointLists[listIndex].Add(p2);
                    }
                }
            }

            // Do the last point again, rather than going back to the first one
            var point = m_smoothPoints[0];
            point.z += m_depth * 0.5f;
            pointLists[listIndex].Add(point);

            point = m_smoothPoints[0];
            point.z -= m_depth * 0.5f;
            pointLists[listIndex].Add(point);

            int index = 0;
            foreach (var pointList in pointLists)
            {
                var go = new GameObject("Surface");
                go.transform.parent = transform;
                go.transform.position = transform.position;

                var filter = go.AddComponent<MeshFilter>();
                var renderer = go.AddComponent<MeshRenderer>();
                go.AddComponent<SurfaceTextureModifier>();

                filter.mesh = CreateMesh(pointList.ToArray());
                renderer.material = GetMaterial(pointList.ToArray());

                if (DisplayTrim)
                {
                    var trim = new GameObject("Trim");
                    trim.transform.parent = transform;
                    trim.transform.position = transform.position;

                    var trimFilter = trim.AddComponent<MeshFilter>();
                    var trimRenderer = trim.AddComponent<MeshRenderer>();

                    var lastIndex = index - 1;
                    var nextIndex = index + 1;

                    if (lastIndex < 0)
                    {
                        lastIndex += pointLists.Count;
                    }

                    if (nextIndex >= pointLists.Count)
                    {
                        nextIndex -= pointLists.Count;
                    }

                    trimFilter.mesh = CreateTrimMesh(pointList.ToArray(), pointLists[lastIndex].ToArray(), pointLists[nextIndex].ToArray());
                    trimRenderer.material = GetTrimMaterial(pointList.ToArray());
                    trimRenderer.sortingLayerName = m_trimSortingLayer;
                    trimRenderer.sortingOrder = m_trimSortingOrder;
                }

                index++;
            }
        }

        public void ResetPlatform()
        {
            m_terrainPoints = new List<Vector3>();

            m_terrainPoints.Add(new Vector3(-5f, 2.5f, 0f));
            m_terrainPoints.Add(new Vector3(5f, 2.5f, 0f));
            m_terrainPoints.Add(new Vector3(5f, -2.5f, 0f));
            m_terrainPoints.Add(new Vector3(-5f, -2.5f, 0f));

            UpdateMesh();
        }

        public void SetPoints(List<Vector3> a_points)
        {
            m_terrainPoints = a_points;
            UpdateMesh();
        }

        public void UpdateMesh()
        {
            AddSmoothPoints();

            UpdateFrontMesh();
            UpdateSurfaceMesh();
            UpdateCollider();
        }
        #endregion

        #region Private Methods

        Mesh CreateMesh(Vector3[] a_points)
        {
            var points = a_points;

            float uvScale = GetTextureScale(a_points);
            Vector2 uvOffset = GetTextureOffset(a_points);

            List<float> backDistances = new List<float>();
            List<float> frontDistances = new List<float>();

            for (int i = 0; i < points.Length - 2; i++)
            {
                if (i % 2 == 0)
                {
                    backDistances.Add(Mathf.Abs(Vector3.Distance(points[i], points[i + 2])));
                }
                else
                {
                    frontDistances.Add(Mathf.Abs(Vector3.Distance(points[i], points[i + 2])));
                }
            }

            backDistances.Add(Mathf.Abs(Vector3.Distance(points[0], points[points.Length - 2])));
            frontDistances.Add(Mathf.Abs(Vector3.Distance(points[1], points[points.Length - 1])));

            // Create an array of triangles
            int index = 0;
            int[] triangles = new int[points.Length * 3];
            for (int i = 0; i < triangles.Length; i += 6)
            {
                if (index == points.Length - 2) // Deal with the loop
                {
                    //triangles[i] = index;
                    //triangles[i + 1] = 0;
                    //triangles[i + 2] = index + 1;

                    //triangles[i + 3] = index + 1;
                    //triangles[i + 4] = 0;
                    //triangles[i + 5] = 1;
                }
                else
                {
                    triangles[i] = index;
                    triangles[i + 1] = index + 2;
                    triangles[i + 2] = index + 1;

                    triangles[i + 3] = index + 1;
                    triangles[i + 4] = index + 2;
                    triangles[i + 5] = index + 3;
                }

                index += 2;
            }

            // Create an array of uvs
            Vector2[] uv = new Vector2[points.Length];

            float backDistance = 0f;
            float frontDistance = 0f;

            int distanceIndex = 0;

            for (int i = 0; i < uv.Length; i += 2)
            {
                uv[i] = Rotate(new Vector2(backDistance, m_depth), 90f) * uvScale + uvOffset;
                uv[i + 1] = Rotate(new Vector2(frontDistance, 0f), 90f) * uvScale + uvOffset;

                backDistance += backDistances[distanceIndex];
                frontDistance += frontDistances[distanceIndex];
                distanceIndex++;
            }

            // Assign them to the mesh 
            var mesh = new Mesh();
            mesh.vertices = points;
            mesh.uv = uv;
            mesh.triangles = triangles;
            mesh.RecalculateNormals();

            return mesh;
        }

        Material GetMaterial(Vector3[] a_points)
        {
            var normal = -GetNormal(a_points[0], a_points[1], a_points[2]).normalized;

            if (Mathf.Abs(normal.x) > Mathf.Abs(normal.y)) // It's a wall
            {
                return WallMaterial;
            }
            else // It's a Floor / Ceiling
            {
                if (normal.y > 0f)
                {
                    return FloorMaterial;
                }
                else
                {
                    return CeilingMaterial;
                }
            }
        }

        public Texture GetSurfaceTextureFromNormal(Vector2 a_normal)
        {
            if (Mathf.Abs(a_normal.x) > Mathf.Abs(a_normal.y)) // It's a wall
            {
                return WallMaterial.mainTexture;
            }
            else // It's a Floor / Ceiling
            {
                if (a_normal.y > 0f)
                {
                    return FloorMaterial.mainTexture;
                }
                else
                {
                    return CeilingMaterial.mainTexture;
                }
            }
        }

        float GetTextureScale(Vector3[] a_points)
        { 
            var normal = -GetNormal(a_points[0], a_points[1], a_points[2]).normalized;

            if (Mathf.Abs(normal.x) > Mathf.Abs(normal.y)) // It's a wall
            {
                return m_wallScale;
            }
            else // It's a Floor / Ceiling
            {
                if (normal.y > 0f)
                {
                    return m_floorScale;
                }
                else
                {
                    return m_ceilingScale;
                }
            }
        }

        Vector2 GetTextureOffset(Vector3[] a_points)
        {
            var normal = -GetNormal(a_points[0], a_points[1], a_points[2]).normalized;

            if (Mathf.Abs(normal.x) > Mathf.Abs(normal.y)) // It's a wall
            {
                return m_wallOffset;
            }
            else // It's a Floor / Ceiling
            {
                if (normal.y > 0f)
                {
                    return m_floorOffset;
                }
                else
                {
                    return m_ceilingOffset;
                }
            }
        }

        void UpdateCollider()
        {
            m_collider.points = m_smoothPoints.Select(vec => { return vec.ToVector2(); }).ToArray();
        }

		Vector2[] Get2DPoints(Vector3[] a_points)
		{
			Vector2[] returnPoints = new Vector2[a_points.Length];

			for (int i = 0; i < returnPoints.Length; i++)
			{
				returnPoints[i] = new Vector2(a_points[i].x, a_points[i].y);
			}

			return returnPoints;
		}

		Vector3 GetCenterPoint(Vector3[] a_points)
		{
			Vector2 maxPoints = Vector2.zero;
			Vector2 minPoints = Vector2.zero;

			for (int i = 0; i < a_points.Length; i++)
			{
				if (a_points[i].x < minPoints.x)
				{
					minPoints.x = a_points[i].x;
				}

				if (a_points[i].x > maxPoints.x)
				{
					maxPoints.x = a_points[i].x;
				}

				if (a_points[i].y < minPoints.y)
				{
					minPoints.y = a_points[i].y;
				}
				
				if (a_points[i].y > maxPoints.y)
				{
					maxPoints.y = a_points[i].y;
				}
			}

			var vec = Vector2.Lerp(minPoints, maxPoints, 0.5f);

			return new Vector3(vec.x, vec.y, 0f);
		}

        Vector3 GetNormal(Vector3 a_vecA, Vector3 a_vecB, Vector3 a_vecC)
        {
            //Debug.Log(string.Format("{0},{1},{2}", a_vecA, a_vecB, a_vecC));

            Vector3 one = a_vecA - a_vecB;
            Vector3 two = a_vecA - a_vecC;

            return Vector3.Cross(one, two);
        }

        Vector2 Rotate(Vector2 a_point, float a_angle)
        {
            float c = Mathf.Cos(a_angle * Mathf.Deg2Rad);
            float s = Mathf.Sin(a_angle * Mathf.Deg2Rad);
            return new Vector2(
                a_point.x * c - a_point.y * s,
                a_point.x * s + a_point.y * c);
        }

        List<Vector3> GenerateTrimPoints(Vector3[] a_points, Vector2 a_normal, Vector2 a_lastSegmentNormal, Vector2 a_nextSegmentNormal)
        {
            var pointList = new List<Vector3>();

            if (Mathf.Abs(a_normal.x) > Mathf.Abs(a_normal.y)) // It's a wall
            {
                if (a_normal.x > 0f)
                {
                    // Get Every Second Point (The Front Ones) and add new points to create a cap
                    for (int i = 1; i < a_points.Length; i += 2)
                    {
                        if (i == 1)
                        {
                            pointList.Add(a_points[i] + new Vector3(m_trimOffset, m_trimOffset, m_trimSize - m_trimOffset));

                            var centerPoint = a_points[i] + new Vector3(m_trimOffset, m_trimOffset, -m_trimOffset);

                            pointList.Add(centerPoint);

                            var frontPoint = a_points[i] + new Vector3(-(m_trimSize - m_trimOffset), m_trimOffset, -m_trimOffset);
                            frontPoint = Rotate(frontPoint - centerPoint, -Mathf.Atan2(a_normal.x, a_normal.y) * Mathf.Rad2Deg + 90f).ToVector3() + centerPoint;
                            
                            pointList.Add(frontPoint);
                        }
                        else if (i == a_points.Length - 1)
                        {
                            var endNormal = GetTrimEndNormal(a_points);

                            pointList.Add(a_points[i] + new Vector3(m_trimOffset, -m_trimOffset, m_trimSize - m_trimOffset));

                            var centerPoint = a_points[i] + new Vector3(m_trimOffset, -m_trimOffset, -m_trimOffset);

                            pointList.Add(centerPoint);

                            var frontPoint = a_points[i] + new Vector3(-(m_trimSize - m_trimOffset), m_trimOffset, -m_trimOffset);

                            frontPoint = Rotate(frontPoint - centerPoint, -Mathf.Atan2(endNormal.x, endNormal.y) * Mathf.Rad2Deg + 90f).ToVector3() + centerPoint;
                            
                            pointList.Add(frontPoint);
                        }
                        else
                        {
                            var nextNormal = GetNormal(a_points[i - 1], a_points[i], a_points[i + 2]).normalized;
                            var prevNormal = -GetNormal(a_points[i - 3], a_points[i - 1], a_points[i]).normalized;
                            var averageNormal = new Vector2(1f, (nextNormal.y + prevNormal.y) * 0.5f);

                            pointList.Add(a_points[i] + new Vector3(m_trimOffset, 0f, m_trimSize - m_trimOffset));

                            var centerPoint = a_points[i] + new Vector3(m_trimOffset, 0f, -m_trimOffset);

                            pointList.Add(centerPoint);

                            var frontPoint = a_points[i] + new Vector3(-(m_trimSize - m_trimOffset), 0f, - m_trimOffset);
                            frontPoint = Rotate(frontPoint - centerPoint, Mathf.Atan2(averageNormal.x, averageNormal.y) * Mathf.Rad2Deg - 90f).ToVector3() + centerPoint;

                            pointList.Add(frontPoint);
                        }
                    }
                }
                else
                {
                    // Get Every Second Point (The Front Ones) and add new points to create a cap
                    for (int i = 1; i < a_points.Length; i += 2)
                    {
                        if (i == 1)
                        {
                            pointList.Add(a_points[i] + new Vector3(-m_trimOffset, -m_trimOffset, m_trimSize - m_trimOffset));

                            var centerPoint = a_points[i] + new Vector3(-m_trimOffset, -m_trimOffset, -m_trimOffset);

                            pointList.Add(centerPoint);

                            var frontPoint = a_points[i] + new Vector3(-m_trimOffset + m_trimSize, -m_trimOffset, -m_trimOffset);
                            frontPoint = Rotate(frontPoint - centerPoint, -Mathf.Atan2(a_normal.x, a_normal.y) * Mathf.Rad2Deg - 90f).ToVector3() + centerPoint;

                            pointList.Add(frontPoint);
                        }
                        else if (i == a_points.Length - 1)
                        {
                            var endNormal = GetTrimEndNormal(a_points);

                            pointList.Add(a_points[i] + new Vector3(-m_trimOffset, m_trimOffset, m_trimSize - m_trimOffset));

                            var centerPoint = a_points[i] + new Vector3(-m_trimOffset, m_trimOffset, -m_trimOffset);

                            pointList.Add(centerPoint);

                            var frontPoint = a_points[i] + new Vector3((m_trimSize - m_trimOffset), m_trimOffset, -m_trimOffset);
                            frontPoint = Rotate(frontPoint - centerPoint, -Mathf.Atan2(endNormal.x, endNormal.y) * Mathf.Rad2Deg - 90f).ToVector3() + centerPoint;

                            pointList.Add(frontPoint);
                        }
                        else
                        {
                            var nextNormal = GetNormal(a_points[i - 1], a_points[i], a_points[i + 2]).normalized;
                            var prevNormal = -GetNormal(a_points[i - 3], a_points[i - 1], a_points[i]).normalized;
                            var averageNormal = new Vector2(1f, (nextNormal.y + prevNormal.y) * 0.5f);

                            pointList.Add(a_points[i] + new Vector3(-m_trimOffset, 0f, m_trimSize - m_trimOffset));

                            var centerPoint = a_points[i] + new Vector3(-m_trimOffset, 0f, -m_trimOffset);

                            pointList.Add(centerPoint);

                            var frontPoint = a_points[i] + new Vector3((m_trimSize - m_trimOffset), 0f, - m_trimOffset);
                            frontPoint = Rotate(frontPoint - centerPoint, -Mathf.Atan2(averageNormal.x, averageNormal.y) * Mathf.Rad2Deg + 90f).ToVector3() + centerPoint;

                            pointList.Add(frontPoint);
                        }
                    }
                }
            }
            else // It's a Floor / Ceiling
            {
                if (a_normal.y > 0f)
                {
                    // Get Every Second Point (The Front Ones) and add new points to create a cap
                    for (int i = 1; i < a_points.Length; i += 2)
                    {
                        if (i == 1)
                        {
                            pointList.Add(a_points[i] + new Vector3(-m_trimOffset, m_trimOffset, m_trimSize - m_trimOffset + 0.01f));

                            var centerPoint = a_points[i] + new Vector3(-m_trimOffset, m_trimOffset, -m_trimOffset - 0.01f);

                            pointList.Add(centerPoint);

                            var frontPoint = a_points[i] + new Vector3(0f, -(m_trimSize - m_trimOffset), -m_trimOffset - 0.01f);
                            frontPoint = Rotate(frontPoint - centerPoint, -Mathf.Atan2(a_normal.x, a_normal.y) * Mathf.Rad2Deg).ToVector3() + centerPoint;

                            pointList.Add(frontPoint);
                        }
                        else if (i == a_points.Length - 1)
                        {
                            var endNormal = GetTrimEndNormal(a_points);

                            pointList.Add(a_points[i] + new Vector3(m_trimOffset, m_trimOffset, m_trimSize - m_trimOffset + 0.01f));

                            var centerPoint = a_points[i] + new Vector3(m_trimOffset, m_trimOffset, -m_trimOffset - 0.01f);

                            pointList.Add(centerPoint);

                            var frontPoint = a_points[i] + new Vector3(0f, -(m_trimSize - m_trimOffset), -m_trimOffset - 0.01f);
                            frontPoint = Rotate(frontPoint - centerPoint, -Mathf.Atan2(endNormal.x, endNormal.y) * Mathf.Rad2Deg).ToVector3() + centerPoint;

                            pointList.Add(frontPoint);
                        }
                        else
                        {
                            var nextNormal = -GetNormal(a_points[i - 1], a_points[i], a_points[i + 2]).normalized;
                            var prevNormal = GetNormal(a_points[i - 3], a_points[i - 1], a_points[i]).normalized;
                            var averageNormal = new Vector2((nextNormal.x + prevNormal.x) * 0.5f, 1f);

                            pointList.Add(a_points[i] + new Vector3(0f, m_trimOffset, m_trimSize - m_trimOffset + 0.01f));

                            var centerPoint = a_points[i] + new Vector3(0f, m_trimOffset, -m_trimOffset - 0.01f);

                            pointList.Add(centerPoint);

                            var frontPoint = a_points[i] + new Vector3(0f, -(m_trimSize - m_trimOffset), -m_trimOffset - 0.01f);
                            frontPoint = Rotate(frontPoint - centerPoint, -Mathf.Atan2(averageNormal.x, averageNormal.y) * Mathf.Rad2Deg).ToVector3() + centerPoint;

                            pointList.Add(frontPoint);
                        }
                    }
                }
                else
                {
                    // Get Every Second Point (The Front Ones) and add new points to create a cap
                    for (int i = 1; i < a_points.Length; i += 2)
                    {
                        if (i == 1)
                        {
                            pointList.Add(a_points[i] + new Vector3(m_trimOffset, -m_trimOffset, m_trimSize - m_trimOffset + 0.01f));

                            var centerPoint = a_points[i] + new Vector3(m_trimOffset, -m_trimOffset, -m_trimOffset - 0.01f);

                            pointList.Add(centerPoint);

                            var frontPoint = a_points[i] + new Vector3(m_trimOffset, (m_trimSize + m_trimOffset), -m_trimOffset - 0.1f);
                            frontPoint = Rotate(frontPoint - centerPoint, -Mathf.Atan2(a_normal.x, a_normal.y) * Mathf.Rad2Deg + 180f).ToVector3() + centerPoint;

                            pointList.Add(frontPoint);
                        }
                        else if (i == a_points.Length - 1)
                        {
                            var endNormal = GetTrimEndNormal(a_points);

                            pointList.Add(a_points[i] + new Vector3(-m_trimOffset, -m_trimOffset, m_trimSize - m_trimOffset + 0.01f));

                            var centerPoint = a_points[i] + new Vector3(-m_trimOffset, -m_trimOffset, -m_trimOffset - 0.01f);

                            pointList.Add(centerPoint);

                            var frontPoint = a_points[i] + new Vector3(-m_trimOffset, (m_trimSize - m_trimOffset), -m_trimOffset - 0.01f);
                            frontPoint = Rotate(frontPoint - centerPoint, -Mathf.Atan2(endNormal.x, endNormal.y) * Mathf.Rad2Deg + 180f).ToVector3() + centerPoint;

                            pointList.Add(frontPoint);
                        }
                        else
                        {
                            var nextNormal = -GetNormal(a_points[i - 1], a_points[i], a_points[i + 2]).normalized;
                            var prevNormal = GetNormal(a_points[i - 3], a_points[i - 1], a_points[i]).normalized;
                            var averageNormal = new Vector2((nextNormal.x + prevNormal.x) * 0.5f, 1f);

                            pointList.Add(a_points[i] + new Vector3(0f, -m_trimOffset, m_trimSize - m_trimOffset + 0.01f));

                            var centerPoint = a_points[i] + new Vector3(0f, -m_trimOffset, -m_trimOffset - 0.01f);

                            pointList.Add(centerPoint);

                            var frontPoint = a_points[i] + new Vector3(0f, (m_trimSize - m_trimOffset), -m_trimOffset - 0.01f);
                            frontPoint = Rotate(frontPoint - centerPoint, Mathf.Atan2(averageNormal.x, averageNormal.y) * Mathf.Rad2Deg).ToVector3() + centerPoint;

                            pointList.Add(frontPoint);
                        }
                    }
                }
            }

            return pointList;
        }

        Mesh CreateTrimMesh(Vector3[] a_points, Vector3[] a_lastSegmentPoints, Vector3[] a_nextSegmentPoints)
        {
            Vector2 lastSegmentNormal = GetTrimEndNormal(a_lastSegmentPoints);
            Vector2 trimNormal = GetTrimNormal(a_points);
            Vector2 nextSegmentNormal = GetTrimNormal(a_nextSegmentPoints);

            var pointList = GenerateTrimPoints(a_points, trimNormal, lastSegmentNormal, nextSegmentNormal);

            // Create Triangles
            var triangleList = new List<int>();

            for (int i = 0; i < pointList.Count - 4; i += 3)
            {
                var index = i;

                for (int j = 0; j < 2; j++)
                {
                    triangleList.Add(index);
                    triangleList.Add(index + 3);
                    triangleList.Add(index + 1);

                    triangleList.Add(index + 1);
                    triangleList.Add(index + 3);
                    triangleList.Add(index + 4);

                    index++;
                }
            }

            var distanceList = new List<float>();
            distanceList.Add(0f);
            for (int i = 0; i < pointList.Count - 3; i += 3)
            {
                distanceList.Add(Vector3.Distance(pointList[i], pointList[i + 3]));
            }

            float distance = 0f;
            int distanceIndex = 0;
            var uv = new List<Vector2>();
            // Create UV's
            for (int i = 0; i < pointList.Count; i += 3)
            {
                distance += (distanceList[distanceIndex] * 0.5f);

                uv.Add(new Vector2(distance, 1f));
                uv.Add(new Vector2(distance, 0.5f));
                uv.Add(new Vector2(distance, 0f));

                
                distanceIndex++;
            }

            var mesh = new Mesh();
            mesh.vertices = pointList.ToArray();
            mesh.triangles = triangleList.ToArray();
            mesh.uv = uv.ToArray();
            mesh.RecalculateNormals();

            return mesh;
        }

        Vector2 GetTrimNormal(Vector3[] a_points)
        {
            return -GetNormal(a_points[0], a_points[1], a_points[2]).normalized;
        }

        Vector2 GetTrimEndNormal(Vector3[] a_points)
        {
            return GetNormal(a_points[a_points.Length - 3], a_points[a_points.Length - 2], a_points[a_points.Length - 1]).normalized;
        }

        Material GetTrimMaterial(Vector3[] a_points)
        {
            var normal = -GetNormal(a_points[0], a_points[1], a_points[2]).normalized;

            if (Mathf.Abs(normal.x) > Mathf.Abs(normal.y)) // It's a wall
            {
                return WallTrimMaterial;
            }
            else // It's a Floor / Ceiling
            {
                if (normal.y > 0f)
                {
                    return FloorTrimMaterial;
                }
                else
                {
                    return CeilingTrimMaterial;
                }
            }
        }

        void AddSmoothPoints()
        {
            m_smoothPoints = new List<Vector3>();

            if (!m_isSmooth)
            {
                m_smoothPoints = m_terrainPoints;
            }
            else
            {

                for (int i = 0; i < m_terrainPoints.Count; i++)
                {
                    var previous = i - 1;
                    if (previous < 0)
                    {
                        previous += m_terrainPoints.Count;
                    }

                    var next = i + 1;
                    if (next >= m_terrainPoints.Count)
                    {
                        next -= m_terrainPoints.Count;
                    }



                    var prevPad = m_terrainPoints[i] - (m_terrainPoints[i] - m_terrainPoints[previous]).normalized;// * 1f;
                    var nextPad = m_terrainPoints[i] + (m_terrainPoints[next] - m_terrainPoints[i]).normalized;// * 0.5f;

                    var prevMiddle = Vector3.Lerp(prevPad, nextPad, 0.25f);
                    var middlePoint = Vector3.Lerp(prevPad, nextPad, 0.5f);
                    var nextMiddle = Vector3.Lerp(prevPad, nextPad, 0.75f);

                    //for (int j = 1; j > 0; j--)
                    //{
                    //    m_smoothPoints.Add(m_terrainPoints[i] - (m_terrainPoints[i] - m_terrainPoints[previous]).normalized * (0.3f * j));
                    //}

                    //for (int j = 0; j < 10; j++)
                    //{
                    //    m_smoothPoints.Add(GetPointOnCurve(prevPad, nextPad, m_terrainPoints[i], j * 0.5f));
                    //}


                    m_smoothPoints.Add(prevPad);
                    m_smoothPoints.Add(GetPointOnCurve(prevPad, nextPad, m_terrainPoints[i], 0.25f));
                    m_smoothPoints.Add(GetPointOnCurve(prevPad, nextPad, m_terrainPoints[i], 0.5f));
                    m_smoothPoints.Add(GetPointOnCurve(prevPad, nextPad, m_terrainPoints[i], 0.75f));
                    m_smoothPoints.Add(nextPad);

                    //for (int j = 1; j <= 1; j++)
                    //{
                    //    m_smoothPoints.Add(m_terrainPoints[i] + (m_terrainPoints[next] - m_terrainPoints[i]).normalized * (0.3f * j));
                    //}
                }
            }
        }

        Vector3 GetPointOnCurve(Vector3 a_start, Vector3 a_end, Vector3 a_control, float a_time)
        {
            var CurveX = (((1 - a_time) * (1 - a_time)) * a_start.x) + (2 * a_time * (1 - a_time) * a_control.x) + ((a_time * a_time) * a_end.x);
            var CurveY = (((1 - a_time) * (1 - a_time)) * a_start.y) + (2 * a_time * (1 - a_time) * a_control.y) + ((a_time * a_time) * a_end.y);
            return new Vector3(CurveX, CurveY, 0);
        }

        void AddPoints(int index)
        {
            //Clamp to allow looping
            Vector3 p0 = m_terrainPoints[ClampListPos(index - 1)];
            Vector3 p1 = m_terrainPoints[index];
            Vector3 p2 = m_terrainPoints[ClampListPos(index + 1)];
            Vector3 p3 = m_terrainPoints[ClampListPos(index + 2)];

            //Just assign a tmp value to this
            Vector3 lastPos = Vector3.zero;

            //t is always between 0 and 1 and determines the resolution of the spline
            //0 is always at p1
            for (float t = 0; t < 1; t += 0.5f)
            {
                //Find the coordinates between the control points with a Catmull-Rom spline
                Vector3 newPos = ReturnCatmullRom(t, p0, p1, p2, p3);

                //Cant display anything the first iteration
                if (t == 0)
                {
                    lastPos = newPos;
                    continue;
                }

                //Gizmos.DrawLine(lastPos, newPos);
                lastPos = newPos;

                m_smoothPoints.Add(newPos);
            }
        }

        Vector3 ReturnCatmullRom(float t, Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3)
        {
            Vector3 a = 0.5f * (2f * p1);
            Vector3 b = 0.5f * (p2 - p0);
            Vector3 c = 0.5f * (2f * p0 - 5f * p1 + 4f * p2 - p3);
            Vector3 d = 0.5f * (-p0 + 3f * p1 - 3f * p2 + p3);

            Vector3 pos = a + (b * t) + (c * t * t) + (d * t * t * t);

            return pos;
        }

        //Clamp the list positions to allow looping
        //start over again when reaching the end or beginning
        int ClampListPos(int pos)
        {
            if (pos < 0)
            {
                pos = m_terrainPoints.Count - 1;
            }

            if (pos > m_terrainPoints.Count)
            {
                pos = 1;
            }
            else if (pos > m_terrainPoints.Count - 1)
            {
                pos = 0;
            }

            return pos;
        }
        #endregion
    }
}
