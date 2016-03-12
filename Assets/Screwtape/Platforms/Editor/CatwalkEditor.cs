using UnityEngine;
using UnityEditor;
using System.Collections;

namespace Screwtape
{
    [CustomEditor(typeof(Catwalk))]
    public class CatwalkEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            Catwalk catwalk = (Catwalk)target;

            GUI.color = Color.green;

            if (GUILayout.Button("Update Mesh"))
            {
                catwalk.UpdateMesh();
                SceneView.RepaintAll();
            }

            GUI.color = Color.white;

            catwalk.Height = EditorGUILayout.FloatField("Catwalk Thickness", catwalk.Height);
            catwalk.Depth = EditorGUILayout.FloatField("Catwalk Depth", catwalk.Depth);
            catwalk.Scale = EditorGUILayout.FloatField("Front Texture Scale", catwalk.Scale);
            catwalk.StripMaterial = (Material)EditorGUILayout.ObjectField("Catwalk Material", catwalk.StripMaterial, typeof(Material), false);

            catwalk.MovingPlatform = EditorGUILayout.Toggle("Moving Platform", catwalk.MovingPlatform);

            GUI.color = Color.red;

            if (GUILayout.Button("Reset Mesh"))
            {
                catwalk.ResetMesh();
                SceneView.RepaintAll();
            }

            GUI.color = Color.white;
        }

        void OnSceneGUI()
        {
            Catwalk catwalk = (Catwalk)target;

            bool addNewPoint = false;
            bool isDirty = false;
            int newPointIndex = 0;
            Vector3 newPointPosition = Vector3.zero;

            if (catwalk.Points != null)
            {
                for (int i = 0; i < catwalk.Points.Count; i++)
                {
                    Handles.color = Color.green;

                    Vector3 point = catwalk.Points[i];

                    catwalk.Points[i] = catwalk.transform.InverseTransformPoint(Handles.FreeMoveHandle(catwalk.transform.TransformPoint(catwalk.Points[i]),
                                                                                                      Quaternion.identity,
                                                                                                      0.2f,
                                                                                                      new Vector3(0.25f, 0.25f, 0f),
                                                                                                      Handles.CircleCap));

                    Handles.color = Color.white;

                    if (point != catwalk.Points[i])
                    {
                        point = catwalk.Points[i];
                        point.z = 0f;

                        catwalk.Points[i] = point;
                        isDirty = true;
                    }

                    if (i < catwalk.Points.Count - 1)
                    {
                        Vector3 position = Vector3.Lerp(catwalk.transform.TransformPoint(catwalk.Points[i]),
                                                        catwalk.transform.TransformPoint(catwalk.Points[i + 1]), 0.5f);

                        if (Handles.Button(position, Quaternion.identity, 0.15f, 0.2f, Handles.RectangleCap))
                        {
                            addNewPoint = true;
                            newPointIndex = i + 1;
                            newPointPosition = position;
                        }

                        Handles.DrawLine(catwalk.transform.TransformPoint(catwalk.Points[i]),
                                         catwalk.transform.TransformPoint(catwalk.Points[i + 1]));
                    }
                }
            }

            if (addNewPoint)
            {
                catwalk.Points.Insert(newPointIndex,
                                    catwalk.transform.InverseTransformPoint(newPointPosition));

                isDirty = true;
            }

            if (isDirty)
            {
                catwalk.UpdateMesh();
            }
        }
    }
}

