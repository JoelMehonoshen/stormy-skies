using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class SurfaceTextureModifier : MonoBehaviour
{
    [SerializeField]
    private float m_scale = 1f;

    [SerializeField]
    private float m_rotation;

    [SerializeField]
    private Vector2 m_offset;

    private Vector2[] initalUV;

    private MeshFilter m_meshFilter;

    void OnValidate()
    {
        if (m_meshFilter == null)
        {
            m_meshFilter = GetComponent<MeshFilter>();
            initalUV = m_meshFilter.sharedMesh.uv;
        }

        var mesh = m_meshFilter.sharedMesh;

        var uv = mesh.uv;

        for (int i = 0; i < uv.Length; i++)
        {
            uv[i] = Rotate(initalUV[i], m_rotation) * m_scale + m_offset;
        }

        mesh.uv = uv;
    }

    Vector2 Rotate(Vector2 a_point, float a_angle)
    {
        float c = Mathf.Cos(a_angle * Mathf.Deg2Rad);
        float s = Mathf.Sin(a_angle * Mathf.Deg2Rad);
        return new Vector2(
            a_point.x * c - a_point.y * s,
            a_point.x * s + a_point.y * c);
    }
}
