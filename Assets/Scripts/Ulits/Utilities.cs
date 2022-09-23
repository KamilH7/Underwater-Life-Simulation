using UnityEngine;

public static class Utilities
{
    public static Vector3 ClampMagnitude(this Vector3 inputVector, float maxMagnitue, float minMagnitude)
    {
        double inputMagnitude = inputVector.magnitude;

        if (inputMagnitude > maxMagnitue)
        {
            return inputVector.normalized * maxMagnitue;
        }
        
        if (inputMagnitude < minMagnitude)
        {
            return inputVector.normalized * minMagnitude;
        }
         
        return inputVector;
    }
}
