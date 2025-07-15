using NewHorizons.Utility;
using System;
using System.Collections;
using UnityEngine;

namespace FifthModJam
{
    public class TowerCollapse : MonoBehaviour
    {
        [SerializeField]
        private Animator towerAnim;
        [SerializeField]
        private Animator shuttleAnim;
        [SerializeField]
        private OWAudioSource shuttleAudio;
        [SerializeField]
        private OWAudioSource towerAudio;
        [SerializeField]
        private InteractReceiver _interactReceiver;

        [SerializeField]
        private GearInterfaceEffects _gearInterface;

        private Animator smallTower;
        public bool hasFallen;

        private void Start()
        {
            smallTower = SearchUtilities.Find("OminousOrbiter_Body/Sector/KarviShip_Interior/Interactibles/Diorama/Exhibit_STR/GhostExhibit/Structure/Tower/Tower_Pivot").GetComponent<Animator>();
            hasFallen = false;
            if (_interactReceiver != null)
            {
                _interactReceiver.OnPressInteract += OnPressInteract;
                _interactReceiver.SetPromptText(UITextType.RotateGearPrompt);
            }
        }

        public void ForceTowerFallenState()
        {
            smallTower.Play("TOWER_AFTER", 0);
            towerAnim.Play("TOWER_AFTER", 0);
        }

        private IEnumerator PlayAnim()
        {
            smallTower.Play("TOWER_AFTER", 0);
            shuttleAudio.PlayOneShot(global::AudioType.NomaiVesselPowerUp, 1f);
            shuttleAnim.Play("SHUTTLE", 0);
            yield return new WaitForSeconds(2f);
            shuttleAudio.PlayOneShot(global::AudioType.ShipImpact_LightDamage, 1f);
            towerAnim.Play("TOWER", 0);
            towerAudio.PlayOneShot(global::AudioType.Tower_RW_Fall_1, 1f);
            yield return new WaitForSeconds(0.75f);
            towerAudio.PlayOneShot(global::AudioType.Tower_RW_Fall_2, 1f);
            yield return new WaitForSeconds(1.4f);
            towerAudio.PlayOneShot(global::AudioType.GeneralDestruction, 1f);
            Locator.GetShipLogManager().RevealFact("MUSEUM_NOM_TOWER_E");
        }

        private void OnDestroy()
        {
            if (_interactReceiver != null)
            {
                _interactReceiver.OnPressInteract -= OnPressInteract;
            }
        }

        private void OnPressInteract()
        {
            if (_gearInterface != null && !hasFallen)
            {
                StartCoroutine(PlayAnim());
                hasFallen = true;
            }
        }
    }
}