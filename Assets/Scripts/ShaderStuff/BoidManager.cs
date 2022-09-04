using System.Collections.Generic;
using UnityEngine;

namespace Shaders
{

    public class BoidManager : MonoBehaviour
    {
        [Header("References")] [SerializeField]
        private BoidSpawner boidSpawner;

        [SerializeField] private ComputeShader boidComputeShader;
        [SerializeField] private BoidSettings boidSettings;

        private const int threadGroupSize = 1024;
        private List<Boid> boids = new List<Boid>();

        private void Start()
        {
            boids = boidSpawner.SpawnBoids();
            SetupComputeShader();
        }

        private void SetupComputeShader()
        {
            boidComputeShader.SetInt("totalNumberOfBoids", boids.Count);
            boidComputeShader.SetFloat("viewDistance", boidSettings.viewDistance);
            boidComputeShader.SetFloat("avoidanceDistance", boidSettings.safeDistance);
        }

        private void Update()
        {
            CalculateBoids();
            UpdateBoids();
            SetupComputeShader();
        }

        private void UpdateBoids()
        {
            foreach (Boid boid in boids)
            {
                boid.UpdateBoid();
            }
        }

        private void CalculateBoids()
        {
            ComputeBuffer buffer = SetupBuffers();

            LaunchComputeShader();

            BoidData[] calculatedBoidData = new BoidData[boids.Count];
            buffer.GetData(calculatedBoidData);
            PopulateBoidsWithNewData(calculatedBoidData);
            buffer.Release();
        }

        private BoidData[] GetBoidData()
        {
            BoidData[] data = new BoidData[boids.Count];

            for (int i = 0; i < boids.Count; i++)
            {
                data[i].position = boids[i].transform.position;
                data[i].displacementVector = boids[i].transform.forward;
            }

            return data;
        }

        private void LaunchComputeShader()
        {
            boidComputeShader.Dispatch(0, 1, 1, 1);
        }

        private void PopulateBoidsWithNewData(BoidData[] newData)
        {
            for (int i = 0; i < boids.Count; i++)
            {
                boids[i].avgPosition = newData[i].avgPosition;
                boids[i].avgDisplacementVector = newData[i].avgDisplacementVector;
                boids[i].avgAvoidanceDisplacementVector = newData[i].avgAvoidanceDisplacementVector;
            }
        }

        private ComputeBuffer SetupBuffers()
        {
            ComputeBuffer buffer = PopulateBufferWithData(GetBoidData());
            AssignBufferToComputeShader(buffer);

            return buffer;
        }

        private void AssignBufferToComputeShader(ComputeBuffer buffer)
        {
            boidComputeShader.SetBuffer(0, "boids", buffer);
        }

        private ComputeBuffer PopulateBufferWithData(BoidData[] data)
        {
            ComputeBuffer boidBuffer = new ComputeBuffer(data.Length, BoidData.Size);
            boidBuffer.SetData(data);
            return boidBuffer;
        }

        private BoidData[] getDataFromBuffer;

        private struct BoidData
        {
            public Vector3 position;
            public Vector3 displacementVector;

            public Vector3 avgPosition;
            public Vector3 avgDisplacementVector;
            public Vector3 avgAvoidanceDisplacementVector;


            public static int Size
            {
                get { return sizeof(float) * 3 * 5; }
            }
        }
    }
}