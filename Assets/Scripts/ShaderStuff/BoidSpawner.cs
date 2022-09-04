using System.Collections.Generic;
using UnityEngine;

namespace Shaders
{

    public class BoidSpawner : MonoBehaviour
    {
        [Header("Settings")] [SerializeField] private Boid boidPrefab;
        [SerializeField] [Range(0, 10)] private float spawnRadius;
        [SerializeField] [Range(1, 1000)] private int boidAmount;

        public List<Boid> SpawnBoids()
        {
            List<Boid> boids = new List<Boid>();

            for (int i = 0; i < boidAmount; i++)
            {
                Vector3 rand = Random.insideUnitSphere;
                Boid boid = Instantiate(boidPrefab);
                boid.transform.position = transform.position + rand.normalized * spawnRadius;
                boid.transform.forward = rand.normalized;
                boids.Add(boid);
            }

            return boids;
        }
    }
}
