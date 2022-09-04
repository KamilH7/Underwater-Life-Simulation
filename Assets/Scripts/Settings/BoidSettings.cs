using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class BoidSettings : ScriptableObject 
{
    [Header("Behaviour Settings")]

    [Range(0,1)]
    public float alignmentBehaviour;
    public float AlignmentBehaviour { 
        get => alignmentBehaviour; 
        set => alignmentBehaviour = value; 
    }

    [Range(0, 1)]
    public float cohesionBehaviour;
    public float CohesionBehaviour
    {
        get => cohesionBehaviour;
        set => cohesionBehaviour = value;
    }

    [Range(0, 1)]
    public float separationBehaviour;
    public float SeparationBehaviour
    {
        get => separationBehaviour;
        set => separationBehaviour = value;
    }

    public bool environmentCollision = true;

    [Header("General")]
    public float minSpeed = 2;
    public float maxSpeed = 5;
    public float steerForce = 5;

    [Range(0, 40)]
    public float viewDistance = 5f;
    public float ViewDistance
    {
        get => viewDistance;
        set => viewDistance = value;
    }
    [Range(0, 5)]
    public float safeDistance = 1;
    public float AvoidanceDistance
    {
        get => safeDistance;
        set => safeDistance = value;
    }

    [Header ("Collision Avoidance Settings")]
    public LayerMask obstacleLayer;
    public float boundsRadius = 0.2f;
    public float collisionAvoidDst;
}