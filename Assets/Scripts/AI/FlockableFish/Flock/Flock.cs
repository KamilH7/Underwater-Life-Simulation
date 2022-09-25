using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class Flock : MonoBehaviour
{
    #region Serialized Fields

    [Header("Initialization Settings"), SerializeField]
    private bool spawnAutomatically = true;
    [SerializeField, ShowIf(nameof(spawnAutomatically))]
    private float spawnRange = 5;
    [SerializeField, ShowIf(nameof(spawnAutomatically))]
    private FlockableFish fishPrefab;
    [SerializeField, ShowIf(nameof(spawnAutomatically))]
    private int spawnAmount;
    [field: SerializeField]
    public List<FlockableFish> CurrentFishes { get; private set; } = new();
    [field: SerializeField]
    public List<PredatorFish> CurrentPredators { get; private set; } = new();

    #endregion

    #region Public Properties

    [field: Header("Behaviour Settings"), Range(0, 10), SerializeField]
    public float AlignmentBehaviour { get; private set; }
    [field: Range(0, 10), SerializeField]
    public float CohesionBehaviour { get; private set; }
    [field: Range(0, 10), SerializeField]
    public float SeparationBehaviour { get; private set; }
    [field: Range(0, 100), SerializeField]
    public float FearBehaviour { get; private set; }
    [field: SerializeField]
    public float ViewDistance { get; private set; }
    [field: SerializeField]
    public float SafeDistance { get; private set; }

    #endregion

    #region Unity Callbacks

    private void Start()
    {
        if (spawnAutomatically)
        {
            SpawnFlock();
        }

        InitializeFlock();
        InitializePredators();
    }

    private void Update()
    {
        BoidShader.Instance.PopulateAgentsWithData(this);
    }

    #endregion

    public void FishKilled(FlockableFish flockableFish)
    {
        CurrentFishes.Remove(flockableFish);
        
        InformPredatorsOfFishKilled(flockableFish);
    }
    
    #region Private Methods

    private void InformPredatorsOfFishKilled(FlockableFish flockableFish)
    {
        foreach (PredatorFish predator in CurrentPredators)
        {
            predator.RemoveFishFromTargets(flockableFish);
        }
    }

    private void InitializeFlock()
    {
        foreach (var flockableFish in CurrentFishes)
        {
            flockableFish.Initialize(this);
        }
    }

    private void InitializePredators()
    {
        foreach (PredatorFish predator in CurrentPredators)
        {
            predator.PopulateTargetsFromFlock(this);
        }
    }
    
    private void SpawnFlock()
    {
        CurrentFishes = new List<FlockableFish>();
        
        for (int i = 0; i < spawnAmount; i++)
        {
            Vector3 spawnPosition = Random.insideUnitSphere * spawnRange;
            FlockableFish newFish = Instantiate(fishPrefab, spawnPosition + transform.position, Quaternion.identity, transform);
            newFish.transform.forward = spawnPosition.normalized;
            CurrentFishes.Add(newFish);
        }
    }

    #endregion
}