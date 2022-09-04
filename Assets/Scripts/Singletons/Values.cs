using UnityEngine;
[CreateAssetMenu(fileName = "Values", menuName = "Singletons/Values")]
public class Values : SingletonScriptableObject<Values>
{
    [field: SerializeField]
    public LayerMask ObstacleLayer { get; private set; }

    private Vector3[] goldenRatioDirections;
    public Vector3[] GoldenRatioDirections 
    {
        get
        {
            if (goldenRatioDirections.Length == 0)
            {
                goldenRatioDirections = GenerateRaycastDirections();
            }

            return goldenRatioDirections;
        }
    }

    public Vector3[] GenerateRaycastDirections()
    {
        Vector3[] directions = new Vector3[600];
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

        return directions;
    }
}
