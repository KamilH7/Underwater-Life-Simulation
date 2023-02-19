using TMPro;
using UnityEngine;

public class TimeText : MonoBehaviour
{
    [SerializeField]
    private TMP_Text targetText;

    private float currentTime;
    private void Update()
    {
        currentTime += Time.deltaTime;
        UpdateTimer(currentTime);
    }

    private void UpdateTimer(float timeInSeconds)
    {
        int minutes = Mathf.FloorToInt(timeInSeconds / 60);
        int seconds = Mathf.FloorToInt(timeInSeconds % 60);
        
        targetText.text = $"{GetText(minutes)}:{GetText(seconds)}";
    }

    private string GetText(int value)
    {
        string valueText = value.ToString();
        return valueText.Length == 1 ? valueText.Insert(0, "0") : valueText;
    }
}
