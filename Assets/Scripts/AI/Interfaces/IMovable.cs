public interface IMovable
{
    #region Public Properties
    
    float MinSpeed { get; }
    float MaxSpeed { get; }
    float CollisionDetectDistance { get; }
    
    #endregion
}