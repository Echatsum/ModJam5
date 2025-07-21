using NewHorizons.Utility.Files;
using System.Collections;
using UnityEngine;

namespace FifthModJam
{
    public class EndingTrigger2 : MonoBehaviour
    {
        [SerializeField]
        private Animator kavAnim;
        [SerializeField]
        private FlameTrigger[] torches;
        [SerializeField]
        private OWAudioSource oneShot;
        [SerializeField]
        private OWAudioSource audio;
        [SerializeField]
        private GameObject dialogue;

        private bool _hasAlreadyTriggeredEnding = false;

        private void Start()
        {
            dialogue?.SetActive(false);
        }

        public virtual void OnTriggerEnter(Collider hitCollider)
        {
            // If the ending has already been triggered, we don't need to do anything
            if (_hasAlreadyTriggeredEnding) return;

            // Checks if player collides with the trigger volume
            if (hitCollider.CompareTag("PlayerDetector") && enabled)
            {
                StartCoroutine(KavIntroduction());
                _hasAlreadyTriggeredEnding = true;
            } 
        }

        public IEnumerator KavIntroduction()
        {
            foreach (FlameTrigger torch in torches)
            {
                torch.IgniteFlame();
            }
            if (audio != null && !audio.loop)
            {
                audio.loop = true;
            }
            kavAnim.Play("preidel2postidel", 0);
            oneShot?.PlayOneShot(global::AudioType.Door_CloseStart, 1f);
            yield return new WaitForSeconds(1.4167f);
            audio?.AssignAudioLibraryClip(global::AudioType.MovementMetalFootstep);
            audio?.Play();
            yield return new WaitForSeconds(2.5833f);
            audio?.Stop();
            dialogue.SetActive(true);
        }
    }
}