using UnityEngine;

public struct AgentData
{
    public Vector3 position;
    public Vector3 moveVector;

    public Vector3 avgPosition;
    public Vector3 avgMoveVector;
    public Vector3 avgNeighbourAvoidanceVector;
    public Vector3 avgPredatorAvoidanceVector;

    #region Public Properties

    public static int Size => sizeof(float) * 3 * 6;

    #endregion
}