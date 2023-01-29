using System.Collections;
using UI;
using UnityEngine;

public class GraphDataPopulator : MonoBehaviour
{
    [field: SerializeField, Range(0.1f, 1f)]
    private float UpdateFrequency { get; set; }
    [field: SerializeField]
    private GameObjectCountGraph Graph { get; set; }
    [field: SerializeField]
    private Transform ObjectParent { get; set; }

    private void Start()
    {
        StartCoroutine(UpdateDataCoroutine());
    }

    private IEnumerator UpdateDataCoroutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(UpdateFrequency);
            Graph.AddData(ObjectParent.childCount);
        }
    }
}
