using UnityEngine;

public interface ISpawnable
{
    #region Public Methods

    void Spawn(Vector3 position, Vector3 direction, Quaternion rotation, Transform parent, GameObject prefab);

    #endregion
}