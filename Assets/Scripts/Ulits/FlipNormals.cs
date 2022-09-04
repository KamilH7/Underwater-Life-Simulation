using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;

public class FlipNormals : MonoBehaviour
{
    public bool removeExistingColliders = true;

    #region Public Methods

    [Button]
    private void CreateInvertedMeshCollider()
    {
        if (removeExistingColliders)
        {
            RemoveExistingColliders();
        }

        InvertMesh();

        gameObject.AddComponent<MeshCollider>();
    }

    #endregion

    #region Private Methods

    private void RemoveExistingColliders()
    {
        Collider[] colliders = GetComponents<Collider>();

        for (int i = 0; i < colliders.Length; i++)
        {
            DestroyImmediate(colliders[i]);
        }
    }

    [Button]
    private void InvertMesh()
    {
        Mesh mesh = GetComponent<MeshFilter>().mesh;
        mesh.triangles = mesh.triangles.Reverse().ToArray();
    }

    #endregion
}