using UnityEngine;

namespace FifthModJam
{
    public class CreditsHandler : MonoBehaviour
    {
        [SerializeField]
        public GameObject credits;

        private bool _hasLaunchedScout;

        private void VerifyUnityParameters()
        {
            if (credits == null)
            {
                FifthModJam.WriteLine("[CreditsHandler] credits is null", OWML.Common.MessageType.Error);
            }
        }

        private void Start()
        {
            VerifyUnityParameters();

            credits?.SetActive(false);
            _hasLaunchedScout = false;

            GlobalMessenger<string, bool>.AddListener("DialogueConditionChanged", OnDialogueConditionChanged);
            GlobalMessenger<SurveyorProbe>.AddListener("LaunchProbe", OnLaunchProbe);
        }

        private void OnDestroy()
        {
            GlobalMessenger<string, bool>.RemoveListener("DialogueConditionChanged", OnDialogueConditionChanged);
            GlobalMessenger<SurveyorProbe>.RemoveListener("LaunchProbe", OnLaunchProbe);
        }

        private void OnDialogueConditionChanged(string conditionName, bool conditionState)
        {
            if (credits.activeSelf) return; // No need to check again once credits are active

            if (conditionName.Equals("KARVI_MET") && conditionState) // Has talked to Karvi
            {
                credits.SetActive(true);
                FifthModJam.AchievementsAPI.EarnAchievement(Constants.ACHIEVEMENT_THE_COSMIC_CURATORS);

                if (!_hasLaunchedScout)
                {
                    FifthModJam.AchievementsAPI.EarnAchievement(Constants.ACHIEVEMENT_SCOUTLESS);
                }
            }
        }
        private void OnLaunchProbe(SurveyorProbe probe)
        {
            _hasLaunchedScout = true;
        }
    }
}