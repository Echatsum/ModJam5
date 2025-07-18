using UnityEngine;

namespace FifthModJam
{
    public class GhostMatterTrigger : MonoBehaviour
    {
        [SerializeField]
        private GeyserController geyser; // The geyser that needs to be active for the ghost matter to be damageless to the player
        [SerializeField]
        private DarkMatterVolume[] ghostMatter; // The patches of ghost matter

        private const float defaultFirstContact = 70;
        private const float defaultDPS = 80;

        // Nullified damage. Not 0, so that it's pretty much nothing but player still can't stand indefinitely inside
        private const float nullifiedFirstContact = 1; 
        private const float nullifiedDPS = 1;

        private void VerifyUnityParameters()
        {
            if (geyser == null)
            {
                FifthModJam.WriteLine("[GhostMatterTrigger] geyser is null", OWML.Common.MessageType.Error);
            }
            if (ghostMatter == null || ghostMatter.Length == 0)
            {
                FifthModJam.WriteLine("[GhostMatterTrigger] ghostMatter array is null or empty", OWML.Common.MessageType.Error);
            }
        }

        public void Start()
        {
            VerifyUnityParameters();
        }

        private void ToggleGhostMatterDamage(bool nullifyDamage)
        {
            var firstContactDamage = nullifyDamage ? nullifiedFirstContact : defaultFirstContact;
            var damagePerSecond = nullifyDamage ? nullifiedDPS : defaultDPS;

            foreach (var gm in ghostMatter)
            {
                gm._firstContactDamage = firstContactDamage;
                gm._damagePerSecond = damagePerSecond;
            }
        }

        public virtual void OnTriggerEnter(Collider hitCollider)
        {
            if (!geyser._isActive) return; // Only make damageless when we enter while the geyser is actively spewing water (not just when the geyser rock exists)

            // Checks if player collides with the trigger volume
            if (hitCollider.CompareTag("PlayerDetector") && enabled)
            {
                Locator.GetShipLogManager().RevealFact("COSMICCURATORS_GHOST_MATTER_CAVE_ENTER1");
                Locator.GetShipLogManager().RevealFact("COSMICCURATORS_GHOST_MATTER_CAVE_ENTER2");
                Locator.GetShipLogManager().RevealFact("COSMICCURATORS_FELDSPAR_MYTH_R");
                ToggleGhostMatterDamage(nullifyDamage: true);
            } 
        }

        public virtual void OnTriggerExit(Collider hitCollider)
        {
            // Checks if player no longer collides with the trigger volume
            if (hitCollider.CompareTag("PlayerDetector") && enabled)
            {
                ToggleGhostMatterDamage(nullifyDamage: false);
            }
        }
    }
}