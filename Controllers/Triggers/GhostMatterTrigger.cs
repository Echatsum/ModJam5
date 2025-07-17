using UnityEngine;

namespace FifthModJam
{
    public class GhostMatterTrigger : MonoBehaviour
    {
        [SerializeField]
        private GeyserController geyser;
        [SerializeField]
        private DarkMatterVolume[] ghostMatter;
        private const float defaultFirstContact = 70;
        private const float defaultDPS = 80;
        private bool isInVolume;
        private bool hasResetData;

        public void Start()
        {
            isInVolume = false;
            hasResetData = false;
        }

        public void Update()
        {
            if (isInVolume && !geyser._isActive && !hasResetData)
            {
                foreach (var gm in ghostMatter)
                {
                    gm._firstContactDamage = defaultFirstContact;
                    gm._damagePerSecond = defaultDPS;
                }
                hasResetData = true;
            }
        }

        public virtual void OnTriggerEnter(Collider hitCollider)
        {
            // Checks if player collides with the trigger volume
            if (hitCollider.CompareTag("PlayerDetector") && enabled && geyser._isActive)
            {
                isInVolume = true;
                Locator.GetShipLogManager().RevealFact("MUSEUM_HEA_CAVES_E1");
                foreach (var gm in ghostMatter)
                {
                    gm._firstContactDamage = 0;
                    gm._damagePerSecond = 0;
                }
            } 
        }

        public virtual void OnTriggerExit(Collider hitCollider)
        {
            // Checks if player collides with the trigger volume
            if (hitCollider.CompareTag("PlayerDetector") && enabled)
            {
                isInVolume = false;
                hasResetData = false;
                foreach (var gm in ghostMatter)
                {
                    gm._firstContactDamage = defaultFirstContact;
                    gm._damagePerSecond = defaultDPS;
                }
            }
        }
    }
}