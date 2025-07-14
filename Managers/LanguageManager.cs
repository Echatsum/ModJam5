using UnityEngine;

namespace FifthModJam
{
    /// <summary>
    /// Manager made to spread the event about languages learnt.
    /// </summary>
    public class LanguageManager : AbstractManager<LanguageManager>
    {
        // Neat way to share about the language learning.
        public delegate void LearnLangEvent();
        public LearnLangEvent OnLanguagesLearned;

        // Shiplog methods
        public bool HasLearnedLang()
        {
            return Locator.GetShipLogManager().IsFactRevealed("SHIP_CRYSTAL_E1");
        }
        public void RevealFactLanguagesLearned()
        {
            Locator.GetShipLogManager().RevealFact("SHIP_CRYSTAL_E1");
        }
    }
}
