using UnityEngine;

public class SimpleFish : MonoBehaviour, IFish
{
    #region Serialized Fields

    [SerializeField]
    protected float speed;
    [SerializeField]
    protected float collisionDetectDistance;
    [SerializeField]
    protected bool debugMode;

    #endregion

    #region Public Properties

    public float Speed => speed;
    public float CollisionDetectDistance => collisionDetectDistance;

    #endregion

    #region Unity Callbacks

    protected void Update()
    {
        Vector3 moveVector = transform.forward * Speed;
        Move(moveVector);
    }

    protected virtual void OnDrawGizmos()
    {
        if (debugMode)
        {
            Gizmos.color = IsHeadedForObstacle() ? Color.red : Color.green;
            Gizmos.DrawLine(transform.position, transform.position + transform.forward * CollisionDetectDistance);
        }
    }

    #endregion

    #region Public Methods

    public void Spawn(Vector3 position, Vector3 direction, Transform parent, float speed)
    {
        transform.position = position;
        transform.forward = direction;
        transform.parent = parent;
        this.speed = speed;
    }

    #endregion

    #region Protected Methods

    protected void Move(Vector3 moveVector)
    {
        if (IsHeadedForObstacle())
        {
            moveVector = GetAvoidanceData().GetAvoidanceVector(moveVector);
        }

        transform.forward = moveVector;
        transform.position += moveVector * Time.deltaTime;
    }

    protected virtual bool IsHeadedForObstacle()
    {
        return GetCollisionInfo(transform.position, transform.forward).collider;
    }

    protected virtual AvoidanceData GetAvoidanceData()
    {
        Vector3[] rayDirections = Values.Instance.GoldenRatioDirections;

        for (int i = 0; i < rayDirections.Length; i++)
        {
            var dir = transform.TransformDirection(rayDirections[i]);

            RaycastHit hit = GetCollisionInfo(transform.position, dir);

            if (hit.collider == null)
            {
                return new AvoidanceData(dir, hit.distance, CollisionDetectDistance);
            }
        }

        return null;
    }

    protected virtual RaycastHit GetCollisionInfo(Vector3 position, Vector3 raycastDirection)
    {
        Physics.Raycast(transform.position, raycastDirection, out RaycastHit hit, CollisionDetectDistance, Values.Instance.ObstacleLayer);

        return hit;
    }

    #endregion
}