using UnityEngine;

namespace FifthModJam
{
    /// <summary>
    /// Trigger placed on a torch object. As expected with fire behavior, can be lit and/or light up a flammable held object.
    /// </summary>
    public class FlameTrigger : MonoBehaviour
    {
        // Neat way to share about the ignition
        public delegate void FlameTriggerEvent();
        public FlameTriggerEvent OnFlameIgnition;

        // Whether The flame is on (and can be used to ignite the pole item)
        [SerializeField]
        private bool _isIgnited;
        public bool IsIgnited => _isIgnited;

        // The torch itself
        [SerializeField]
        private GameObject _torchFlame;

        // Audio and animator
        [SerializeField]
        private OWAudioSource _audio;
        [SerializeField]
        private Animator _animator; // Can be left null if ignited from the start

        protected void VerifyUnityParameters()
        {
            if (_torchFlame == null)
            {
                FifthModJam.WriteLine($"[FlameTrigger] torchFlame is null", OWML.Common.MessageType.Error);
            }
            if (_audio == null && !_isIgnited)
            {
                FifthModJam.WriteLine($"[FlameTrigger] audio is null while torch is not lit", OWML.Common.MessageType.Error);
            }
            if (_animator == null && !_isIgnited)
            {
                FifthModJam.WriteLine("[FlameTrigger] animator is null while torch is not lit", OWML.Common.MessageType.Error);
            }
        }

        private void Start()
        {
            VerifyUnityParameters();

            _torchFlame?.SetActive(_isIgnited);
        }

        public virtual void OnTriggerEnter(Collider hitCollider)
        {
            // Checks if player collides with the trigger volume
            if (hitCollider.CompareTag("PlayerDetector") && enabled)
            {
                // Try to get pole. If the held item (if any) isn't the flammable pole so we can exit the method
                var poleItem = GetPole();
                if (poleItem == null) return;

                var isPoleIgnited = poleItem.IsIgnited;

                // Case 1: The torch ignites the unlit pole
                if (_isIgnited && !isPoleIgnited)
                {
                    poleItem.ToggleFlames(true);
                }
                // Case 2: The flaming pole ignites the unlit torch
                else if (!_isIgnited && isPoleIgnited)
                {
                    IgniteFlame();
                }
            } 
        }

        private void IgniteFlame()
        {
            Locator.GetShipLogManager().RevealFact("COSMICCURATORS_VOLCANO_SUMMIT_IGNITE");

            _isIgnited = true;
            _torchFlame?.SetActive(true);
            _animator?.Play("FLAME", 0);
            PlayAudio(true);

            // This will tell objects that are linked, that this torch is now lit [Note: Namely, the torch puzzle]
            OnFlameIgnition?.Invoke();
        }

        public FlammablePoleItem GetPole()
        {
            // Returns the held item if it is a FlammablePoleItem, otherwise null
            return (Locator.GetToolModeSwapper()?.GetItemCarryTool()?.GetHeldItem() is FlammablePoleItem item) ? item : null;
        }

        private void PlayAudio(bool isIgniting)
        {

            if (isIgniting)
            {
                _audio?.PlayOneShot(global::AudioType.TH_Campfire_Ignite, 0.5f);
            }
            else
            {
                // [Note: It doesn't seem the lit torches can be extinguished so this code is unused, but it's fine to keep code like this in case we want to change that]

                _audio?.PlayOneShot(global::AudioType.Artifact_Extinguish, 0.5f);
            }
        }
    }
}