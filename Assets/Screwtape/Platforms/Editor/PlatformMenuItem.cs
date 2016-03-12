using UnityEngine;
using UnityEditor;
using System.Collections;
using Screwtape;

public class PlatformMenuItem
{
    [MenuItem("GameObject/Create Other/Platform", priority = 0)]
    public static void CreateNewPlatform()
    {
        var go = new GameObject("New Platform");
        var platform = go.AddComponent<Platform>();
        go.transform.position = GetViewCenterWorldPos();
        Selection.activeGameObject = go;

        //var assets = AssetDatabase.FindAssets("DefaultRoom t:Material");
        //var defaultMaterial = AssetDatabase.LoadAssetAtPath(AssetDatabase.GUIDToAssetPath(assets[0]), typeof(Material)) as Material;

        //platform.FaceMaterial = defaultMaterial;
        //platform.FloorMaterial = defaultMaterial;
        //platform.CeilingMaterial = defaultMaterial;
        //platform.WallMaterial = defaultMaterial;

        //platform.UpdateMesh();

    }

    private static Vector3 GetViewCenterWorldPos()
    {
        Ray worldRay = SceneView.lastActiveSceneView.camera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 1.0f));
        Plane groundPlane = new Plane(Vector3.up, Vector3.zero);
        float distanceToGround; groundPlane.Raycast(worldRay, out distanceToGround);
        Vector3 worldPos = worldRay.GetPoint(distanceToGround);
        worldPos.z = 0f;
        return worldPos;
    }
}
