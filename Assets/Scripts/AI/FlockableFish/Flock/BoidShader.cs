using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BoidShader", menuName = "Singletons/BoidShader")]
public class BoidShader : SingletonScriptableObject<BoidShader>
{
    #region Serialized Fields

    [SerializeField]
    private ComputeShader boidComputeShader;

    #endregion

    #region Public Methods

    public void PopulateAgentsWithData(Flock flock)
    {
        int flockSize = flock.CurrentFishes.Count;
        List<FlockableFish> agents = flock.CurrentFishes;

        //Initialize static values inside the shader
        InitializeShaderValues(flockSize, flock.ViewDistance, flock.SafeDistance);

        //Create a buffer and populate it with starting data
        AgentData[] agentData = GetDataFromFlock(agents);
        ComputeBuffer buffer = PopulateBufferWithData(agentData);

        //Assign the buffer to the shader
        AssignBufferToComputeShader(buffer);

        //Calculate the data
        LaunchComputeShader();

        //Populate the old data with calculated data
        buffer.GetData(agentData);

        //Assign the new data to current fishes
        PopulateBoidsWithNewData(agentData, agents);

        //Release the buffer
        buffer.Release();
    }

    #endregion

    #region Private Methods

    private void InitializeShaderValues(int flockSize, float viewDistance, float safeDistance)
    {
        boidComputeShader.SetInt("totalNumberOfBoids", flockSize);
        boidComputeShader.SetFloat("viewDistance", viewDistance);
        boidComputeShader.SetFloat("avoidanceDistance", safeDistance);
    }

    private AgentData[] GetDataFromFlock(List<FlockableFish> flockableFishes)
    {
        AgentData[] data = new AgentData[flockableFishes.Count];

        for (int i = 0; i < flockableFishes.Count; i++)
        {
            data[i].position = flockableFishes[i].transform.position;
            data[i].displacementVector = flockableFishes[i].transform.forward;
        }

        return data;
    }

    private void LaunchComputeShader()
    {
        boidComputeShader.Dispatch(0, 1, 1, 1);
    }

    private void PopulateBoidsWithNewData(AgentData[] newAgentData, List<FlockableFish> agents)
    {
        for (int i = 0; i < agents.Count; i++)
        {
            agents[i].UpdateBehaviourValues(newAgentData[i].avgPosition,newAgentData[i].avgDisplacementVector,newAgentData[i].avgAvoidanceDisplacementVector);
        }
    }

    private void AssignBufferToComputeShader(ComputeBuffer buffer)
    {
        boidComputeShader.SetBuffer(0, "boids", buffer);
    }

    private ComputeBuffer PopulateBufferWithData(AgentData[] data)
    {
        ComputeBuffer boidBuffer = new(data.Length, AgentData.Size);
        boidBuffer.SetData(data);

        return boidBuffer;
    }

    #endregion
}