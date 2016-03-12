using UnityEngine;
using UnityEditor;
using System.Collections;

namespace Screwtape
{
	[CustomEditor(typeof(TextureStrip))]
	public class TextureStripEditor : Editor
	{
		public override void OnInspectorGUI()
		{
			TextureStrip strip = (TextureStrip)target;

			GUI.color = Color.green;

			if (GUILayout.Button("Update Mesh"))
			{
				strip.UpdateMesh();
				SceneView.RepaintAll();
			}

			GUI.color = Color.white;

			strip.Height = EditorGUILayout.FloatField("Strip Height", strip.Height);
			strip.Scale = EditorGUILayout.FloatField("Strip Scale", strip.Scale);
			strip.StripMaterial = (Material)EditorGUILayout.ObjectField("Strip Material", strip.StripMaterial, typeof(Material), false);

			GUI.color = Color.red;

			if (GUILayout.Button("Reset Mesh"))
			{
				strip.ResetMesh();
				SceneView.RepaintAll();
			}

			GUI.color = Color.white;
		}

		void OnSceneGUI()
		{
			TextureStrip strip = (TextureStrip)target;

			bool addNewPoint = false;
			bool isDirty = false;
			int newPointIndex = 0;
			Vector3 newPointPosition = Vector3.zero;

			if (strip.Points != null)
			{
				for (int i = 0; i < strip.Points.Count; i++)
				{
					Handles.color = Color.green;

					Vector3 point = strip.Points[i];

					strip.Points[i] = strip.transform.InverseTransformPoint(Handles.FreeMoveHandle(strip.transform.TransformPoint(strip.Points[i]), 
					                       														   Quaternion.identity, 
					                       														   0.2f, 
					                       														   new Vector3(0.25f, 0.25f, 0f), 
					                       														   Handles.CircleCap));

					Handles.color = Color.white;

					if (point != strip.Points[i])
					{
						point = strip.Points[i];
						point.z = 0f;

						strip.Points[i] = point;
						isDirty = true;
					}

					if (i < strip.Points.Count - 1)
					{
						Vector3 position = Vector3.Lerp(strip.transform.TransformPoint(strip.Points[i]),
						                                strip.transform.TransformPoint(strip.Points[i + 1]), 0.5f);
						
						if (Handles.Button(position, Quaternion.identity, 0.15f, 0.2f, Handles.RectangleCap))
						{
							addNewPoint = true;
							newPointIndex = i + 1;
							newPointPosition = position;
						}
						
						Handles.DrawLine(strip.transform.TransformPoint(strip.Points[i]), 
						                 strip.transform.TransformPoint(strip.Points[i + 1]));
					}
				}
			}

			if (addNewPoint)
			{
				strip.Points.Insert(newPointIndex, 
				                    strip.transform.InverseTransformPoint(newPointPosition));
				
				isDirty = true;
			}
			
			if (isDirty)
			{
				strip.UpdateMesh();
			}
		}
	}
}
