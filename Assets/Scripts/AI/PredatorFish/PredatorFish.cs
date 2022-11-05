using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class PredatorFish : EnergyBasedMovingFish
{
    [field: SerializeField]
    protected float ViewRange { get; set; }
    [field: SerializeField]
    protected float KillRange { get; set; }

    [field: SerializeField, ReadOnly]
    protected MovingFish CurrentTarget { get; set; }
    [field: SerializeField, ReadOnly]
    protected float CurrentDistance { get; set; }
    [field: SerializeField, ReadOnly]
    protected List<MovingFish> TargetCollection { get; set; }

    [field: SerializeField, ShowIf(nameof(DebugMode))]
    protected Color TargetVectorColor { get; set; }

    #region Unity Callbacks

    protected void Update()
    {
        DrawDebug();
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

    public void PopulateTargetsFromFlock(Flock flock)
    {
        foreach (MovingFish fish in flock.CurrentFishes)
        {
            TargetCollection.Add(fish);
        }
    }

    public void RemoveFishFromTargets(MovingFish fish)
    {
        TargetCollection.Remove(fish);
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
        CurrentTarget.Kill();
        CurrentTarget = null;
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

        foreach (MovingFish fish in TargetCollection)
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