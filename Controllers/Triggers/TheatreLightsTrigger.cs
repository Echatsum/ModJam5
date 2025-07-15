using UnityEngine;

namespace FifthModJam
{
    public class TheatreLightsTrigger : MonoBehaviour
    {
        [SerializeField]
        private GameObject[] lights;

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
            // Checks if player collides with the trigger volume
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