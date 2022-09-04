using UnityEngine;

public class SimpleFish : MonoBehaviour, IFish
{
    #region Serialized Fields

    [SerializeField]
    protected float speed;

    #endregion

    #region Private Fields

    protected Transform location;

    #endregion

    #region Public Properties

    public Transform Location => location;
    public float Speed => speed;
    
    [field: SerializeField]
    public float CollisionDetectDistance { get; set; }

    #endregion

    #region Unity Callbacks

    protected virtual void Start()
    {
        InitializeReferences();
    }

    protected void Update()
    {
        Vector3 moveVector = Vector3.forward * Speed;

        if (IsHeadedForObstacle())
        {
            AvoidanceData avoidanceData = GetAvoidanceData();
            moveVector = avoidanceData.GetAvoidanceVector(moveVector);
        }

        Move(moveVector);
    }

    #endregion

    #region Public Methods

    public void Spawn(Vector3 position, Vector3 direction, Transform parent, float speed)
    {
        location.position = position;
        location.forward = direction;
        location.parent = parent;
        this.speed = speed;
    }

    #endregion

    #region Protected Methods

    protected void Move(Vector3 moveVector)
    {
        if (IsHeadedForObstacle())
        {
            AvoidanceData avoidanceData = GetAvoidanceData();
            moveVector = avoidanceData.GetAvoidanceVector(moveVector);
        }

        transform.position += moveVector * Time.deltaTime;
    }

    protected virtual bool IsHeadedForObstacle()
    {
        return GetCollisionInfo(location.position, location.forward).collider;
    }

    protected virtual AvoidanceData GetAvoidanceData()
    {
        Vector3[] rayDirections = Values.Instance.GoldenRatioDirections;

        for (int i = 0; i < rayDirections.Length; i++)
        {
            var dir = location.TransformDirection(rayDirections[i]);
            
            RaycastHit hit = GetCollisionInfo(location.position, dir);

            if (hit.collider == null)
            {
                return new AvoidanceData(dir, GetAvoidanceWeight(hit.distance));
            }
        }

        return null;
    }

    protected virtual float GetAvoidanceWeight(float distanceFromObstacle)
    {
        float minDistance = CollisionDetectDistance;

        return 1 - distanceFromObstacle / minDistance;
    }

    protected virtual RaycastHit GetCollisionInfo(Vector3 position, Vector3 raycastDirection)
    {
        Physics.Raycast(location.position, raycastDirection, out RaycastHit hit, CollisionDetectDistance, Values.Instance.ObstacleLayer);

        return hit;
    }

    #endregion

    #region Private Methods

    private void InitializeReferences()
    {
        location = transform;
    }

    #endregion
}