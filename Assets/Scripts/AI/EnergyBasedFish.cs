using System;
using Sirenix.OdinInspector;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnergyBasedFish : MovingFish
{
    #region Public Properties

    [field: Header("Energy Management Settings"), SerializeField]
    public float MaxEnergy { get; protected set; }
    [field: SerializeField, Range(0f, 1f)]
    public float MaxStartEnergy { get; protected set; }
    [field: SerializeField, Range(0f, 1f)]
    public float MinStartEnergy { get; protected set; }
    [field: SerializeField]
    public float RegenerationSpeed { get; protected set; }
    [field: SerializeField]
    public float MaxEnergyDecaySpeed { get; protected set; }
    [field: SerializeField, Range(0f, 1f)]
    public float DecaySpeedThreshold { get; protected set; }
    [field: SerializeField, Range(0f, 1f)]
    public float MaxSpeedDecay { get; protected set; }

    [field: SerializeField, ReadOnly]
    public float CurrentEnergy { get; set; }

    [field: SerializeField, ReadOnly]
    public float RealMaxSpeed { get; set; }
    
    [field: SerializeField, ReadOnly]
    public float RealMaxSteerSpeed { get; set; }

    #endregion

    #region Unity Callbacks

    protected virtual void OnEnable()
    {
        CurrentEnergy = MaxEnergy * Random.Range(MinStartEnergy, MaxStartEnergy);
        RealMaxSpeed = MaxSpeed;
        RealMaxSteerSpeed = MaxSteerSpeed;
    }

    #endregion

    #region Protected Methods

    protected override void Move(Vector3 inputVector)
    {
        base.Move(inputVector);

        HandleEnergyManagement();
    }

    #endregion

    #region Private Methods

    protected void HandleEnergyManagement()
    {
        if (IsSpeedOverThreshold())
        {
            DecayEnergy();
        }
        else
        {
            RegenerateEnergy();
        }

        CurrentEnergy = Mathf.Clamp(CurrentEnergy, 0, MaxEnergy);

        UpdateMaxSpeed();
    }

    protected float EnergyRatio => CurrentEnergy / MaxEnergy;
    
    private void DecayEnergy()
    {
        float speedToMaxSpeedRatio = CurrentSpeed / MaxSpeed;
        float speedOverThreshold = Mathf.Abs(speedToMaxSpeedRatio - DecaySpeedThreshold);

        float decayRange = Mathf.Abs(DecaySpeedThreshold - 1);
        float speedOverThresholdRatio = speedOverThreshold / decayRange;

        CurrentEnergy -= MaxEnergyDecaySpeed * speedOverThresholdRatio * Time.deltaTime;
    }

    private void RegenerateEnergy()
    {
        CurrentEnergy += RegenerationSpeed * Time.deltaTime;
    }

    private bool IsSpeedOverThreshold()
    {
        return CurrentSpeed / MaxSpeed > DecaySpeedThreshold;
    }

    private void UpdateMaxSpeed()
    {
        float missingEnergyToMaxEnergyRatio = Mathf.Abs(EnergyRatio - 1);
        float decreaseRatio = missingEnergyToMaxEnergyRatio * MaxSpeedDecay;
        
        MaxSteerSpeed = RealMaxSteerSpeed - RealMaxSteerSpeed * decreaseRatio;
        MaxSpeed = RealMaxSpeed - RealMaxSpeed * decreaseRatio;
    }

    #endregion
}