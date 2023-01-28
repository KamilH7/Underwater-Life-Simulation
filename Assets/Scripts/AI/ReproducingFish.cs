using System;
using System.Collections;
using Sirenix.OdinInspector;
using UnityEngine;
using Random = UnityEngine.Random;

public class ReproducingFish : EnergyBasedFish
{
    #region Public Properties

    public Action<ReproducingFish> PartnerFoundEvent { get; set; }

    public Action<ReproducingFish> FishWasBornEvent { get; set; }
    [field: SerializeField]
    public float PartnerViewRange { get; set; }

    #endregion

    [field: Header("Reproducing Settings")]

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
    [field: SerializeField, ReadOnly]
    private ReproducingFish CurrentPartner { get; set; }

    [field: SerializeField, ReadOnly]
    private bool IsReproductionOnCooldown { get; set; }
    [field: SerializeField, ReadOnly]
    private bool PartnerRequested { get; set; }
    [field: SerializeField, ReadOnly]
    private bool HasReproduced { get; set; }

    #region Unity Callbacks

    protected override void OnEnable()
    {
        base.OnEnable();
        AttachEvents();
    }

    protected virtual void OnDisable()
    {
        DetachEvents();
    }

    #endregion

    #region Public Methods

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
        if (CanReproduce())
        {
            if (CanRequestPartner())
            {
                RequestPartner();
            }
        }

        if (HasPartner())
        {
            Vector3 moveVectorToPartner = CalculateMoveVectorToPartner();

            if (moveVectorToPartner.magnitude < ReproductionRange)
            {
                Reproduce();
            }

            return moveVectorToPartner;
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

    private void PartnerFound(ReproducingFish newPartner)
    {
        CurrentPartner = newPartner;
    }

    private void Reproduce()
    {
        if (HasReproduced == false)
        {
            ReproducingFish child = SpawnChild();

            FishWasBornEvent?.Invoke(child);
            
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
        IsReproductionOnCooldown = true;

        yield return new WaitForSeconds(ReproductionCooldown);
        IsReproductionOnCooldown = false;
    }

    private ReproducingFish SpawnChild()
    {
        Vector3 spawnPosition = (transform.position + CurrentPartner.transform.position) / 2;

        ReproducingFish newInstance = Instantiate(FishPrefab).GetComponent<ReproducingFish>();
        newInstance.Spawn(spawnPosition, transform.forward, Random.rotation, transform.parent, FishPrefab);
        
        return newInstance;
    }

    private bool CanReproduce()
    {
        return EnergyRatio >= MinEnergyRatioForReproduction && !IsReproductionOnCooldown;
    }

    private bool HasPartner()
    {
        return CurrentPartner != null;
    }

    private bool CanRequestPartner()
    {
        return PartnerRequested == false;
    }

    #endregion
}