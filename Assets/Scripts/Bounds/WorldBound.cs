using UnityEngine;

public class WorldBound : MonoBehaviour
{
    #region Private Fields

    [Header("Settings")]
    [SerializeField]
    private float minResistanceDistance = 1;
    [SerializeField]
    private float maxResistanceDistance  = 0;
    [SerializeField]
    private bool showBounds = false;
    [SerializeField]
    private Renderer renderer;
    
    private Vector3 center;
    private float radius;

    #endregion

    #region Unity Callbacks

    private void Start()
    {
        center = transform.position;
        radius = GetSphereRadius();
    }

    #endregion

    public float GetResistance(Vector3 point)
    {
        float distanceFromCenter = (point - center).magnitude;
        float minDistance = radius - minResistanceDistance;

        if (minDistance < distanceFromCenter)
        {
            float absoluteDistance = minResistanceDistance - maxResistanceDistance;
            float distanceFromMin = Mathf.Abs(minDistance - distanceFromCenter);
            float percentage = Mathf.Clamp01(distanceFromMin / absoluteDistance);
            return percentage;
        }

        return 0;
    }  

    #region Private Methods

    private float GetSphereRadius()
    {
        return renderer.bounds.extents.magnitude/2;
    }

    private void OnDrawGizmos()
    {
        if (showBounds)
        {
            var position = transform.position;
            
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(position, GetSphereRadius() - maxResistanceDistance);
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(position, GetSphereRadius() - minResistanceDistance);
        }
    }

    #endregion
}