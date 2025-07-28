using System;
using System.Collections;
using UnityEngine;

// [Note: Ideally I would like to split this class in two, this is starting to carry the task of both the cannon and the tower. But we're too close to the jam end so I'll abstain from risking it]
namespace FifthModJam
{
    /// <summary>
    /// Controls the cannon launch.
    /// </summary>
    public class CannonLaunch : MonoBehaviour
    {
        // The handler on whether the cannon has power
        [SerializeField]
        private CannonPowerHandler _cannonPowerHandler;

        // The activating button
        [SerializeField]
        private InteractReceiver _interactReceiver;
        [SerializeField]
        private GearInterfaceEffects _gearInterface; // [Note: Currently unused. Initially meant for effects at button press]
        [SerializeField]
        private OWAudioSource _interactAudio;

        private bool _hasLaunched = false;

        private void VerifyUnityParameters()
        {
            if (_cannonPowerHandler == null)
            {
                FifthModJam.WriteLine("[CannonLaunch] cannon power hnadler is null", OWML.Common.MessageType.Error);
            }
            if (_interactReceiver == null)
            {
                FifthModJam.WriteLine("[CannonLaunch] interact receiver is null", OWML.Common.MessageType.Error);
            }
            if (_interactAudio == null)
            {
                FifthModJam.WriteLine("[CannonLaunch] audio is null", OWML.Common.MessageType.Error);
            }
        }

        private void Start()
        {
            VerifyUnityParameters();

            if (_interactReceiver != null)
            {
                _interactReceiver.OnPressInteract += OnPressInteract;
                _interactReceiver.SetPromptText(UITextType.RotateGearPrompt);
            }
        }
        private void OnDestroy()
        {
            if (_interactReceiver != null)
            {
                _interactReceiver.OnPressInteract -= OnPressInteract;
            }
        }

        private IEnumerator PlayAudio(bool hasPower)
        {
            if (hasPower)
            {
                _interactAudio?.PlayOneShot(global::AudioType.GearRotate_Heavy, 1f);
            }
            else
            {
                _interactAudio?.PlayOneShot(global::AudioType.GearRotate_Fail, 1f);
            }
            yield return new WaitForSeconds(1f);
        }

        private void OnPressInteract()
        {
            if (_hasLaunched) return; // Doesn't do anything after the tower has fallen

            Locator.GetShipLogManager().RevealFact("COSMICCURATORS_NOMAI_CANNON_NOPOWER"); // [Note: I put this out of the IsCannonPowered check, so that the fact is revealed even if the player launches after already powering the cannon]
            
            if (!_cannonPowerHandler.IsCannonPowered)
            {
                StartCoroutine(PlayAudio(hasPower: false));
            }
            else
            {
                _hasLaunched = true;
                TowerCollapseManager.Instance.CollapseTower();
                StartCoroutine(PlayAudio(hasPower: true));
            }
        }
    }
}