using UnityEngine;

namespace FifthModJam
{
    public class ExitDioramaTrigger : MonoBehaviour
    {
        // Where to warp the player to
        private SpawnPoint _returnPoint;

        public void Start()
        {
            const string spawnPath = Constants.UNITYPATH_KARVISHIP_SPAWNRETURN;
            _returnPoint = GameObject.Find(spawnPath)?.GetComponent<SpawnPoint>();

            if (_returnPoint == null)
            {
                FifthModJam.WriteLineObjectOrComponentNotFound("ExitDioramaTrigger", spawnPath, nameof(SpawnPoint));
            }
            else
            {
                FifthModJam.WriteLineReady("ExitDioramaTrigger");
            }
        }   

        public virtual void OnTriggerExit(Collider hitCollider)
        {
            // Checks if player no longer collides with the trigger volume
            if (hitCollider.CompareTag("PlayerDetector") && enabled && _returnPoint != null)
            {
                FifthModJam.Instance.ExitDiorama(_returnPoint);
            }
        }
    }
}