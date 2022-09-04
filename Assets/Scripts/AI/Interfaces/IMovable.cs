using UnityEngine;

public interface IMovable
{
    #region Public Properties

    Transform Location { get; }
    float Speed { get; }
    float CollisionDetectDistance { get; set; }
    
    #endregion
}