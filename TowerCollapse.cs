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

        private bool hasBegunFalling;

        private void Start()
        {
            hasBegunFalling = false;
            if (_interactReceiver != null)
            {
                _interactReceiver.OnPressInteract += OnPressInteract;
                _interactReceiver.SetPromptText(UITextType.RotateGearPrompt);
            }
        }

        private IEnumerator PlayAnim()
        {
            shuttleAudio[0].Play(); // this line gives index out of bounds exception, yet all the values are there in unity explorer.
            shuttleAnim.Play("SHUTTLE", 0);
            yield return new WaitForSeconds(0.917f);
            shuttleAudio[1].Play();
            towerAnim.Play("TOWER", 0);
            towerAudio[0].Play();
            yield return new WaitForSeconds(0.75f);
            towerAudio[1].Play();
            yield return new WaitForSeconds(1.4f);
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
            if (_gearInterface != null && !hasBegunFalling)
            {
                hasBegunFalling = true;
                StartCoroutine(PlayAnim());
            }
        }
    }
}