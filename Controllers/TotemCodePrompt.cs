using System.Collections.Generic;
using UnityEngine;

namespace FifthModJam
{
    [RequireComponent(typeof(EclipseCodeController4))]
    public class TotemCodePrompt : MonoBehaviour
    {
        // The totem interact, 
        [SerializeField]
        private TotemCodePromptEnum _totemLocation;
        [SerializeField]
        private InteractReceiver _interactReceiver;

        private IList<int> _totemCode;
        private ScreenPrompt _prompt;
        private bool _cachedRequirementCheck = false;

        private void VerifyUnityParameters()
        {
            if (_totemLocation == TotemCodePromptEnum.INVALID)
            {
                FifthModJam.WriteLine("[TotemCodePrompt] totemCode is invalid", OWML.Common.MessageType.Error);
            }
            if (_interactReceiver == null)
            {
                FifthModJam.WriteLine("[TotemCodePrompt] interact receiver is null", OWML.Common.MessageType.Error);
            }
        }

        void Start()
        {
            VerifyUnityParameters();

            // Initialize player detector trigger
            if(_interactReceiver != null)
            {
                _interactReceiver.OnGainFocus += OnGainFocus;
                _interactReceiver.OnLoseFocus += OnLoseFocus;
            }
        }
        private void OnDestroy()
        {
            if (_interactReceiver != null)
            {
                _interactReceiver.OnGainFocus -= OnGainFocus;
                _interactReceiver.OnLoseFocus -= OnLoseFocus;
            }
        }

        private void InitializePrompt()
        {
            _totemCode = this.GetComponent<EclipseCodeController4>()._code;

            var promptText = TotemCodePromptManager.Instance.GetPromptText(_totemLocation);
            var sprite = TotemCodePromptManager.Instance.GetSprite(_totemCode);
            _prompt = new ScreenPrompt(promptText, sprite, 0);

            Locator.GetPromptManager().AddScreenPrompt(_prompt);
        }
        private void OnGainFocus()
        {
            // Init prompt if not already done
            if (_prompt == null)
            {
                InitializePrompt();
            }

            // Update cache if needed (helps 
            if (!_cachedRequirementCheck)
            {
                _cachedRequirementCheck = TotemCodePromptManager.Instance.DoesMeetPromptRequirements(_totemLocation);
            }

            // show prompt if requirements are good
            if (_cachedRequirementCheck)
            {
                _prompt.SetVisibility(true);
            }
        }
        private void OnLoseFocus()
        {
            _prompt.SetVisibility(false);
        }
    }
}
