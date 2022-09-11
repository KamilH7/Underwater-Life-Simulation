using UnityEngine;

public struct AgentData
{
    public Vector3 position;
    public Vector3 displacementVector;

    public Vector3 avgPosition;
    public Vector3 avgDisplacementVector;
    public Vector3 avgAvoidanceDisplacementVector;

    #region Public Properties

    public static int Size => sizeof(float) * 3 * 5;

    #endregion
}