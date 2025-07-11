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
        private OWAudioSource[] shuttleAudio = new OWAudioSource[2];
        [SerializeField]
        private OWAudioSource[] towerAudio = new OWAudioSource[3];
        [SerializeField]
        private InteractReceiver _interactReceiver;

        [SerializeField]
        private GearInterfaceEffects _gearInterface;

        private Animator smallTower; // [Note for @Anon: smallTower and towerAnim point to the same animator, what's the difference?]
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
            FifthModJam.WriteLine("playing shuttleAudio[0]", OWML.Common.MessageType.Debug);
            shuttleAudio[0].Play();
            shuttleAnim.Play("SHUTTLE", 0);
            yield return new WaitForSeconds(0.917f);
            FifthModJam.WriteLine("playing shuttleAudio[1]", OWML.Common.MessageType.Debug);
            shuttleAudio[1].Play();
            towerAnim.Play("TOWER", 0);
            FifthModJam.WriteLine("playing towerAudio[0]", OWML.Common.MessageType.Debug);
            towerAudio[0].Play();
            yield return new WaitForSeconds(0.75f);
            FifthModJam.WriteLine("playing towerAudio[1]", OWML.Common.MessageType.Debug);
            towerAudio[1].Play();
            yield return new WaitForSeconds(1.4f);
            FifthModJam.WriteLine("playing towerAudio[2]", OWML.Common.MessageType.Debug);
            towerAudio[2].Play();
            // ship reveal here maybe?
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
            if (_gearInterface != null && !hasFallen) // [Note: What is _gearInterface used for here?]
            {
                StartCoroutine(PlayAnim());
                hasFallen = true;
            }
        }
    }
}