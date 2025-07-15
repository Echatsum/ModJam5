using UnityEngine;

namespace FifthModJam
{
    public class TheatreLightsTrigger : MonoBehaviour
    {
        [SerializeField]
        private GameObject[] lights;

        private void VerifyUnityParameters()
        {
            if (lights == null || lights.Length == 0)
            {
                FifthModJam.WriteLine("[TheatreLightsTrigger] light array is null or empty", OWML.Common.MessageType.Error);
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
                foreach (var light in lights)
                {
                    light.SetActive(false);
                }
            } 
        }

        public virtual void OnTriggerExit(Collider hitCollider)
        {
            // Checks if player no longer collides with the trigger volume
            if (hitCollider.CompareTag("PlayerDetector") && enabled)
            {
                foreach (var light in lights)
                {
                    light.SetActive(true);
                }
            }
        }
    }
}