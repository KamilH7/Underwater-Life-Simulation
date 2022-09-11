using UnityEngine;

public class SimpleFish : MonoBehaviour, IFish
{
    #region Serialized Fields

    [SerializeField]
    protected Transform target;
    [SerializeField]
    protected float speed;
    [SerializeField]
    protected float collisionDetectDistance;
    [SerializeField, Range(0,1)]
    protected float steerForce;
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
        Vector3 moveDirection;
        
        if (target)
        {
            moveDirection = (transform.position - target.position).normalized;
        }
        else
        {
            moveDirection = transform.forward;
        }
        
        Vector3 moveVector = moveDirection * Speed;
        Move(moveVector);
    }

    protected virtual void OnDrawGizmos()
    {
        if (debugMode)
        {
            Gizmos.color = GetCollisionInfo(transform.position, transform.forward).collider ? Color.red : Color.green;
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
        moveVector = SteerInto(moveVector);
        moveVector = AvoidObstacles(moveVector);
        transform.forward = moveVector;
        transform.position += moveVector * Time.deltaTime;
    }

    protected Vector3 SteerInto(Vector3 steerInto)
    {
        Vector3 directionChange = transform.forward - steerInto.normalized;
        Vector3 steeredDirectionChange = directionChange * steerForce;
        Vector3 goalDirection = transform.forward + steeredDirectionChange;

        return steerInto.magnitude * goalDirection;
    }
    
    protected Vector3 AvoidObstacles(Vector3 moveVector)
    {
        RaycastHit hitInfo = GetCollisionInfo(transform.position, transform.forward);

        if (hitInfo.collider)
        {
            moveVector = GetAvoidanceData(hitInfo).GetAvoidanceVector(moveVector);
        }

        return moveVector;
    }

    protected virtual AvoidanceData GetAvoidanceData(RaycastHit currentHitInfo)
    {
        Vector3[] rayDirections = Values.Instance.GoldenRatioDirections;
        Transform cachedTransform = transform;
        
        for (int i = 0; i < rayDirections.Length; i++)
        {
            var dir = cachedTransform.TransformDirection(rayDirections[i]);

            RaycastHit hit = GetCollisionInfo(cachedTransform.position, dir);

            if (hit.collider == null)
            {
                return new AvoidanceData(dir, currentHitInfo.distance, CollisionDetectDistance);
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