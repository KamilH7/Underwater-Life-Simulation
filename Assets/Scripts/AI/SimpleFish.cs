using Sirenix.OdinInspector;
using UnityEngine;

public class SimpleFish : MonoBehaviour, IFish
{
    #region Serialized Fields

    [SerializeField]
    protected Transform target;
    [SerializeField]
    protected float minSpeed;
    [SerializeField, ReadOnly]
    protected Vector3 moveVector;
    [SerializeField]
    protected float maxSpeed;
    [SerializeField]
    protected float collisionDetectDistance;
    [SerializeField, Range(0, 1)]
    protected float steerForce;
    [SerializeField]
    protected bool debugMode;
    [SerializeField, ShowIf(nameof(debugMode))]
    private Color moveVectorColor;
    [SerializeField, ShowIf(nameof(debugMode))]
    private Color collisionDetectedColor;
    [SerializeField, ShowIf(nameof(debugMode))]
    private Color collisionNotDetectedColor;

    #endregion

    #region Public Properties

    public float MinSpeed { get => minSpeed; }
    public float MaxSpeed { get => maxSpeed; }
    public float CollisionDetectDistance { get => collisionDetectDistance; }
    public Vector3 MoveVector { get => moveVector; }

    #endregion

    #region Unity Callbacks

    protected virtual void Update()
    {
        DrawDebug();

        Vector3 moveDirection;

        if (target)
        {
            moveDirection = (transform.position - target.position).normalized;
        }
        else
        {
            moveDirection = transform.forward;
        }

        Vector3 moveVector = moveDirection * MinSpeed;

        Move(moveVector);
    }

    #endregion

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
        if (debugMode)
        {
            bool headingForObstacle = GetCollisionInfo(transform.position, transform.forward).collider;
            Color lineColor = headingForObstacle ? collisionDetectedColor : collisionNotDetectedColor;

            Vector3 origin = transform.position;
            Vector3 destination = origin + transform.forward * collisionDetectDistance;
            Debug.DrawLine(origin, destination, lineColor);

            destination = origin + moveVector;
            Debug.DrawLine(origin, destination, moveVectorColor);
        }
    }

    protected void Move(Vector3 moveVector)
    {
        moveVector = SteerInto(moveVector);
        moveVector = AvoidObstacles(moveVector);
        this.moveVector = moveVector.ClampMagnitude(maxSpeed, minSpeed);
        
        transform.forward = this.moveVector;
        transform.position += this.moveVector * Time.deltaTime;
    }

    protected Vector3 SteerInto(Vector3 steerInto)
    {
        Vector3 directionChange = steerInto.normalized - transform.forward;
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