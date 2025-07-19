using UnityEngine;

namespace FifthModJam
{
    public class ScoutRecallTrigger : MonoBehaviour
    {
        public virtual void OnTriggerEnter(Collider hitCollider)
        {
            // Checks if scout collides with the trigger volume
            if (hitCollider.CompareTag("ProbeDetector") && enabled)
            {
                Locator.GetProbe().Retrieve(0.5f); // recall scout
            }
        }
    }
}
