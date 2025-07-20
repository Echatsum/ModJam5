using UnityEngine;

namespace FifthModJam
{
    public class TheaterLightsTrigger : MonoBehaviour
    {
        [SerializeField]
        private TheatreLightsHandler _lightsHandler;
        [SerializeField]
        private bool _isHouseEntry; // Whether this is the trigger placed on the outside or inside of the house

        private void VerifyUnityParameters()
        {
            if (_lightsHandler == null)
            {
                FifthModJam.WriteLine("[TheaterLightsTrigger] lightsHandler is null", OWML.Common.MessageType.Error);
            }
        }
        private void Start()
        {
            VerifyUnityParameters();
        }

        public virtual void OnTriggerEnter(Collider hitCollider)
        {
            // Checks if player collides with the trigger volume
            if (hitCollider.CompareTag("PlayerDetector") && enabled)
            {
                if (_isHouseEntry)
                {
                    _lightsHandler?.OnEnterHouse();
                }
                else
                {
                    _lightsHandler?.OnExitHouse();
                }
            }
        }
    }
}
