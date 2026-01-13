using UnityEngine;

namespace FifthModJam
{
    public class CreditsHandler : MonoBehaviour
    {
        [SerializeField]
        public GameObject credits;

        [SerializeField]
        private GameObject _dialogueParent;
        private InteractReceiver _interactReceiver;

        private bool _hasLaunchedScout;

        private void VerifyUnityParameters()
        {
            if (credits == null)
            {
                FifthModJam.WriteLine("[CreditsHandler] credits is null", OWML.Common.MessageType.Error);
            }
            if (_dialogueParent == null)
            {
                FifthModJam.WriteLine("[CreditsHandler] dialogue parent is null", OWML.Common.MessageType.Error);
            }
            _interactReceiver = _dialogueParent?.GetComponentInChildren<InteractReceiver>();
            if (_interactReceiver == null)
            {
                FifthModJam.WriteLine("[CreditsHandler] interactReceiver not found", OWML.Common.MessageType.Error);
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
                _interactReceiver.SetPromptText(UITextType.TalkToPrompt, FifthModJam.NewHorizonsAPI.GetTranslationForDialogue(Constants.TRANSLATIONKEY_NPCNAME_PHOSPHORUS)); // Update the talkto prompt to show Phosphorus's name
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