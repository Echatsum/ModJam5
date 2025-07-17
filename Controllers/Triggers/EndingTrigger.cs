using UnityEngine;

namespace FifthModJam
{
    public class EndingTrigger : MonoBehaviour
    {
        [SerializeField]
        private GameObject _music;
        [SerializeField]
        private TorchPuzzle _torchPuzzle;

        private GameObject _nhPlanet;
        private Sector _desiredSector;

        private bool _hasAlreadyTriggeredEnding = false;

        protected void VerifyUnityParameters()
        {
            if (_music == null)
            {
                FifthModJam.WriteLine("[EndingTrigger] music null", OWML.Common.MessageType.Error);
            }
            if (_torchPuzzle == null)
            {
                FifthModJam.WriteLine("[EndingTrigger] torchPuzzle null", OWML.Common.MessageType.Error);
            }
        }

        private void Start()
        {
            _music?.SetActive(false);
            _nhPlanet = FifthModJam.NewHorizonsAPI.GetPlanet("Scaled Museum");
            _desiredSector = _nhPlanet?.transform?.Find("Sector")?.gameObject?.GetComponent<Sector>();

            if (_nhPlanet == null)
            {
                FifthModJam.WriteLine("[EndingTrigger] nhplanet not found", OWML.Common.MessageType.Error);
            }
            if (_desiredSector == null)
            {
                FifthModJam.WriteLine("[EndingTrigger] desiredSector not found", OWML.Common.MessageType.Error);
            }
        }

        public virtual void OnTriggerEnter(Collider hitCollider)
        {
            // If the ending has already been triggered, we don't need to do anything
            if (_hasAlreadyTriggeredEnding) return;

            // The ending only triggers after the torch puzzle is completed
            if (!_torchPuzzle.IsPuzzleCompleted) return;

            // Checks if player collides with the trigger volume
            if (hitCollider.CompareTag("PlayerDetector") && enabled)
            {
                _music.SetActive(true);
                Locator.GetToolModeSwapper().GetItemCarryTool()?.DropItemInstantly(_desiredSector, _nhPlanet.transform);
                Locator.GetPlayerSuit().RemoveSuit();

                _hasAlreadyTriggeredEnding = true;
            } 
        }
    }
}