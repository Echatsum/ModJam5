using UnityEngine;

namespace FifthModJam
{
    /// <summary>
    /// The controller that turns on the signals once the first scout signal is identified
    /// </summary>
    public class SignalAppear : MonoBehaviour
    {
        // The gameObject that parents all the signals
        [SerializeField]
        private GameObject _signals;

        private void VerifyUnityParameters()
        {
            if (_signals == null)
            {
                FifthModJam.WriteLine("[SignalAppear] signals object null", OWML.Common.MessageType.Error);
            }
        }

        private void Start()
        {
            VerifyUnityParameters();

            if (!HasDiscoveredSignals())
            {
                _signals.SetActive(false);
            }

            GlobalMessenger.AddListener("ShipLogUpdated", OnShipLogUpdated);
        }
        private void OnDestroy()
        {
            GlobalMessenger.RemoveListener("ShipLogUpdated", OnShipLogUpdated);
        }

        private void OnShipLogUpdated()
        {
            if (_signals.activeSelf) return; // If the signals are already active we don't need to update

            // While the ShipLogUpdated event firing doesn't say which event, we can check the one we care about
            if (HasDiscoveredSignals())
            {
                _signals.SetActive(true);
            }
        }

        private bool HasDiscoveredSignals()
        {
            return Locator.GetShipLogManager().IsFactRevealed("MUSEUM_HEA_FIRSTSIGNAL_E");
        }
    }
}