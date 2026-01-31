using NewHorizons.External;

namespace FifthModJam
{
    /// <summary>
    /// Manager made to maintain compatibility between versions of the mod:
    /// The Karvi frequency used to be named "Karvi Wavelength" but got changed to "COSMICCURATORS_SIGNAL_KARVIFREQUENCY" (translated in the translation files)
    /// However because of how NH works (separates signals and frequencies), any profile that knows the signals will not get any way to learn the new name.
    /// Therefore this manager makes sure that if the profile knows the old name, it immediately learns the new name.
    /// </summary>
    public class FrequencyUpgradeManager : AbstractManager<FrequencyUpgradeManager>
    {
        public void Start()
        {
            if (!FifthModJam.Instance.IsInJamFiveSystem()) return; // [Note: this sends an error anyway because NewHorizonsAPI might not be ready]

            var knowsOldName = NewHorizonsData.KnowsFrequency(Constants.FREQUENCY_NAME_OLD);
            if (knowsOldName)
            {
                NewHorizonsData.ForgetFrequency(Constants.FREQUENCY_NAME_OLD);
                NewHorizonsData.LearnFrequency(Constants.FREQUENCY_NAME_NEW);
            }

            this.enabled = false;
            FifthModJam.WriteLineReady("FrequencyUpgradeManager");
        }
    }
}
