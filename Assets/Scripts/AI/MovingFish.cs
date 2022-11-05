using Sirenix.OdinInspector;
using UnityEngine;

public abstract class MovingFish : MonoBehaviour, IFish
{
    #region Public Properties

    [field: Header("Movement Settings"), SerializeField]
    public float MinSpeed { get; protected set; }
    [field: SerializeField]
    public float MaxSpeed { get; protected set; }
    [field: SerializeField, ReadOnly]
    public Vector3 MoveVector { get; protected set; }

    [field: Header("Collision Detection Settings"), SerializeField]
    public float CollisionDetectDistance { get; protected set; }

    [field: SerializeField, Range(0, 1)]
    public float SteerForce { get; protected set; }

    #endregion

    [field: Header("Debug"), SerializeField]
    protected bool DebugMode { get; set; }
    [field: SerializeField, ShowIf(nameof(DebugMode))]
    private Color MoveVectorColor { get; set; }
    [field: SerializeField, ShowIf(nameof(DebugMode))]
    private Color CollisionDetectedColor { get; set; }
    [field: SerializeField, ShowIf(nameof(DebugMode))]
    private Color CollisionNotDetectedColor { get; set; }

    #region Public Methods

    public void Spawn(Vector3 position, Vector3 direction, Transform parent)
    {
        transform.position = position;
        transform.forward = direction;
        transform.parent = parent;
    }

    public virtual void Kill()
    {
        Destroy(this);
    }

    #endregion

    #region Protected Methods

    protected virtual void DrawDebug()
    {
        if (DebugMode)
        {
            bool headingForObstacle = GetCollisionInfo(transform.position, transform.forward).collider;
            Color lineColor = headingForObstacle ? CollisionDetectedColor : CollisionNotDetectedColor;

            Vector3 origin = transform.position;
            Vector3 destination = origin + transform.forward * CollisionDetectDistance;
            Debug.DrawLine(origin, destination, lineColor);

            destination = origin + MoveVector;
            Debug.DrawLine(origin, destination, MoveVectorColor);
        }
    }

    protected virtual void Move(Vector3 moveVector)
    {
        moveVector = SteerInto(moveVector);
        moveVector = AvoidObstacles(moveVector);
        MoveVector = moveVector.ClampMagnitude(MaxSpeed, MinSpeed);

        transform.forward = MoveVector;
        transform.position += MoveVector * Time.deltaTime;
    }

    protected Vector3 SteerInto(Vector3 steerInto)
    {
        Vector3 directionChange = steerInto.normalized - transform.forward;
        Vector3 steeredDirectionChange = directionChange * SteerForce;
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

        Debug.LogError("NO SAFE PATH FOUND");

        return new AvoidanceData(-transform.forward, currentHitInfo.distance, CollisionDetectDistance);
    }

    protected virtual RaycastHit GetCollisionInfo(Vector3 position, Vector3 raycastDirection)
    {
        Physics.Raycast(transform.position, raycastDirection, out RaycastHit hit, CollisionDetectDistance, Values.Instance.ObstacleLayer);

        return hit;
    }

    #endregion
}