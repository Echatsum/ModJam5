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
        [SerializeField]
        private bool _isSoftTrigger; // For EXIT types: "hard" is when warping happens all the time, "soft" is when warping only happens when out of all overlapping triggers

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
            // Checks if player collides with the trigger volume
            if (hitCollider.CompareTag("PlayerDetector") && enabled)
            {
                if (!_isExhibitEntry)
                {
                    if (_isSoftTrigger)
                    {
                        DioramaWarpManager.Instance.OnEnteringExitSoftTrigger(); // Notify manager about entering a 'soft exit' type
                    }
                    return; // Continue only if this trigger is an ENTRY
                }

                Locator.GetShipLogManager().RevealFact("COSMICCURATORS_DIORAMA_ROOM_MINIATURE");
                DioramaWarpManager.Instance.WarpTo(_warpTargetDiorama, isEnteringDiorama: true);
            }
        }

        public virtual void OnTriggerExit(Collider hitCollider)
        {
            if (_isExhibitEntry) return; // Continue only if this trigger is an EXIT

            if (hitCollider.CompareTag("PlayerDetector") && enabled)
            {
                var shouldWarp = true;
                if (_isSoftTrigger)
                {
                    shouldWarp = DioramaWarpManager.Instance.OnExitingExitSoftTrigger(); // Notify manager about exiting a 'soft exit' type. If not last overlap, then do not warp
                }

                if (shouldWarp)
                {
                    DioramaWarpManager.Instance.WarpTo(_warpTargetDiorama, isEnteringDiorama: false);
                }
            }
        }

    }
}
