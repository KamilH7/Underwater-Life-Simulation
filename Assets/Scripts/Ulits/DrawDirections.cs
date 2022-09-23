using UnityEngine;

public class DrawDirections : MonoBehaviour
{
    private void OnValidate()
    {
        Draw();
    }

    private void Draw()
    {
        Vector3[] rayDirections = Values.Instance.GoldenRatioDirections;

        for (int i = 0; i < rayDirections.Length; i++)
        {
            Debug.DrawLine(transform.position, transform.position + rayDirections[i] * 5, Color.black, 1000);
        }
    }
}
