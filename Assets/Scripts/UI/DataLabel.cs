using TMPro;
using UnityEngine;

namespace UI
{
    public class DataLabel : MonoBehaviour
    {
        [field: SerializeField]
        private TMP_Text DataText { get; set; }

        public void UpdateData(float value)
        {
            DataText.text = value.ToString();
        }
        
        public void UpdateData(int value)
        {
            DataText.text = value.ToString();
        }
        
        public void UpdateData(string value)
        {
            DataText.text = value;
        }
    }
}
