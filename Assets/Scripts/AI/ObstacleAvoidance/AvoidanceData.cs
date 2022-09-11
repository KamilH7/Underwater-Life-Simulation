using UnityEngine;

public class AvoidanceData
{
    private Vector3 ClearDirection { get; }
    private float AvoidanceWeight { get; }

    #region Constructors

    public AvoidanceData(Vector3 clearDirection, float distanceFromObstacle, float collisionDetectDistance)
    {
        ClearDirection = clearDirection;
        AvoidanceWeight = GetAvoidanceWeight(distanceFromObstacle, collisionDetectDistance);
    }

    #endregion

    #region Public Methods

    public Vector3 GetAvoidanceVector(Vector3 initialMovementVector)
    {
        Vector3 clearVector = ClearDirection * AvoidanceWeight;
        Vector3 initialVector = initialMovementVector.normalized * (1 - AvoidanceWeight);
        Vector3 avoidanceDirection = clearVector + initialVector;

        return avoidanceDirection * initialMovementVector.magnitude;
    }

    #endregion

    #region Private Methods

    private float GetAvoidanceWeight(float distanceFromObstacle, float collisionDetectDistance)
    {
        float minDistance = collisionDetectDistance;

        return Mathf.Abs(1 - distanceFromObstacle / minDistance);
    }

    #endregion
}