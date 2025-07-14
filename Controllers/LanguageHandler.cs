using System;
using UnityEngine;

namespace FifthModJam
{
    /// <summary>
    /// Controls the activation of the dialogue gameObject based on whether learned languages.
    /// </summary>
    public class LanguageHandler : MonoBehaviour
    {
        // The empty gameobject that NH will use to spawn dialogue element
        [SerializeField]
        private GameObject _dialogue;
        public bool IsDialogueEnabled => _dialogue?.activeSelf ?? false; // based on the activation of the dialogue. If gameObject is null, defaults to false

        protected void VerifyUnityParameters()
        {
            if (_dialogue == null)
            {
                FifthModJam.WriteLine("[LanguageHandler] dialogue object is null", OWML.Common.MessageType.Error);
            }
        }

        private void Awake()
        {
            LanguageManager.Instance.OnLanguagesLearned = (LanguageManager.LearnLangEvent)Delegate.Combine(LanguageManager.Instance.OnLanguagesLearned, new LanguageManager.LearnLangEvent(OnLanguagesLearned));
        }
        private void OnDestroy()
        {
            LanguageManager.Instance.OnLanguagesLearned = (LanguageManager.LearnLangEvent)Delegate.Remove(LanguageManager.Instance.OnLanguagesLearned, new LanguageManager.LearnLangEvent(OnLanguagesLearned));
        }
        private void OnLanguagesLearned()
        {
            _dialogue.SetActive(true);
        }

        private void Start()
        {
            VerifyUnityParameters();

            _dialogue.SetActive(LanguageManager.Instance.HasLearnedLang());
        }

    }
}