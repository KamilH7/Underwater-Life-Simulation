using UnityEngine;

public class DrawDirections : MonoBehaviour
{
    private void OnDrawGizmos()
    {
        Vector3[] rayDirections = Values.Instance.GoldenRatioDirections;
        
        for (int i = 0; i < rayDirections.Length; i++)
        {
            float ColorValue = i * 0.002f;
            Gizmos.color = new Color(0, ColorValue,  ColorValue);
            Gizmos.DrawSphere(transform.position + rayDirections[i] * 5, 0.1f);
        }
    }
}
