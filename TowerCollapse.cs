using NewHorizons.Utility;
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

        public void ForceTowerFall()
        {
            smallTower.Play("TOWER_AFTER", 0);
            towerAnim.Play("TOWER_AFTER", 0);
        }

        private IEnumerator PlayAnim()
        {
            smallTower.Play("TOWER_AFTER", 0);
            //shuttleAudio[0].Play(); // this line gives index out of bounds exception, yet all the values are there in unity explorer.
            shuttleAnim.Play("SHUTTLE", 0);
            yield return new WaitForSeconds(0.917f);
            //shuttleAudio[1].Play();
            towerAnim.Play("TOWER", 0);
            //towerAudio[0].Play();
            yield return new WaitForSeconds(0.75f);
            //towerAudio[1].Play();
            yield return new WaitForSeconds(1.4f);
            //towerAudio[2].Play();
            hasFallen = true;
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
            if (_gearInterface != null && !hasFallen)
            {
                StartCoroutine(PlayAnim());
            }
        }
    }
}