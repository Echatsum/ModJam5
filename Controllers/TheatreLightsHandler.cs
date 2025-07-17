using UnityEngine;

namespace FifthModJam
{
    public class TheatreLightsHandler : MonoBehaviour
    {
        [SerializeField]
        private GameObject[] lights;
        [SerializeField]
        private SingleLightSensor slideTotemSensor;
        private bool isOn;
        private bool isOff;

        private void VerifyUnityParameters()
        {
            if (lights == null || lights.Length == 0)
            {
                FifthModJam.WriteLine("[TheatreLightsHandler] light array is null or empty", OWML.Common.MessageType.Error);
            }
            if (slideTotemSensor == null)
            {
                FifthModJam.WriteLine("[TheatreLightsHandler] slideTotemSensor is null", OWML.Common.MessageType.Error);
            }
        }

        private void Start()
        {
            VerifyUnityParameters();
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