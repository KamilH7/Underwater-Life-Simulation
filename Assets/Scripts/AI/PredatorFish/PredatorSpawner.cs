using Sirenix.OdinInspector;
using UnityEngine;

public class PredatorSpawner : MonoBehaviour
{
    [Header("Initialization Settings"), SerializeField]
    private bool spawnAutomatically = true;
    [SerializeField, ShowIf(nameof(spawnAutomatically))]
    private float spawnRange = 5;
    [SerializeField, ShowIf(nameof(spawnAutomatically))]
    private PredatorFish fishPrefab;
    [SerializeField, ShowIf(nameof(spawnAutomatically))]
    private int spawnAmount;

    private void Start()
    {
        if (spawnAutomatically)
        {
            SpawnPredators();
        }
    }

    private void SpawnPredators()
    {
        for (int i = 0; i < spawnAmount; i++)
        {
            Vector3 spawnPosition = Random.insideUnitSphere * spawnRange;
            PredatorFish newFish = Instantiate(fishPrefab);
            newFish.Spawn(spawnPosition + transform.position,spawnPosition.normalized, Quaternion.identity, transform, fishPrefab.gameObject);
            newFish.Initialize();
        }
    }
}
