using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(fileName = "ReproductionGroup", menuName = "ReproductionGroup")]
public class ReproductionGroup : ScriptableObject
{
    #region Serialized Fields

    [field: SerializeField, ReadOnly]
    private List<LifeCycledFish> availableFishes;

    #endregion

    #region Unity Callbacks

    private void OnEnable()
    {
        availableFishes = new List<LifeCycledFish>();
    }

    #endregion

    #region Public Methods

    public void RequestPartner(LifeCycledFish newFish)
    {
        availableFishes.Add(newFish);
        CalculatePartners();
    }

    public void StopRequestingPartner(LifeCycledFish fish)
    {
        availableFishes.Remove(fish);
    }

    #endregion

    #region Private Methods

    private bool CalculatePartners()
    {
        for (int i = 0; i < availableFishes.Count; i++)
        {
            for (int j = 0; j < availableFishes.Count; j++)
            {
                if (i == j)
                {
                    continue;
                }

                if (TryMatchFishes(availableFishes[i], availableFishes[j]))
                {
                    return true;
                }
            }
        }

        return false;
    }

    private bool TryMatchFishes(LifeCycledFish fish1, LifeCycledFish fish2)
    {
        float distance = (fish1.transform.position - fish2.transform.position).magnitude;

        if (distance <= fish1.PartnerViewRange)
        {
            availableFishes.Remove(fish1);
            fish1.PartnerFoundEvent.Invoke(fish2);
            availableFishes.Remove(fish2);
            fish2.PartnerFoundEvent.Invoke(fish1);

            return true;
        }

        return false;
    }

    #endregion
}