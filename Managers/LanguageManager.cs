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

        public void Start()
        {
            // Set cache at the start
            _hasLearnedLangCache = Locator.GetShipLogManager().IsFactRevealed("COSMICCURATORS_CRYSTAL_PROX");

            FifthModJam.WriteLineReady("LanguageManager");
        }

        // Shiplog methods
        public bool HasLearnedLang()
        {
            return _hasLearnedLangCache;
        }
        public void RevealFactLanguagesLearned()
        {
            Locator.GetShipLogManager().RevealFact("COSMICCURATORS_CRYSTAL_PROX");
            _hasLearnedLangCache = true; // Update the cache when learning

            OnLanguagesLearned?.Invoke(); // Event to let every handler update their status
        }
    }
}
