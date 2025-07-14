namespace FifthModJam
{
    /// <summary>
    /// Manager made to spread the event about languages learnt.
    /// Uses a small cache to avoid spamming the IsFactRevealed.
    /// </summary>
    public class LanguageManager : AbstractManager<LanguageManager>
    {
        // Neat way to share about the language learning.
        public delegate void LearnLangEvent();
        public LearnLangEvent OnLanguagesLearned;

        // We only need to ping the shiplog twice: at the start + when the event is triggered. For other times a cache saves a bit of performance
        private bool _hasLearnedLangCache;
        private bool _hasCached = false;

        // Shiplog methods
        public bool HasLearnedLang()
        {
            if (!_hasCached)
            {
                _hasLearnedLangCache = Locator.GetShipLogManager().IsFactRevealed("SHIP_CRYSTAL_E1"); // Set cache at the start
                _hasCached = true;
            }

            return _hasLearnedLangCache;
        }
        public void RevealFactLanguagesLearned()
        {
            Locator.GetShipLogManager().RevealFact("SHIP_CRYSTAL_E1");
            _hasLearnedLangCache = true; // Update the cache when learning
            _hasCached = true;

            OnLanguagesLearned?.Invoke(); // Event to let every handler update their status
        }
    }
}
