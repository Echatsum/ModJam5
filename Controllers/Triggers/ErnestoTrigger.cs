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

        // scaling stuff
        private Vector3 initialScale = new Vector3(0.00015f, 0.00015f, 0.00015f);
        private Vector3 targetScale = new Vector3(0.015f, 0.015f, 0.015f);
        private float lerpDuration = 1.5f;

        private bool _isErnestoGrowling = false;
        private const float ERNESTO_GROWL_TIME = 0.5f; // To put in constants later

        private void Start()
        {
            hasEnteredTrigger = false;
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
            if (!deathVol.activeSelf && IsErnestoPissed())
            {
                deathVol.SetActive(true);                
            }

            if (!_isErnestoGrowling && ShouldErnestoGrowl())
            {
                _isErnestoGrowling = true;
                StartCoroutine(ErnestoGrowl());            
            }
        }

        private bool IsErnestoPissed()
        {
            return DialogueConditionManager.SharedInstance.GetConditionState("ErnestoPissed");
        }
        private bool ShouldErnestoGrowl()
        {
            return DialogueConditionManager.SharedInstance.GetConditionState("ErnestoGrowling"); // [Note: maybe a bit ugly to work with a condition outside the script, but should work well]
        }

        private IEnumerator SummonErnesto()
        {
            // Kill ghosts
            for (int i = 0; i < worshipperAnim.Length; i++)
            {
                ghostDialogue[i].SetActive(false);
                worshipperAnim[i].Play("STR_SACRIFICE", 0);
            }
            ghostAudio.PlayOneShot(global::AudioType.Ghost_DeathGroup, 0.8f);
            yield return new WaitForSeconds(2); // wait until ghosts die

            // Activate and grow Ernesto
            ernesto.SetActive(true);
            ernesto.transform.localScale = initialScale;
            float elapsedTime = 0f;
            while (elapsedTime < lerpDuration)
            {
                ernesto.transform.localScale = Vector3.Lerp(ernesto.transform.localScale, targetScale, elapsedTime / lerpDuration); // [Not quite a lerp, since we reuse the current value instead of initial (this speeds things up)]
                elapsedTime += Time.deltaTime;
                yield return null; // wait for the next frame
            }
            ernesto.transform.localScale = targetScale; // ensure it reaches the target scale

            // Roar
            ernestoAnim._animator.Play("anglernew_idle", 0);
            ernestoAnim.OnChangeAnglerState(AnglerfishController.AnglerState.Chasing);
            yield return new WaitForSeconds(2); // wait until ernesto stops making noise

            // Calm down
            ernestoAnim.OnChangeAnglerState(AnglerfishController.AnglerState.Lurking);
            ernestoDialogue.SetActive(true);
        }

        private IEnumerator ErnestoGrowl()
        {
            ernestoAnim.audio.PlayOneShot(global::AudioType.DBAnglerfishOpeningMouth, ERNESTO_GROWL_TIME);
            yield return new WaitForSeconds(ERNESTO_GROWL_TIME);

            DialogueConditionManager.SharedInstance.SetConditionState("ErnestoGrowling", false);
            _isErnestoGrowling = false;
        }
    }
}