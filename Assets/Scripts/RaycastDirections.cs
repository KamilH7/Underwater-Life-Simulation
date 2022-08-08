using UnityEngine;

public static class RaycastDirections
{
    public static Vector3[] directions;

    static RaycastDirections()
    {
        directions = new Vector3[600];
        float goldenRatio = 1.61803399f;
        float angleIncrement = Mathf.PI * 2f * goldenRatio;

        for (int i = 0; i < directions.Length; i++)
        {
            float angle = (float) i / directions.Length;
            float latitude = Mathf.Acos(1 - 2 * angle);
            float logintude = angleIncrement * i;

            Vector3 pointOnSphere = Vector3.zero;

            pointOnSphere.x = Mathf.Sin(latitude) * Mathf.Cos(logintude);
            pointOnSphere.y = Mathf.Sin(latitude) * Mathf.Sin(logintude);
            pointOnSphere.z = Mathf.Cos(latitude);

            directions[i] = pointOnSphere;
        }
    }

}