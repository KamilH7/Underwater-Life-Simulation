using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class SimulationSpeedSlider : MonoBehaviour
    {
        [SerializeField]
        private Slider slider;
        
        private void Awake()
        {
            slider.onValueChanged.Invoke(slider.value);
        }

        public void ChangeSimulationSpeed(float value)
        {
            Time.timeScale = value;
        }
    }
}
