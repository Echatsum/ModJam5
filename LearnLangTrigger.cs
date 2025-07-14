using UnityEngine;

// [TODO: Move to Triggers/ folder once safe for push/pull]

namespace FifthModJam
{
    /// <summary>
    /// Trigger placed on the knowledge crystal object.
    /// Reveals a shiplog fact so that the player can read other languages.
    /// </summary>
    public class LearnLangTrigger : MonoBehaviour
    {
        // Animator for the bubble
        [SerializeField]
        private Animator _crystalFX;

        // Triggers only once
        private bool _hasAlreadyEnteredOnce = false;

        protected void VerifyUnityParameters()
        {
            if (_crystalFX == null)
            {
                FifthModJam.WriteLine("[LearnLangTrigger] crystalFX is null", OWML.Common.MessageType.Error);
            }
        }

        private void Start()
        {
            VerifyUnityParameters();
        }

        public virtual void OnTriggerEnter(Collider hitCollider)
        {
            if (_hasAlreadyEnteredOnce) return;

            // Checks if player collides with the trigger volume
            if (hitCollider.CompareTag("PlayerDetector") && enabled)
            {
                _crystalFX?.Play("CrystalFade", 0);
                
                if (!LanguageManager.Instance.HasLearnedLang()) // Check for fact (the trigger resets between loops, but we only need to reveal fact once)
                {
                    LanguageManager.Instance.RevealFactLanguagesLearned();
                    LanguageManager.Instance.OnLanguagesLearned?.Invoke(); // Event to let every handler update their status
                }

                _hasAlreadyEnteredOnce = true;
            } 
        }
    }
}