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
    private Vector3 avgDisplacementVector;
    [SerializeField,ReadOnly]
    private Vector3 avgAvoidanceDisplacementVector;

    [Header("Flockable fish debug settings")]
    [SerializeField, ShowIf(nameof(debugMode))]
    private Color alignmentBehaviourColour;
    [SerializeField, ShowIf(nameof(debugMode))]
    private Color cohesionBehaviourColour;
    [SerializeField, ShowIf(nameof(debugMode))]
    private Color separationBehaviourColour;
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

    public void UpdateBehaviourValues(Vector3 avgPosition, Vector3 avgDisplacementVector, Vector3 avgAvoidanceDisplacementVector)
    {
        this.avgPosition = avgPosition;
        this.avgDisplacementVector = avgDisplacementVector;
        this.avgAvoidanceDisplacementVector = avgAvoidanceDisplacementVector;
    }

    #endregion

    #region Private Methods

    private Vector3 GetBehaviour()
    {
        Vector3 moveDirection = transform.forward;

        moveDirection += AlignmentBehaviour();
        moveDirection += CohesionBehaviour();
        moveDirection += SeparationBehaviour();

        return moveDirection;
    }

    private Vector3 AlignmentBehaviour()
    {
        return avgDisplacementVector * flock.AlignmentBehaviour;
    }

    private Vector3 CohesionBehaviour()
    {
        var direction = (avgPosition - transform.position);

        return direction * flock.CohesionBehaviour;
    }

    private Vector3 SeparationBehaviour()
    {
        return avgAvoidanceDisplacementVector * flock.SeparationBehaviour;
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
            Debug.DrawLine(currentPosition, currentPosition + GetBehaviour(), finalBehaviourColour);
        }
    }

    #endregion
}