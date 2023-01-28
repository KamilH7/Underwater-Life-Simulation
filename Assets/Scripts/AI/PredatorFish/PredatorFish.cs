using Sirenix.OdinInspector;
using UnityEngine;

[SelectionBase]
public class PredatorFish : LifeCycledFish
{
    [field: SerializeField]
    protected float ViewRange { get; set; }
    [field: SerializeField]
    protected float KillRange { get; set; }
    [field: SerializeField]
    protected float EnergyForKill { get; set; }

    [field: SerializeField, ReadOnly]
    protected MovingFish CurrentTarget { get; set; }
    [field: SerializeField, ReadOnly]
    protected float CurrentDistance { get; set; }
    [field: SerializeField, ShowIf(nameof(DebugMode))]
    protected Color TargetVectorColor { get; set; }
    protected Flock TargetsFlock { get; set; }

    #region Unity Callbacks

    protected virtual void Update()
    {
        DrawDebug();

        Vector3? reproductionVector = GetReproductionBehaviour();

        if (reproductionVector != null)
        {
            Move((Vector3) reproductionVector);
            return;
        }

        SetTarget();

        if (CurrentTarget != null)
        {
            TargetBehaviour();
        }
        else
        {
            DocileBehaviour();
        }
    }

    #endregion

    #region Public Methods

    public void Initialize()
    {
        TargetsFlock = Flock.Instance;
        TargetsFlock.CurrentPredators.Add(this);
    }

    public override void Spawn(Vector3 position, Vector3 direction, Quaternion rotation, Transform parent, GameObject prefab)
    {
        base.Spawn(position, direction, rotation, parent, prefab);
        Initialize();
    }

    public override void Despawn()
    {
        TargetsFlock.CurrentPredators.Remove(this);

        base.Despawn();
    }

    #endregion

    #region Protected Methods

    protected override void DrawDebug()
    {
        base.DrawDebug();

        if (DebugMode)
        {
            if (CurrentTarget)
            {
                Vector3 currentPosition = transform.position;
                Debug.DrawLine(currentPosition, CurrentTarget.transform.position, TargetVectorColor);
            }
        }
    }

    #endregion

    #region Private Methods

    private void TargetBehaviour()
    {
        Vector3 moveVector = CurrentTarget.transform.position - transform.position;

        Move(moveVector.normalized * MaxSpeed);

        if (moveVector.magnitude < KillRange)
        {
            KillCurrentTarget();
        }
    }

    private void DocileBehaviour()
    {
        Vector3 direction = Vector3.forward;
        Move(direction * MinSpeed);
    }

    private void KillCurrentTarget()
    {
        CurrentTarget.Despawn();
        CurrentTarget = null;
        AddEnergy(EnergyForKill);
    }

    private void SetTarget()
    {
        if (CurrentTarget != null)
        {
            if ((CurrentTarget.transform.position - transform.position).magnitude > ViewRange)
            {
                CurrentTarget = null;
            }
        }

        foreach (FlockableFish fish in TargetsFlock.CurrentFishes)
        {
            if (CurrentTarget == null)
            {
                CurrentDistance = ViewRange;
            }

            float newDistance = (transform.position - fish.transform.position).magnitude;

            if (newDistance < CurrentDistance)
            {
                CurrentTarget = fish;
                CurrentDistance = (transform.position - CurrentTarget.transform.position).magnitude;
            }
        }
    }

    #endregion
}