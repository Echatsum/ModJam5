using UnityEngine;

namespace FifthModJam
{
    public class CreditsHandler : MonoBehaviour
    {
        [SerializeField]
        public GameObject credits;

        private bool creditsEnabled;

        private void Start()
        {
            credits.SetActive(false);
            creditsEnabled = false;
        }

        private void Update()
        {
            if (!creditsEnabled && HasTalkedToKarvi())
            {
                credits.SetActive(true);
                creditsEnabled = true;
            }
        }

        private bool HasTalkedToKarvi()
        {
            return DialogueConditionManager.SharedInstance.GetConditionState("KARVI_MET");
        }
    }
}