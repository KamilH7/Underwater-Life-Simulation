using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class Menu : MonoBehaviour
{
    [SerializeField]
    BoidSettings settings;
    [SerializeField]
    Slider cohesion;
    [SerializeField]
    Slider alignment;
    [SerializeField]
    Slider separation;
    [SerializeField]
    Slider viewDistance;
    [SerializeField]
    Slider separationDistance;

    public void Start()
    {
        ResetSliders();
    }

    public void ResetSimulation()
    {
        SceneManager.LoadScene(0);
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void ResetSliders()
    {
        cohesion.value = settings.cohesionBehaviour;
        alignment.value = settings.alignmentBehaviour;
        separation.value = settings.separationBehaviour;
        viewDistance.value = settings.viewDistance;
        separationDistance.value = settings.safeDistance;
    }
}
