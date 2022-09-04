using UnityEngine;

public class AvoidanceData
{
    public Vector3 ClearDirection { get; }
    public float AvoidanceWeight { get; }

    public AvoidanceData(Vector3 clearDirection, float avoidanceWeight)
    {
        ClearDirection = clearDirection;
        AvoidanceWeight = avoidanceWeight;
    }

    public Vector3 GetAvoidanceVector(Vector3 initialMovementVector)
    {
        return ClearDirection * AvoidanceWeight + initialMovementVector * (1 - AvoidanceWeight);
    }
}
