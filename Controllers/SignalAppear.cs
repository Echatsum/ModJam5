using UnityEngine;

namespace FifthModJam
{
    public class SignalAppear : MonoBehaviour
    {
        [SerializeField]
        public GameObject signals;

        private bool signalsEnabled;

        private void Start()
        {
            if (HasDiscoveredSignals())
            {
                signals.SetActive(true);
                signalsEnabled = true;
            }
            else
            {
                signals.SetActive(false);
                signalsEnabled = false;
            }
        }

        private void Update()
        {
            if (!signalsEnabled && HasDiscoveredSignals())
            {
                signals.SetActive(true);
            }

            if (!HasDiscoveredSignals())
            {
                signals.SetActive(false);
            }
        }

        private bool HasDiscoveredSignals()
        {
            return Locator.GetShipLogManager().IsFactRevealed("MUSEUM_HEA_FIRSTSIGNAL_E");
        }
    }
}