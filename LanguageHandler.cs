using UnityEngine;

namespace FifthModJam
{
    public class LanguageHandler : MonoBehaviour
    {
        [SerializeField]
        public GameObject dialogue;

        private bool dialogueEnabled;

        private void Start()
        {
            if (HasLearnedLang())
            {
                dialogue.SetActive(true);
                dialogueEnabled = true;
            }
            else
            {
                dialogue.SetActive(false);
                dialogueEnabled = false;
            }
        }

        private void Update()
        {
            if (!dialogueEnabled && HasLearnedLang())
            {
                dialogue.SetActive(true);
            }

            if (!HasLearnedLang())
            {
                dialogue.SetActive(false);
            }
        }

        private bool HasLearnedLang()
        {
            return Locator.GetShipLogManager().IsFactRevealed("SHIP_CRYSTAL_E1");
        }
    }
}