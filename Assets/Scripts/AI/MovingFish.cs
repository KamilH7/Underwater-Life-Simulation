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
    public float CurrentSpeed { get; private set; }
    public Vector3 MoveVector { get; protected set; }

    [field: Header("Collision Detection Settings"), SerializeField]
    public float CollisionDetectDistance { get; protected set; }

    [field: SerializeField, Range(0, 100)]
    public float MaxSteerSpeed { get; protected set; }

    #endregion

    [field: Header("Debug"), SerializeField]
    protected bool DebugMode { get; set; }
    [field: SerializeField, ShowIf(nameof(DebugMode))]
    private Color MoveVectorColor { get; set; }
    [field: SerializeField, ShowIf(nameof(DebugMode))]
    private Color CollisionDetectedColor { get; set; }
    [field: SerializeField, ShowIf(nameof(DebugMode))]
    private Color CollisionNotDetectedColor { get; set; }

    protected GameObject FishPrefab { get; set; }

    #region Public Methods

    public virtual void Spawn(Vector3 position, Vector3 direction, Quaternion rotation, Transform parent, GameObject prefab)
    {
        transform.position = position;
        transform.rotation = rotation;
        transform.forward = direction;
        transform.parent = parent;
        FishPrefab = prefab;
    }

    public virtual void Despawn()
    {
        Destroy(gameObject);
    }

    #endregion

    #region Protected Methods

    protected virtual void DrawDebug()
    {
        if (DebugMode)
        {
            bool headingForObstacle = GetCollisionInfo(transform.forward).collider;
            Color lineColor = headingForObstacle ? CollisionDetectedColor : CollisionNotDetectedColor;

            Vector3 origin = transform.position;
            Vector3 destination = origin + transform.forward * CollisionDetectDistance;
            Debug.DrawLine(origin, destination, lineColor);

            destination = origin + MoveVector;
            Debug.DrawLine(origin, destination, MoveVectorColor);
        }
    }

    protected virtual void Move(Vector3 inputVector)
    {
        Transform cachedTransform = transform;
        
        MoveVector = inputVector;
        Vector3 steerVector = Vector3.Lerp(transform.forward, MoveVector, Time.deltaTime * MaxSteerSpeed);
        cachedTransform.forward += AvoidObstacles(steerVector);
        
        Vector3 positionVector = (MoveVector - cachedTransform.position).ClampMagnitude(MaxSpeed, MinSpeed);
        cachedTransform.position += cachedTransform.forward * (positionVector.magnitude * Time.deltaTime);
        
        CurrentSpeed = MoveVector.magnitude;
    }

    protected Vector3 AvoidObstacles(Vector3 moveVector)
    {
        RaycastHit hitInfo = GetCollisionInfo(moveVector.normalized);

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

            RaycastHit hit = GetCollisionInfo(dir);

            if (hit.collider == null)
            {
                return new AvoidanceData(dir, currentHitInfo.distance, CollisionDetectDistance);
            }
        }

        Debug.LogError("NO SAFE PATH FOUND");

        return new AvoidanceData(-transform.forward, currentHitInfo.distance, CollisionDetectDistance);
    }

    protected virtual RaycastHit GetCollisionInfo(Vector3 raycastDirection)
    {
        Physics.Raycast(transform.position, raycastDirection, out RaycastHit hit, CollisionDetectDistance, Values.Instance.ObstacleLayer);

        return hit;
    }

    #endregion
}