using UnityEngine;

namespace FifthModJam.Controllers
{
    public class TheatreLightsHandler : MonoBehaviour
    {
        [SerializeField]
        private GameObject[] lights;
        [SerializeField]
        private SingleLightSensor slideTotemSensor;
        private bool isOn;
        private bool isOff;

        public void Start()
        {
            isOn = false;
            isOff = true;
        }

        private void Update()
        {
            if (!isOn && slideTotemSensor.IsIlluminated() && slideTotemSensor._lightSourceMask == LightSourceType.SIMPLE_LANTERN)
            {
                isOn = true;
                isOff = false;
                foreach (var light in lights)
                {
                    light.SetActive(true);
                }
            }

            if (!isOff && !slideTotemSensor.IsIlluminated())
            {
                isOn = false;
                isOff = true;
                foreach (var light in lights)
                {
                    light.SetActive(false);
                }
            }
        }
    }
}