using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

namespace Screwtape
{
    [CustomEditor(typeof(Platform))]
    [CanEditMultipleObjects]
    public class PlatformEditor : Editor
    {
        Dictionary<int, int> m_controls;
        bool deleteMode = false;
        bool snap = false;
        bool slice = false;

        List<Vector3> m_splitPoints;
        List<int> m_splitPointIndex;

        public override void OnInspectorGUI()
        {
            Platform platform = (Platform)target;
            
            GUI.color = Color.green;

            if (GUILayout.Button("Update Platform"))
            {
                platform.UpdateMesh();
                SceneView.RepaintAll();
            }

            EditorGUILayout.Space();
            EditorGUILayout.Space();

            GUI.color = Color.white;

            platform.GenerateCollider = EditorGUILayout.Toggle("Enable Collider", platform.GenerateCollider);
            platform.IsSmooth = EditorGUILayout.Toggle("Smooth", platform.IsSmooth);

            EditorGUILayout.Space();

            platform.Depth = EditorGUILayout.FloatField("Depth", platform.Depth);

            EditorGUILayout.Space();
            GUILayout.BeginVertical("box");
            platform.FaceMaterial = (Material)EditorGUILayout.ObjectField("Face Material", platform.FaceMaterial, typeof(Material), false);
            platform.FaceScale = EditorGUILayout.FloatField("Face Scale", platform.FaceScale);
            platform.FaceRotation = EditorGUILayout.FloatField("Face Rotation", platform.FaceRotation);
            GUILayout.EndVertical();

            EditorGUILayout.Space();

            GUILayout.BeginVertical("box");
            platform.WallMaterial = (Material)EditorGUILayout.ObjectField("Wall Material", platform.WallMaterial, typeof(Material), false);
            platform.WallScale = EditorGUILayout.FloatField("Wall Texture Scale", platform.WallScale);
            platform.WallOffset = EditorGUILayout.Vector2Field("Wall Texture Offset", platform.WallOffset);
            GUILayout.EndVertical();

            EditorGUILayout.Space();

            GUILayout.BeginVertical("box");
            platform.FloorMaterial = (Material)EditorGUILayout.ObjectField("Floor Material", platform.FloorMaterial, typeof(Material), false);
            platform.FloorScale = EditorGUILayout.FloatField("Floor Texture Scale", platform.FloorScale);
            platform.FloorOffset = EditorGUILayout.Vector2Field("Floor Texture Offset", platform.FloorOffset);
            GUILayout.EndVertical();

            EditorGUILayout.Space();

            GUILayout.BeginVertical("box");
            platform.CeilingMaterial = (Material)EditorGUILayout.ObjectField("Ceiling Material", platform.CeilingMaterial, typeof(Material), false);
            platform.CeilingScale = EditorGUILayout.FloatField("Ceiling Texture Scale", platform.CeilingScale);
            platform.CeilingOffset = EditorGUILayout.Vector2Field("Ceiling Texture Offset", platform.CeilingOffset);
            GUILayout.EndVertical();

            EditorGUILayout.Space();
            EditorGUILayout.Space();

            GUILayout.BeginVertical("box");
            platform.DisplayTrim = EditorGUILayout.Toggle("Show Trim", platform.DisplayTrim);

            if (platform.DisplayTrim)
            {
                platform.TrimOffset = EditorGUILayout.FloatField("Trim Offset", platform.TrimOffset);
                platform.TrimSize = EditorGUILayout.FloatField("Trim Size", platform.TrimSize);
                platform.WallTrimMaterial = (Material)EditorGUILayout.ObjectField("Wall Trim Material", platform.WallTrimMaterial, typeof(Material), false);
                platform.FloorTrimMaterial = (Material)EditorGUILayout.ObjectField("Floor Trim Material", platform.FloorTrimMaterial, typeof(Material), false);
                platform.CeilingTrimMaterial = (Material)EditorGUILayout.ObjectField("Ceiling Trim Material", platform.CeilingTrimMaterial, typeof(Material), false);
                platform.TrimSortingLayer = EditorGUILayout.TextField("Trim Sorting Layer", platform.TrimSortingLayer);
                platform.TrimSortingOrder = EditorGUILayout.IntField("Trim Sorting Order", platform.TrimSortingOrder);
            }

            GUILayout.EndVertical();

            EditorGUILayout.Space();
            EditorGUILayout.Space();
           
            GUI.color = Color.red;

            if (GUILayout.Button("Reset Platform"))
            {
                platform.ResetPlatform();
                SceneView.RepaintAll();
            }

            GUI.color = Color.white;

            //base.OnInspectorGUI();
        }

        void OnSceneGUI()
        {
            deleteMode = false;
            snap = false;
            slice = false;

            if (Event.current.shift)
            {
                deleteMode = true;
            }

            if (Event.current.control)
            {
                snap = true;
            }

            if (Event.current.alt)
            {
                slice = true;
            }
            else
            {
                m_splitPoints = new List<Vector3>();
                m_splitPointIndex = new List<int>();
            }

            if (m_controls == null)
            {
                m_controls = new Dictionary<int, int>();
            }

            Platform platform = (Platform)target;

            int newPointIndex = 0;
            Vector3 newPointPosition = Vector3.zero;
            bool addNewPoint = false;
            bool removePoint = false;
            int oldPointIndex = 0;

            bool isDirty = false;

            if (slice)
            {
                if (m_splitPoints == null)
                {
                    m_splitPoints = new List<Vector3>();
                    m_splitPointIndex = new List<int>();
                }

                Handles.BeginGUI();
                EditorGUI.LabelField(new Rect(0f, 0f, Screen.width, 50f), string.Format("Platform ({0}): Slice Platform", platform.name));
                Handles.EndGUI();

                for (int i = 0; i < platform.Points.Count; i++)
                {
                    Vector3 pos = platform.transform.TransformPoint(platform.Points[i]);

                    if (Handles.Button(pos, Quaternion.identity, 0.5f, 0.5f, Handles.RectangleCap))
                    {
                        if (m_splitPoints.Count >= 2)
                        {
                            m_splitPoints[1] = pos;
                            m_splitPointIndex[1] = i;
                        }
                        else
                        {
                            m_splitPoints.Add(pos);
                            m_splitPointIndex.Add(i);
                        }
                    }
                }


                if (m_splitPoints.Count == 2)
                {
                    Handles.DrawLine(m_splitPoints[0], m_splitPoints[1]);

                    Handles.BeginGUI();

                    if (GUI.Button(new Rect(0f, 50f, 200f, 40f), "Slice"))
                    {
                        SlicePlatform(m_splitPointIndex[0], m_splitPointIndex[1], platform.Points);
                        return;
                    }

                    Handles.EndGUI();              
                }

                SceneView.RepaintAll();
            }
            else
            {
                if (platform.Points != null)
                {
                    for (int i = 0; i < platform.Points.Count; i++)
                    {
                        Handles.color = Color.green;

                        Vector3 point = platform.Points[i];

                        if (deleteMode)
                        {
                            Handles.BeginGUI();
                            EditorGUI.LabelField(new Rect(0f, 0f, Screen.width, 50f), string.Format("Platform ({0}): Delete Points", platform.name));
                            Handles.EndGUI();

                            Vector3 delPos = platform.transform.TransformPoint(platform.Points[i]);

                            if (Handles.Button(delPos, Quaternion.identity, 0.5f, 0.5f, Handles.RectangleCap))
                            {
                                removePoint = true;
                                oldPointIndex = i;
                            }
                        }
                        else
                        {
                            Handles.BeginGUI();
                            EditorGUI.LabelField(new Rect(0f, 0f, Screen.width, 50f), string.Format("Platform ({0}): Add and Move Points", platform.name));
                            Handles.EndGUI();

                            platform.Points[i] = platform.transform.InverseTransformPoint(Handles.FreeMoveHandle(platform.transform.TransformPoint(platform.Points[i]),
                                Quaternion.identity, 0.5f, new Vector3(0.25f, 0.25f, 0f), (controlId, position, rotation, size) =>
                                {
                                    if (m_controls.ContainsKey(controlId))
                                    {
                                        m_controls[controlId] = i;
                                    }
                                    else
                                    {
                                        m_controls.Add(controlId, i);
                                    }

                                    Handles.CircleCap(controlId, position, rotation, size);
                                }));

                            if (point != platform.Points[i])
                            {
                                point = platform.Points[i];
                                point.z = 0f;

                                if (snap)
                                {
                                    point.x = MathUtils.Round(point.x, 0.5f);
                                    point.y = MathUtils.Round(point.y, 0.5f);
                                }

                                platform.Points[i] = point;
                                isDirty = true;
                            }

                            Handles.color = Color.white;

                            if (i < platform.Points.Count - 1)
                            {
                                Vector3 position = Vector3.Lerp(platform.transform.TransformPoint(platform.Points[i]),
                                    platform.transform.TransformPoint(platform.Points[i + 1]), 0.5f);

                                if (Handles.Button(position, Quaternion.identity, 0.5f, 0.5f, Handles.RectangleCap))
                                {
                                    addNewPoint = true;
                                    newPointIndex = i + 1;
                                    newPointPosition = position;
                                }

                                Handles.DrawLine(platform.transform.TransformPoint(platform.Points[i]),
                                    platform.transform.TransformPoint(platform.Points[i + 1]));
                            }
                            else
                            {
                                Vector3 position = Vector3.Lerp(platform.transform.TransformPoint(platform.Points[i]),
                                    platform.transform.TransformPoint(platform.Points[0]), 0.5f);

                                if (Handles.Button(position, Quaternion.identity, 0.5f, 0.5f, Handles.RectangleCap))
                                {
                                    addNewPoint = true;
                                    newPointIndex = i + 1;
                                    newPointPosition = position;
                                }

                                Handles.DrawLine(platform.transform.TransformPoint(platform.Points[i]),
                                    platform.transform.TransformPoint(platform.Points[0]));
                            }
                        }
                    }
                }
            }

            if (addNewPoint)
            {
                platform.Points.Insert(newPointIndex, 
                    platform.transform.InverseTransformPoint(newPointPosition));

                isDirty = true;
            }

            if (removePoint)
            {
                platform.Points.RemoveAt(oldPointIndex);
                isDirty = true;
            }

            if (isDirty)
            {
                platform.UpdateMesh();
            }
        }

        void SlicePlatform(int firstIndex, int secondIndex, List<Vector3> a_points)
        {
            int one = secondIndex > firstIndex ? firstIndex : secondIndex;
            int two = secondIndex > firstIndex ? secondIndex : firstIndex;

            List<Vector3> firstSet = new List<Vector3>();
            List<Vector3> secondSet = new List<Vector3>();

            // Create the first set
            for (int i = 0; i < a_points.Count; i++)
            {
                if (i <= one)
                {
                    firstSet.Add(a_points[i]);
                }
                else if (i >= two)
                {
                    firstSet.Add(a_points[i]);
                }
            }

            // Create the second set
            for (int i = one; i <= two; i++)
            {
                secondSet.Add(a_points[i]);
            }

            Platform platform = (Platform)target;

            var platformOne = new GameObject(platform.name + "_SplitOne").AddComponent<Platform>();
            platformOne.transform.position = platform.transform.position;

            platformOne.FaceMaterial = platform.FaceMaterial;
            platformOne.FloorMaterial = platform.FloorMaterial;
            platformOne.WallMaterial = platform.WallMaterial;
            platformOne.CeilingMaterial = platform.CeilingMaterial;

            platformOne.GenerateCollider = platform.GenerateCollider;
            platformOne.IsSmooth = platform.IsSmooth;
            platformOne.Depth = platform.Depth;

            platformOne.DisplayTrim = false;

            platformOne.SetPoints(firstSet);


            var platformTwo = new GameObject(platform.name + "_SplitTwo").AddComponent<Platform>();
            platformTwo.transform.position = platform.transform.position;

            platformTwo.FaceMaterial = platform.FaceMaterial;
            platformTwo.FloorMaterial = platform.FloorMaterial;
            platformTwo.WallMaterial = platform.WallMaterial;
            platformTwo.CeilingMaterial = platform.CeilingMaterial;

            platformTwo.GenerateCollider = platform.GenerateCollider;
            platformTwo.IsSmooth = platform.IsSmooth;
            platformTwo.Depth = platform.Depth;

            platformTwo.DisplayTrim = false;

            platformTwo.SetPoints(secondSet);

            platform.gameObject.SetActive(false);

            Selection.activeGameObject = platformOne.gameObject;
        }

        #region Fields
        #endregion

        #region Unity Callbacks
        #endregion

        #region Public Methods
        #endregion

        #region Private Methods

        #endregion
    }
}
