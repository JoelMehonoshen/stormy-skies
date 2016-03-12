using UnityEngine;
using UnityEditor;
using System.Collections;
using Screwtape;

public class TextureStripMenuItem
{
    [MenuItem("GameObject/Create Other/Texture Strip", priority = 0)]
    public static void CreateNewPlatform()
    {
        var go = new GameObject("New Texture Strip");
        go.AddComponent<TextureStrip>();

        go.transform.position = GetViewCenterWorldPos();
        Selection.activeGameObject = go;
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
