using UnityEngine;

namespace FifthModJam
{
    public class LearnLangTrigger : MonoBehaviour
    {
        private bool HasLearnedLang()
        {
            return Locator.GetShipLogManager().IsFactRevealed("SHIP_CRYSTAL_E1");
        }

        public virtual void OnTriggerEnter(Collider hitCollider)
        {
            //checks if player collides with the trigger volume
            if (hitCollider.CompareTag("PlayerDetector") && enabled && !HasLearnedLang())
            {
                Locator.GetShipLogManager().RevealFact("SHIP_CRYSTAL_E1");
            } 
        }
    }
}