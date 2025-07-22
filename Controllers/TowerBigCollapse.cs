using System;
using System.Collections;
using UnityEngine;

// [Note: Ideally I would like to split this class in two, this is starting to carry the task of both the cannon and the tower. But we're too close to the jam end so I'll abstain from risking it]
namespace FifthModJam
{
    /// <summary>
    /// Controls the collapse of the big tower (when the player is in the exhibits).
    /// </summary>
    public class TowerBigCollapse : MonoBehaviour
    {
        // Audio and animator
        [SerializeField]
        private Animator _towerAnim;
        [SerializeField]
        private OWAudioSource _towerAudio;
        [SerializeField]
        private Animator _shuttleAnim;
        [SerializeField]
        private OWAudioSource _shuttleAudio;

        // The handler on whether the cannon has power
        [SerializeField]
        private CannonPowerHandler _cannonPowerHandler;

        // The interact audiosource

        // The activating button
        [SerializeField]
        private InteractReceiver _interactReceiver;
        [SerializeField]
        private GearInterfaceEffects _gearInterface; // [Note: Currently unused. Initially meant for effects at button press]
        [SerializeField]
        private OWAudioSource _interactAudio;

        private bool _hasFallen = false;

        private void VerifyUnityParameters()
        {
            if (_towerAnim == null)
            {
                FifthModJam.WriteLine("[TowerBigCollapse] tower animator is null", OWML.Common.MessageType.Error);
            }
            if (_towerAudio == null)
            {
                FifthModJam.WriteLine("[TowerBigCollapse] tower audio is null", OWML.Common.MessageType.Error);
            }
            if (_shuttleAnim == null)
            {
                FifthModJam.WriteLine("[TowerBigCollapse] shuttle animator is null", OWML.Common.MessageType.Error);
            }
            if (_shuttleAudio == null)
            {
                FifthModJam.WriteLine("[TowerBigCollapse] shuttle audio is null", OWML.Common.MessageType.Error);
            }
            if (_cannonPowerHandler == null)
            {
                FifthModJam.WriteLine("[TowerBigCollapse] cannon power hnadler is null", OWML.Common.MessageType.Error);
            }
            if (_interactReceiver == null)
            {
                FifthModJam.WriteLine("[TowerBigCollapse] interact receiver is null", OWML.Common.MessageType.Error);
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
        private void OnEnable()
        {
            // When we reenter the exhibit, the animator resets. This forces it back to the fallen state
            // [Note: _hasFallen starts false, so there is no risk we mess with things before initialization]
            if (_hasFallen)
            {
                ForceTowerFallenState();
            }
        }

        private void OnDestroy()
        {
            if (_interactReceiver != null)
            {
                _interactReceiver.OnPressInteract -= OnPressInteract;
            }
        }

        private void ForceTowerFallenState()
        {
            _towerAnim?.Play("TOWER_AFTER", 0);
        }

        private IEnumerator PlayNoPowerAnim()
        {
            _interactAudio?.PlayOneShot(global::AudioType.GearRotate_Fail, 1f); // Change the sound here
            yield return new WaitForSeconds(1f);
        }

        private IEnumerator PlayAnim()
        {
            // Launch shuttle
            _interactAudio?.PlayOneShot(global::AudioType.GearRotate_Heavy, 1f); // Change the sound here
            _shuttleAudio?.PlayOneShot(global::AudioType.NomaiVesselPowerUp, 1f);
            _shuttleAnim?.Play("SHUTTLE", 0);
            yield return new WaitForSeconds(2f);
            _shuttleAudio?.PlayOneShot(global::AudioType.ShipImpact_LightDamage, 1f);

            // Tower is hit, creaks and falls
            _shuttleAudio?.PlayOneShot(global::AudioType.ShipImpact_LightDamage, 1f);
            _towerAnim?.Play("TOWER", 0);
            _towerAudio?.PlayOneShot(global::AudioType.Tower_RW_Fall_1, 1f);
            yield return new WaitForSeconds(0.75f);
            _towerAudio?.PlayOneShot(global::AudioType.Tower_RW_Fall_2, 1f);
            yield return new WaitForSeconds(1.4f);
            _towerAudio?.PlayOneShot(global::AudioType.GeneralDestruction, 1f);
        }

        private void OnPressInteract()
        {
            if (_hasFallen) return; // Doesn't do anything after the tower has fallen

            Locator.GetShipLogManager().RevealFact("COSMICCURATORS_NOMAI_CANNON_NOPOWER"); // [Note: I put this out of the IsCannonPowered check, so that the fact is revealed even if the player launches after already powering the cannon]
            if (!_cannonPowerHandler.IsCannonPowered)
            {
                StartCoroutine(PlayNoPowerAnim());
                return;
            }

            _hasFallen = true;
            TowerCollapseManager.Instance.CollapseTower();
            Locator.GetShipLogManager().RevealFact("COSMICCURATORS_INHABITANT_TOWER_COLLAPSE_R");
            StartCoroutine(PlayAnim());
        }
    }
}