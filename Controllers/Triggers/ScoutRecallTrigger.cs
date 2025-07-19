using UnityEngine;

namespace FifthModJam.Controllers.Triggers
{
    public class ScoutRecallTrigger : MonoBehaviour
    {
        public virtual void OnTriggerEnter(Collider hitCollider)
        {
            // Checks if player collides with the trigger volume
            if (hitCollider.CompareTag("PlayerDetector") && enabled)
            {
                Locator.GetProbe().Retrieve(0.5f); // recall scout
            }
        }
    }
}
