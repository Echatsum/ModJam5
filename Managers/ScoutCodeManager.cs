namespace FifthModJam
{
    /// <summary>
    /// Manager because nh does not allow a signal reveal to reveal more than one shiplog
    /// </summary>
    public class ScoutCodeManager : AbstractManager<ScoutCodeManager>
    {
        private bool _hasFoundSource = false;
        private bool _hasfoundOneEcho = false;

        private bool _hasWaitedAFrame = false;

        private void Start()
        {
            if (!FifthModJam.Instance.IsInJamFiveSystem()) return;

            GlobalMessenger.AddListener("IdentifySignal", OnSignalIdentified);

            _hasFoundSource = Locator.GetShipLogManager().IsFactRevealed("COSMICCURATORS_BROKEN_SCOUTS_CODE_R");
            _hasfoundOneEcho = Locator.GetShipLogManager().IsFactRevealed("COSMICCURATORS_CODED_DOORS_SCOUT_R");

            this.enabled = false;
            FifthModJam.WriteLineReady("ScoutCodeManager");
        }
        private void OnDestroy()
        {
            GlobalMessenger.RemoveListener("IdentifySignal", OnSignalIdentified);
        }

        private void OnSignalIdentified()
        {
            _hasWaitedAFrame = false;
            this.enabled = true; // We can't check the shiplog on the exact same frame, we need to wait for the next one
        }
        private void Update()
        {
            if (_hasWaitedAFrame)
            {
                CheckAndUpdate();
                this.enabled = false;
            }
            else
            {
                _hasWaitedAFrame = true;
            }
        }

        private void CheckAndUpdate()
        {
            if (_hasFoundSource && _hasfoundOneEcho) return; // Ignore when all facts revealed

            if (HasIdentifiedSourceCode())
            {
                Locator.GetShipLogManager().RevealFact("COSMICCURATORS_BROKEN_SCOUTS_CODE_R");
                _hasFoundSource = true;
            }
            if (HasIdentifiedEchoCode())
            {
                Locator.GetShipLogManager().RevealFact("COSMICCURATORS_CODED_DOORS_SCOUT_R");
                _hasfoundOneEcho = true;
            }
        }
        private bool HasIdentifiedSourceCode()
        {
            return Locator.GetShipLogManager().IsFactRevealed("COSMICCURATORS_SCOUT_SIGNAL_E");
        }
        private bool HasIdentifiedEchoCode()
        {
            // Too lazy to write something cleaner, this does the job

            if (Locator.GetShipLogManager().IsFactRevealed("COSMICCURATORS_BROKEN_SCOUTS_CODE1")) return true;
            if (Locator.GetShipLogManager().IsFactRevealed("COSMICCURATORS_BROKEN_SCOUTS_CODE2")) return true;
            if (Locator.GetShipLogManager().IsFactRevealed("COSMICCURATORS_BROKEN_SCOUTS_CODE3")) return true;
            if (Locator.GetShipLogManager().IsFactRevealed("COSMICCURATORS_BROKEN_SCOUTS_CODE4")) return true;
            if (Locator.GetShipLogManager().IsFactRevealed("COSMICCURATORS_BROKEN_SCOUTS_CODE5")) return true;

            return false;
        }
    }
}
