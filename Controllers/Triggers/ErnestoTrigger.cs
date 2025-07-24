using System.Collections;
using UnityEngine;

namespace FifthModJam
{
    public class ErnestoTrigger : MonoBehaviour
    {
        [SerializeField]
        private OWAudioSource ghostAudio;
        [SerializeField]
        private GameObject[] ghostDialogue;
        [SerializeField]
        private GameObject ernesto;
        [SerializeField]
        private ErnestoAnim ernestoAnim;
        [SerializeField]
        private GameObject ernestoDialogue;
        [SerializeField]
        private GameObject deathVol;
        [SerializeField]
        private Animator[] worshipperAnim;

        private bool hasEnteredTrigger;
        private bool hasActivatedDeathVol;

        // scaling stuff
        private Vector3 targetScale = new Vector3(0.015f, 0.015f, 0.015f);
        private float lerpDuration = 1.5f;

        private void Start()
        {
            hasEnteredTrigger = false;
            hasActivatedDeathVol = false;
            ernesto.SetActive(false);
            ernestoDialogue.SetActive(false);
            deathVol.SetActive(false);
        }

        public virtual void OnTriggerEnter(Collider hitCollider)
        {
            //checks if player collides with the trigger volume
            if (hitCollider.CompareTag("PlayerDetector") && enabled && !hasEnteredTrigger)
            {
                hasEnteredTrigger = true;
                StartCoroutine(SummonErnesto());
            } 
        }

        private void Update()
        {
            if (IsErnestoPissed())
            {
                if (!hasActivatedDeathVol)
                {
                    ernestoAnim.audio.PlayOneShot(global::AudioType.DBAnglerfishOpeningMouth, 0.5f);
                    hasActivatedDeathVol = true;
                    deathVol.SetActive(true);
                }
            }
        }

        private bool IsErnestoPissed()
        {
            return DialogueConditionManager.SharedInstance.GetConditionState("ErnestoPissed");
        }

        private IEnumerator SummonErnesto()
        {
            for (int i = 0; i < worshipperAnim.Length; i++)
            {
                ghostDialogue[i].SetActive(false);
                worshipperAnim[i].Play("STR_SACRIFICE", 0);
            }
            ghostAudio.PlayOneShot(global::AudioType.Ghost_DeathGroup, 0.8f);
            yield return new WaitForSeconds(2); // wait until ghosts die
            float elapsedTime = 0f;
            ernesto.SetActive(true);
            while (elapsedTime < lerpDuration)
            {
                ernesto.transform.localScale = Vector3.Lerp(ernesto.transform.localScale, targetScale, elapsedTime / lerpDuration);
                elapsedTime += Time.deltaTime;
                yield return null; // wait for the next frame
            }
            ernesto.transform.localScale = targetScale; // ensure it reaches the target scale
            ernestoAnim._animator.Play("anglernew_idle", 0);
            ernestoAnim.OnChangeAnglerState(AnglerfishController.AnglerState.Chasing);
            yield return new WaitForSeconds(2); // wait until ernesto stops making noise
            ernestoAnim.OnChangeAnglerState(AnglerfishController.AnglerState.Lurking);
            ernestoDialogue.SetActive(true);
        }
    }
}