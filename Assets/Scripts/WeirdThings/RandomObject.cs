using UnityEngine;

public class RandomObject : MonoBehaviour
{
    [SerializeField]
    private WorldBound worldBound;

    private void Update()
    {
        Debug.Log(worldBound.GetResistance(transform.position));
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Enter");   
    }

    private void OnTriggerStay(Collider other)
    {
        Debug.Log("Stay");  
    }

    private void OnTriggerExit(Collider other)
    {
        Debug.Log("Exit");  
    }
}