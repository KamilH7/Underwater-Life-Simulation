using UnityEngine;

namespace UI
{
    public class SimulationSpeedSlider : MonoBehaviour
    {
        public void ChangeSimulationSpeed(float value)
        {
            Time.timeScale = value;
        }
    }
}
