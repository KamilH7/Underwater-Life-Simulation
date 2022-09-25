using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
public class PredatorFish : SimpleFish
{
    [SerializeField]
    private List<SimpleFish> targetFishes = new List<SimpleFish>();
    [SerializeField]
    private float viewRange;
    [SerializeField]
    private float killRange;

    [SerializeField, ReadOnly]
    private SimpleFish currentTarget;
    [SerializeField, ReadOnly]
    private float currentDistance;
    
    [SerializeField, ShowIf(nameof(debugMode))]
    private Color targetVectorColor;
    
    protected override void Update()
    {
        DrawDebug();
        SetTarget();
        
        if (currentTarget != null)
        {
            TargetBehaviour();
        }
        else
        {
            DocileBehaviour();
        }
    }

    private void TargetBehaviour()
    {
        Vector3 moveVector = currentTarget.transform.position - transform.position;
        
        Move(moveVector.normalized * maxSpeed);

        if (moveVector.magnitude < killRange)
        {
            KillCurrentTarget();
        }
    }

    private void DocileBehaviour()
    {
        Vector3 direction = Vector3.forward;
        Move(direction * minSpeed);
    }
    
    private void KillCurrentTarget()
    {
        currentTarget.Kill();
        currentTarget = null;
    }

    private void SetTarget()
    {
        if (currentTarget != null)
        {
            if ((currentTarget.transform.position - transform.position).magnitude > viewRange)
            {
                currentTarget = null;
            }
        }

        foreach (SimpleFish fish in targetFishes)
        {
            if (currentTarget == null)
            {
                currentDistance = viewRange;
            }

            float newDistance = (transform.position - fish.transform.position).magnitude;

            if (newDistance < currentDistance)
            {
                currentTarget = fish;
                currentDistance = (transform.position - currentTarget.transform.position).magnitude;
            }
        }
    }

    public void PopulateTargetsFromFlock(Flock flock)
    {
        foreach (SimpleFish fish in flock.CurrentFishes)
        {
            targetFishes.Add(fish);
        }
    }
    
    public void RemoveFishFromTargets(SimpleFish fish)
    {
        targetFishes.Remove(fish);
    }

    protected override void DrawDebug()
    {
        base.DrawDebug();

        if (debugMode)
        {
            if (currentTarget)
            {
                Vector3 currentPosition = transform.position;
                Debug.DrawLine(currentPosition, currentTarget.transform.position, targetVectorColor);
            }
        }
    }
}
