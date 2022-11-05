using UnityEngine;

public interface IMovable
{
    #region Public Properties
    
    float MinSpeed { get; }
    float MaxSpeed { get; }
    float CollisionDetectDistance { get; }
    Vector3 MoveVector { get; }
    float SteerForce { get; }

    #endregion
}