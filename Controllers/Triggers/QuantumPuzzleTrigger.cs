using UnityEngine;

namespace FifthModJam
{
    public class QuantumPuzzleTrigger : MonoBehaviour
    {
        // The index of this socket
        [SerializeField]
        private int _socketIndex;

        // The Quantum puzzle controller to notify
        [SerializeField]
        private QuantumPuzzle _quantumPuzzle;
        
        // The two colliders we care about
        private bool _isPlayerInside = false;
        private bool _isProbeInside = false;

        private void VerifyUnityParameters()
        {
            if (_socketIndex == 0)
            {
                FifthModJam.WriteLine("[QuantumPuzzleTrigger] socket index is zero", OWML.Common.MessageType.Error);
            }
            if (_quantumPuzzle == null)
            {
                FifthModJam.WriteLine("[QuantumPuzzleTrigger] quantumPuzzle is null", OWML.Common.MessageType.Error);
            }
        }
        private void Start()
        {
            VerifyUnityParameters();
            this.enabled = false;
        }

        private void Update()
        {
            // Dirty way to check if the probe is still there
            var probe = Locator.GetProbe();
            if (probe == null || !probe.IsLaunched())
            {
                _isProbeInside = false;
                _quantumPuzzle.SetSocketBlockedByProximity(_socketIndex, _isPlayerInside); // still blocked if player inside
                this.enabled = false;
            }
        }

        public virtual void OnTriggerEnter(Collider hitCollider)
        {
            // Check if player
            if (hitCollider.CompareTag("PlayerDetector"))
            {
                _isPlayerInside = true;
                _quantumPuzzle.SetSocketBlockedByProximity(_socketIndex, true);
            }
            // Check if scout probe
            else if (hitCollider.CompareTag("ProbeDetector"))
            {
                _isProbeInside = true;
                _quantumPuzzle.SetSocketBlockedByProximity(_socketIndex, true);
                this.enabled = true;
            }
        }
        public virtual void OnTriggerExit(Collider hitCollider)
        {
            // Check if player
            if (hitCollider.CompareTag("PlayerDetector"))
            {
                _isPlayerInside = false;
                _quantumPuzzle.SetSocketBlockedByProximity(_socketIndex, _isProbeInside); // still blocked if probe inside
            }
            // Check if scout probe
            else if (hitCollider.CompareTag("ProbeDetector")) // [Note: this does NOT include scout recall, only the scout flying out..]
            {
                _isProbeInside = false;
                _quantumPuzzle.SetSocketBlockedByProximity(_socketIndex, _isPlayerInside); // still blocked if player inside
            }
        }
    }
}
