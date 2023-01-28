using System;
using System.Collections;
using Sirenix.OdinInspector;
using UnityEngine;
using Random = UnityEngine.Random;

public class LifeCycledFish : EnergyBasedFish
{
    #region Public Properties

    public Action<LifeCycledFish> PartnerFoundEvent { get; set; }

    [field: Header("Reproducing Settings"), SerializeField]
    public float PartnerViewRange { get; set; }

    #endregion

    [field: SerializeField, Range(0f, 1f)]
    protected float MinEnergyRatioForReproduction { get; set; }
    [field: SerializeField]
    protected float ReproductionCooldown { get; set; }
    [field: SerializeField]
    protected float ReproductionRange { get; set; }
    [field: SerializeField, Range(0f, 1f)]
    protected float ReproductionEnergyPercentCost { get; set; }
    [field: SerializeField]
    protected ReproductionGroup CurrentReproductionGroup { get; set; }

    [field: Header("Death Settings"), SerializeField]
    private float MinLifetimeSeconds { get; set; }
    [field: SerializeField]
    private float MaxLifetimeSeconds { get; set; }

    [field: SerializeField, ReadOnly]
    private LifeCycledFish CurrentPartner { get; set; }
    [field: SerializeField, ReadOnly]
    private bool PartnerRequested { get; set; }
    [field: SerializeField, ReadOnly]
    private bool HasReproduced { get; set; }
    [field: SerializeField, ReadOnly]
    private float LifetimeSeconds { get; set; }

    #region Unity Callbacks

    protected override void OnEnable()
    {
        PartnerRequested = false;
        base.OnEnable();
        StartCoroutine(LifetimeCoroutine());
        AttachEvents();
    }

    protected virtual void OnDisable()
    {
        DetachEvents();
    }

    #endregion

    #region Public Methods

    public override void Despawn()
    {
        if (PartnerRequested)
        {
            CurrentReproductionGroup.StopRequestingPartner(this);
            PartnerRequested = false;
        }

        base.Despawn();
    }

    public void Reproduced()
    {
        PartnerRequested = false;
        HasReproduced = true;
        CurrentPartner = null;

        ReduceEnergy();
        StartReproductionCooldown();
    }

    #endregion

    #region Protected Methods

    protected Vector3? GetReproductionBehaviour()
    {
        if (HasPartner())
        {
            Vector3 moveVectorToPartner = CalculateMoveVectorToPartner();

            if (moveVectorToPartner.magnitude < ReproductionRange)
            {
                Reproduce();
            }

            return moveVectorToPartner;
        }
        
        if (CanReproduce())
        {
            if (CanRequestPartner())
            {
                RequestPartner();
            }
        }

        return null;
    }

    #endregion

    #region Private Methods

    private void AttachEvents()
    {
        PartnerFoundEvent += PartnerFound;
    }

    private void DetachEvents()
    {
        PartnerFoundEvent -= PartnerFound;
    }

    private void PartnerFound(LifeCycledFish newPartner)
    {
        CurrentPartner = newPartner;
        PartnerRequested = true;
    }

    private void Reproduce()
    {
        if (HasReproduced == false)
        {
            SpawnChild();
            CurrentPartner.Reproduced();
            Reproduced();
        }
    }

    private Vector3 CalculateMoveVectorToPartner()
    {
        return CurrentPartner.transform.position - transform.position;
    }

    private void RequestPartner()
    {
        CurrentReproductionGroup.RequestPartner(this);
        PartnerRequested = true;
    }

    private void ReduceEnergy()
    {
        float energyDecreaseAmount = MaxEnergy * ReproductionEnergyPercentCost;
        CurrentEnergy = Mathf.Clamp(CurrentEnergy - energyDecreaseAmount, 0, MaxEnergy);
    }

    private void StartReproductionCooldown()
    {
        StartCoroutine(ReproductionCooldownCoroutine());
    }

    private IEnumerator ReproductionCooldownCoroutine()
    {
        yield return new WaitForSeconds(ReproductionCooldown);
        HasReproduced = false;
    }

    private LifeCycledFish SpawnChild()
    {
        Vector3 spawnPosition = (transform.position + CurrentPartner.transform.position) / 2;

        LifeCycledFish newInstance = Instantiate(FishPrefab).GetComponent<LifeCycledFish>();
        newInstance.Spawn(spawnPosition, transform.forward, Random.rotation, transform.parent, FishPrefab);

        return newInstance;
    }

    private bool CanReproduce()
    {
        return EnergyRatio >= MinEnergyRatioForReproduction && !HasReproduced;
    }

    private bool HasPartner()
    {
        return CurrentPartner != null;
    }

    private bool CanRequestPartner()
    {
        return PartnerRequested == false && HasReproduced == false;
    }

    private IEnumerator LifetimeCoroutine()
    {
        LifetimeSeconds = Random.Range(MinLifetimeSeconds, MaxLifetimeSeconds);

        yield return new WaitForSeconds(LifetimeSeconds);

        Despawn();
    }

    #endregion
}