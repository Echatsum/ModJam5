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
            kavAnim.Play("priedel2postidel", 0);
            yield return new WaitUntil(() =>
                kavAnim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1 &&
                !kavAnim.IsInTransition(0)
            );
            dialogue.SetActive(true);
        }
    }
}