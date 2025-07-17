using UnityEngine;

namespace FifthModJam.Controllers.Triggers
{
    public class DioramaWarpTrigger : MonoBehaviour
    {
        // Which target this trigger should warp the player to, and whether it is an entry or exit
        [SerializeField]
        private DioramaSpawnPointEnum _warpTargetDiorama;
        [SerializeField]
        private bool _isExhibitEntry;

        private void VerifyUnityParameters()
        {
            if (_warpTargetDiorama == DioramaSpawnPointEnum.INVALID)
            {
                FifthModJam.WriteLine("[DioramaWarpTrigger] targetDiorama left on invalid", OWML.Common.MessageType.Error);
            }
        }
        private void Start()
        {
            VerifyUnityParameters();
        }

        public virtual void OnTriggerEnter(Collider hitCollider)
        {
            if (!_isExhibitEntry) return; // Continue only if this trigger is an ENTRY

            // Checks if player collides with the trigger volume
            if (hitCollider.CompareTag("PlayerDetector") && enabled)
            {
                DioramaWarpManager.Instance.WarpTo(_warpTargetDiorama, isEnteringDiorama: true);
            }
        }

        public virtual void OnTriggerExit(Collider hitCollider)
        {
            if (_isExhibitEntry) return; // Continue only if this trigger is an EXIT

            if (hitCollider.CompareTag("PlayerDetector") && enabled)
            {
                DioramaWarpManager.Instance.WarpTo(_warpTargetDiorama, isEnteringDiorama: false);
            }
        }

    }
}
