using UnityEngine;

namespace FifthModJam
{
    public class LearnLangTrigger : MonoBehaviour
    {
        [SerializeField]
        private Animator crystalFX;
        private bool hasEntered;
        private bool HasLearnedLang()
        {
            return Locator.GetShipLogManager().IsFactRevealed("SHIP_CRYSTAL_E1");
        }

        private void Start()
        {
            hasEntered = false;
        }

        public virtual void OnTriggerEnter(Collider hitCollider)
        {
            //checks if player collides with the trigger volume
            if (hitCollider.CompareTag("PlayerDetector") && enabled)
            {
                if (!hasEntered)
                {
                    crystalFX.Play("CrystalFade", 0);
                    hasEntered = true;
                }
                
                if (!HasLearnedLang())
                {
                    Locator.GetShipLogManager().RevealFact("SHIP_CRYSTAL_E1");
                }
            } 
        }
    }
}