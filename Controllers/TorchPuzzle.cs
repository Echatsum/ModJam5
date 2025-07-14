using System;
using UnityEngine;

namespace FifthModJam
{
    /// <summary>
    /// The torch puzzle manager.
    /// Puzzle is complete when all the torches are lit. It then activates some gameObjects.
    /// </summary>
    public class TorchPuzzle : MonoBehaviour
    {
        // Torches to lit for the puzzle to be completed
        [SerializeField]
        private FlameTrigger[] _torches;

        // Objects to enable once the puzzle is completed
        [SerializeField]
        private GameObject[] _objectsToEnable;

        // Audio and animator
        [SerializeField]
        private OWAudioSource _audio;
        [SerializeField]
        private Animator _beamAnim;

        // Beam starts inactive
        private bool _isBeamActive = false;
        public bool IsPuzzleCompleted => _isBeamActive;

        protected void VerifyUnityParameters()
        {
            if (_torches == null || _torches.Length == 0)
            {
                FifthModJam.WriteLine("[TorchPuzzle] torches array null or empty", OWML.Common.MessageType.Error);
            }
            if (_objectsToEnable == null || _objectsToEnable.Length == 0)
            {
                FifthModJam.WriteLine($"[TorchPuzzle] objectsToEnable array null or empty", OWML.Common.MessageType.Error);
            }
            if (_audio == null)
            {
                FifthModJam.WriteLine($"[TorchPuzzle] audio is null", OWML.Common.MessageType.Error);
            }
            if (_beamAnim == null)
            {
                FifthModJam.WriteLine($"[TorchPuzzle] animator is null", OWML.Common.MessageType.Error);
            }
        }

        private bool AreAllTorchesIgnited()
        {
            foreach (var torch in _torches)
            {
                if (!torch.IsIgnited)
                {
                    return false;
                }
            }

            return true;
        }

        private void Awake()
        {
            // Link each torch to OnTorchIgnition
            foreach (var torch in _torches)
            {
                torch.OnFlameIgnition = (FlameTrigger.FlameTriggerEvent)Delegate.Combine(torch.OnFlameIgnition, new FlameTrigger.FlameTriggerEvent(OnTorchIgnition));
            }
        }
        private void OnDestroy()
        {
            foreach (var torch in _torches)
            {
                torch.OnFlameIgnition = (FlameTrigger.FlameTriggerEvent)Delegate.Remove(torch.OnFlameIgnition, new FlameTrigger.FlameTriggerEvent(OnTorchIgnition));
            }
        }

        private void OnTorchIgnition()
        {
            if (!_isBeamActive) // No need to check again if the beam is already active
            {
                CheckActivation();
            }
        }

        private void Start()
        {
            VerifyUnityParameters();

            // Set objects to inactive (puzzle completion will activate them back)
            foreach (var enabledObject in _objectsToEnable)
            {
                enabledObject.SetActive(false);
            }
        }

        private void CheckActivation()
        {
            if (AreAllTorchesIgnited())
            {
                foreach (var disabledObject in _objectsToEnable)
                {
                    disabledObject.SetActive(true);
                }
                _audio?.PlayOneShot(global::AudioType.NomaiTractorBeamActivate, 0.5f);
                _beamAnim?.Play("BEAM", 0);

                _isBeamActive = true;
            }
        }
    }
}