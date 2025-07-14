using NewHorizons.Utility;
using UnityEngine;

namespace FifthModJam
{
    public class EnterDioramaTrigger : MonoBehaviour
    {
        [SerializeField]
        private SpeciesEnum targetDiorama; // Which target this trigger should warp the player to
        private SpawnPoint _spawnPointTarget;

        protected void VerifyUnityParameters()
        {
            if (targetDiorama == SpeciesEnum.INVALID)
            {
                FifthModJam.WriteLine("[EnterDioramaTrigger] targetDiorama left on invalid", OWML.Common.MessageType.Error);
            }
        }

        public void Start()
        {
            VerifyUnityParameters();

            // Get the SpawnPoint matching the target diorama
            const string pathPrefix = Constants.UNITYPATH_EXHIBITS_PREFIX;
            string spawnPath = pathPrefix + GetTargetDioramaPathSuffix();
            _spawnPointTarget = SearchUtilities.Find(spawnPath)?.GetComponent<SpawnPoint>();

            if (_spawnPointTarget == null)
            {
                FifthModJam.WriteLineObjectOrComponentNotFound($"EnterDioramaTrigger ({targetDiorama})", spawnPath, nameof(SpawnPoint));
            }
            else
            {
                FifthModJam.WriteLineReady($"EnterDioramaTrigger ({targetDiorama})");
            }
        }

        private string GetTargetDioramaPathSuffix()
        {
            return targetDiorama switch
            {
                SpeciesEnum.STRANGER    => Constants.UNITYPATH_EXHIBITS_SUFFIX_STR,
                SpeciesEnum.NOMAI       => Constants.UNITYPATH_EXHIBITS_SUFFIX_NOM,
                SpeciesEnum.HEARTHIAN   => Constants.UNITYPATH_EXHIBITS_SUFFIX_HEA,
                SpeciesEnum.KARVI       => Constants.UNITYPATH_EXHIBITS_SUFFIX_KAR,
                _                       => Constants.UNITYPATH_EXHIBITS_SUFFIX_STR, // Defaults to stranger if issue
            };
        }

        public virtual void OnTriggerEnter(Collider hitCollider)
        {
            // Checks if player collides with the trigger volume
            if (hitCollider.CompareTag("PlayerDetector") && enabled && _spawnPointTarget != null)
            {
                FifthModJam.Instance.EnterDiorama(_spawnPointTarget);
            }
        }
    }
}