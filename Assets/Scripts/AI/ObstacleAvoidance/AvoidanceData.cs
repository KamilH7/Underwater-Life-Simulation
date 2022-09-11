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
        float currentSpeed = initialMovementVector.magnitude;
        Vector3 moveDirection = ClearDirection * AvoidanceWeight + initialMovementVector.normalized * (1 - AvoidanceWeight);

        return moveDirection * currentSpeed;
    }

    #endregion

    #region Private Methods

    private float GetAvoidanceWeight(float distanceFromObstacle, float collisionDetectDistance)
    {
        float minDistance = collisionDetectDistance;

        return 1 - distanceFromObstacle / minDistance;
    }

    #endregion
}