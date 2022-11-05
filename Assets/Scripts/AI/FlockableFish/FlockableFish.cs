using Sirenix.OdinInspector;
using UnityEngine;

public class FlockableFish : EnergyBasedMovingFish
{
    [field: Header("Flockable fish values"), SerializeField, Range(0f,1f)]
    protected float EnergySpentOnFlocking { get; set; }
    [field: SerializeField, ReadOnly]
    protected Flock CurrentFlock { get; set; }
    [field: SerializeField, ReadOnly]
    protected Vector3 AvgPosition { get; set; }
    [field: SerializeField, ReadOnly]
    protected Vector3 AvgMoveVector { get; set; }
    [field: SerializeField, ReadOnly]
    protected Vector3 AvgNeighbourAvoidanceVector { get; set; }
    [field: SerializeField, ReadOnly]
    protected Vector3 AvgPredatorAvoidanceVector { get; set; }

    [field: Header("Flockable fish debug settings"), SerializeField, ShowIf(nameof(DebugMode))]
    private Color AlignmentBehaviourColour { get; set; }
    [field: SerializeField, ShowIf(nameof(DebugMode))]
    private Color CohesionBehaviourColour { get; set; }
    [field: SerializeField, ShowIf(nameof(DebugMode))]
    private Color SeparationBehaviourColour { get; set; }
    [field: SerializeField, ShowIf(nameof(DebugMode))]
    private Color PredatorAvoidanceBehaviourColour { get; set; }
    [field: SerializeField, ShowIf(nameof(DebugMode))]
    private Color FinalBehaviourColour { get; set; }

    #region Unity Callbacks

    protected void Update()
    {
        DrawDebug();
        Move(GetBehaviour());
    }

    #endregion

    #region Public Methods

    public void Initialize(Flock flock)
    {
        CurrentFlock = flock;
    }

    public void UpdateBehaviourValues(Vector3 avgPosition, Vector3 avgMoveVector, Vector3 avgNeighbourAvoidanceVector, Vector3 avgPredatorAvoidanceVector)
    {
        AvgPosition = avgPosition;
        AvgMoveVector = avgMoveVector;
        AvgNeighbourAvoidanceVector = avgNeighbourAvoidanceVector;
        AvgPredatorAvoidanceVector = avgPredatorAvoidanceVector;
    }

    public override void Kill()
    {
        CurrentFlock.FishKilled(this);
        Destroy(gameObject);
    }

    #endregion

    #region Protected Methods

    protected override void DrawDebug()
    {
        base.DrawDebug();

        if (DebugMode)
        {
            Vector3 currentPosition = transform.position;

            Debug.DrawLine(currentPosition, currentPosition + AlignmentBehaviour(), AlignmentBehaviourColour);
            Debug.DrawLine(currentPosition, currentPosition + CohesionBehaviour(), CohesionBehaviourColour);
            Debug.DrawLine(currentPosition, currentPosition + SeparationBehaviour(), SeparationBehaviourColour);
            Debug.DrawLine(currentPosition, currentPosition + PredatorAvoidanceBehaviour(), PredatorAvoidanceBehaviourColour);
            Debug.DrawLine(currentPosition, currentPosition + GetBehaviour(), FinalBehaviourColour);
        }
    }

    #endregion

    #region Private Methods

    private Vector3 GetBehaviour()
    {
        Vector3 moveVector = transform.forward;

        moveVector += AlignmentBehaviour();
        moveVector += CohesionBehaviour();
        moveVector += SeparationBehaviour();
        
        moveVector = moveVector.ClampMagnitude(MaxSpeed * EnergySpentOnFlocking, MinSpeed);
        
        moveVector += PredatorAvoidanceBehaviour();

        return moveVector;
    }

    private Vector3 AlignmentBehaviour()
    {
        return AvgMoveVector * CurrentFlock.AlignmentBehaviour;
    }

    private Vector3 CohesionBehaviour()
    {
        var direction = AvgPosition - transform.position;

        return direction * CurrentFlock.CohesionBehaviour;
    }

    private Vector3 SeparationBehaviour()
    {
        return AvgNeighbourAvoidanceVector * CurrentFlock.SeparationBehaviour;
    }

    private Vector3 PredatorAvoidanceBehaviour()
    {
        return AvgPredatorAvoidanceVector * CurrentFlock.FearBehaviour;
    }

    #endregion
}