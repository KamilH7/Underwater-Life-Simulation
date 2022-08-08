using Sirenix.OdinInspector;
using UnityEngine;

public class FlipNormals : MonoBehaviour
{
    #region Private Methods

    [Button]
    private void Flip()
    {
        Mesh mesh = GetComponent<MeshFilter>().mesh;
        Vector3[] normals = mesh.normals;

        for (int i = 0; i < normals.Length; i++)
        {
            normals[i] = -1 * normals[i];
        }

        mesh.normals = normals;

        for (int i = 0; i < mesh.subMeshCount; i++)
        {
            int[] tris = mesh.GetTriangles(i);

            for (int j = 0; j < tris.Length; j += 3)
            {
                (tris[j], tris[j + 1]) = (tris[j + 1], tris[j]);
            }

            mesh.SetTriangles(tris, i);
        }
    }

    #endregion
}