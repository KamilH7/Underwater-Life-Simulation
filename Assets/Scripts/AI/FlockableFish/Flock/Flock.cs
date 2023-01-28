using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class Flock : MonoBehaviour
{
    #region Serialized Fields

    public static Flock Instance { get; protected set; }

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
    
    public List<PredatorFish> CurrentPredators { get; private set; } = new();

    #region Unity Callbacks

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        if (spawnAutomatically)
        {
            SpawnFlock();
        }
    }

    private void Update()
    {
        if (CurrentFishes.Count > 0)
        {
            BoidShader.Instance.PopulateAgentsWithData(this);
        }
    }

    #endregion

    public void FishSpawned(FlockableFish flockableFish)
    {
        CurrentFishes.Add(flockableFish);
    }

    public void FishKilled(FlockableFish flockableFish)
    {
        CurrentFishes.Remove(flockableFish);
    }
    
    #region Private Methods
    

    private void SpawnFlock()
    {
        CurrentFishes = new List<FlockableFish>();
        
        for (int i = 0; i < spawnAmount; i++)
        {
            Vector3 spawnPosition = Random.insideUnitSphere * spawnRange;
            FlockableFish newFish = Instantiate(fishPrefab);
            newFish.Spawn(spawnPosition + transform.position,spawnPosition.normalized, Quaternion.identity, transform, fishPrefab.gameObject);
        }
    }
    #endregion
}