using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(fileName = "ReproductionGroup", menuName = "ReproductionGroup")]
public class ReproductionGroup : ScriptableObject
{
    [field: SerializeField, ReadOnly]
    private List<ReproducingFish> availableFishes;

    public void RequestPartner(ReproducingFish newFish)
    {
        availableFishes.Add(newFish);
        CalculatePartners();
    }
    
    private void OnEnable()
    {
        availableFishes = new List<ReproducingFish>();
    }

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

    private bool TryMatchFishes(ReproducingFish fish1, ReproducingFish fish2)
    {
        float distance = (fish1.transform.position - fish2.transform.position).magnitude;

        if (distance <= fish1.PartnerViewRange)
        {
            availableFishes.Remove(fish1);
            fish1.PartnerFoundEvent.Invoke(fish2);
            availableFishes.Remove(fish2);
            fish1.PartnerFoundEvent.Invoke(fish1);

            return true;
        }

        return false;
    }
}