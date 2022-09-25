using Sirenix.OdinInspector;
using UnityEngine;

public class FlockableFish : SimpleFish
{
    #region Private Fields
    
    [Header("Flockable fish values")]
    [SerializeField,ReadOnly]
    private Flock flock;
    [SerializeField,ReadOnly]
    private Vector3 avgPosition;
    [SerializeField,ReadOnly]
    private Vector3 avgMoveVector;
    [SerializeField,ReadOnly]
    private Vector3 avgNeighbourAvoidanceVector;
    [SerializeField,ReadOnly]
    private Vector3 avgPredatorAvoidanceVector;

    [Header("Flockable fish debug settings")]
    [SerializeField, ShowIf(nameof(debugMode))]
    private Color alignmentBehaviourColour;
    [SerializeField, ShowIf(nameof(debugMode))]
    private Color cohesionBehaviourColour;
    [SerializeField, ShowIf(nameof(debugMode))]
    private Color separationBehaviourColour;
    [SerializeField, ShowIf(nameof(debugMode))]
    private Color predatorAvoidanceBehaviourColour;
    [SerializeField, ShowIf(nameof(debugMode))]
    private Color finalBehaviourColour;
    #endregion

    #region Unity Callbacks

    protected override void Update()
    {
        DrawDebug();
        Move(GetBehaviour());
    }

    #endregion

    #region Public Methods

    public void Initialize(Flock flock)
    {
        this.flock = flock;
    }

    public void UpdateBehaviourValues(Vector3 avgPosition, Vector3 avgMoveVector, Vector3 avgNeighbourAvoidanceVector, Vector3 avgPredatorAvoidanceVector)
    {
        this.avgPosition = avgPosition;
        this.avgMoveVector = avgMoveVector;
        this.avgNeighbourAvoidanceVector = avgNeighbourAvoidanceVector;
        this.avgPredatorAvoidanceVector = avgPredatorAvoidanceVector;
    }

    #endregion

    #region Private Methods

    private Vector3 GetBehaviour()
    {
        Vector3 moveDirection = transform.forward;

        moveDirection += AlignmentBehaviour();
        moveDirection += CohesionBehaviour();
        moveDirection += SeparationBehaviour();
        moveDirection += PredatorAvoidanceBehaviour();

        return moveDirection;
    }

    private Vector3 AlignmentBehaviour()
    {
        return avgMoveVector * flock.AlignmentBehaviour;
    }

    private Vector3 CohesionBehaviour()
    {
        var direction = (avgPosition - transform.position);

        return direction * flock.CohesionBehaviour;
    }

    private Vector3 SeparationBehaviour()
    {
        return avgNeighbourAvoidanceVector * flock.SeparationBehaviour;
    }
    
    private Vector3 PredatorAvoidanceBehaviour()
    {
        return avgPredatorAvoidanceVector * flock.FearBehaviour;
    }

    public override void Kill()
    {
        flock.FishKilled(this);
        Destroy(gameObject);
    }
    
    protected override void DrawDebug()
    {
        base.DrawDebug();

        if (debugMode)
        {
            Vector3 currentPosition = transform.position;
            
            Debug.DrawLine(currentPosition, currentPosition + AlignmentBehaviour(), alignmentBehaviourColour);
            Debug.DrawLine(currentPosition, currentPosition + CohesionBehaviour(), cohesionBehaviourColour);
            Debug.DrawLine(currentPosition, currentPosition + SeparationBehaviour(), separationBehaviourColour);
            Debug.DrawLine(currentPosition, currentPosition + PredatorAvoidanceBehaviour(), predatorAvoidanceBehaviourColour);
            Debug.DrawLine(currentPosition, currentPosition + GetBehaviour(), finalBehaviourColour);
        }
    }

    #endregion
}