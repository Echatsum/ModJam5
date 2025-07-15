using NewHorizons.Utility;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FifthModJam
{
    public class DioramaWarpManager : AbstractManager<DioramaWarpManager>
    {
        // The spawn point list
        private readonly Dictionary<DioramaSpawnPointEnum, SpawnPoint> _spawnPoints = new();

        // museum and star gameobjects
        private GameObject _museum;
        private GameObject _starLight;

        private bool _hasRegisteredObjects = false;

        private void Start()
        {
            // Register game objects
            _hasRegisteredObjects = RegisterGameobjects();

            // Get spawn points
            var flag2 = AddSpawnPoint(DioramaSpawnPointEnum.KARVI);
            flag2 &= AddSpawnPoint(DioramaSpawnPointEnum.STRANGER);
            flag2 &= AddSpawnPoint(DioramaSpawnPointEnum.NOMAI);
            flag2 &= AddSpawnPoint(DioramaSpawnPointEnum.HEARTHIAN);
            flag2 &= AddSpawnPoint(DioramaSpawnPointEnum.RETURN);

            // Everything loaded correctly
            if (_hasRegisteredObjects && flag2)
            {
                FifthModJam.WriteLineReady("DioramaWarpManager");
            }
        }

        private bool RegisterGameobjects()
        {
            bool flag1 = true;

            // Get starlight
            _starLight = SearchUtilities.Find(Constants.UNITYPATH_STARLIGHT);
            if (_starLight == null)
            {
                FifthModJam.WriteLineObjectOrComponentNotFound("DioramaWarpManager", Constants.UNITYPATH_STARLIGHT);
                flag1 = false;
            }

            // Get museum
            _museum = SearchUtilities.Find(Constants.UNITYPATH_MUSEUM);
            if (_museum == null)
            {
                FifthModJam.WriteLineObjectOrComponentNotFound("DioramaWarpManager", Constants.UNITYPATH_MUSEUM);
                flag1 = false;
            }

            return flag1;
        }
        private bool AddSpawnPoint(DioramaSpawnPointEnum spawnPointTarget)
        {
            // Get path
            string spawnPath = null;
            switch (spawnPointTarget)
            {
                case DioramaSpawnPointEnum.RETURN:
                    spawnPath = Constants.UNITYPATH_KARVISHIP_SPAWNRETURN;
                    break;
                case DioramaSpawnPointEnum.KARVI:
                    spawnPath = Constants.UNITYPATH_EXHIBITS_PREFIX + Constants.UNITYPATH_EXHIBITS_SUFFIX_KAR;
                    break;
                case DioramaSpawnPointEnum.STRANGER:
                    spawnPath = Constants.UNITYPATH_EXHIBITS_PREFIX + Constants.UNITYPATH_EXHIBITS_SUFFIX_STR;
                    break;
                case DioramaSpawnPointEnum.NOMAI:
                    spawnPath = Constants.UNITYPATH_EXHIBITS_PREFIX + Constants.UNITYPATH_EXHIBITS_SUFFIX_NOM;
                    break;
                case DioramaSpawnPointEnum.HEARTHIAN:
                    spawnPath = Constants.UNITYPATH_EXHIBITS_PREFIX + Constants.UNITYPATH_EXHIBITS_SUFFIX_HEA;
                    break;

                default:
                    FifthModJam.WriteLine($"[DioramaWarpManager ({spawnPointTarget})] Target spawn not handled", OWML.Common.MessageType.Error);
                    break;
            }

            // Try to find spawnPoint component
            SpawnPoint spawnPoint = spawnPath == null ? null : SearchUtilities.Find(spawnPath)?.GetComponent<SpawnPoint>();
            if (spawnPoint == null)
            {
                FifthModJam.WriteLineObjectOrComponentNotFound($"DioramaWarpManager ({spawnPointTarget})", spawnPath, nameof(SpawnPoint));
            }

            // Add to list
            _spawnPoints[spawnPointTarget] = spawnPoint;

            return spawnPoint != null;
        }

        public void WarpTo(DioramaSpawnPointEnum spawnPointTarget, bool isEnteringDiorama)
        {
            if (!_hasRegisteredObjects) return; // Do not try to warp if invalid gameobjects

            // Warp
            var spawnPoint = _spawnPoints.ContainsKey(spawnPointTarget) ? _spawnPoints[spawnPointTarget] : null;
            if (spawnPoint == null) return; // Do not try to warp if invalid spawnPoint

            if (isEnteringDiorama)
            {
                StartCoroutine(EnterDioramaCoroutine(spawnPoint));
            }
            else
            {
                StartCoroutine(ExitDioramaCoroutine(spawnPoint));
            }
        }

        // COROUTINES
        private IEnumerator EnterDioramaCoroutine(SpawnPoint spawnPointTarget)
        {
            // Close eyes
            yield return StartCoroutine(FifthModJam.Instance.CloseEyesCoroutine());
            yield return new WaitForSeconds(Constants.BLINK_STAY_CLOSED_TIME);

            // Update objects
            _museum.SetActive(true);
            _starLight.SetActive(false);

            // Warp
            var spawner = GameObject.FindGameObjectWithTag("Player").GetRequiredComponent<PlayerSpawner>();
            spawner.DebugWarp(spawnPointTarget);

            // Open eyes
            yield return StartCoroutine(FifthModJam.Instance.OpenEyesCoroutine());
        }
        private IEnumerator ExitDioramaCoroutine(SpawnPoint spawnPointTarget)
        {
            // Close eyes
            yield return StartCoroutine(FifthModJam.Instance.CloseEyesCoroutine());

            // Warp
            var spawner = GameObject.FindGameObjectWithTag("Player").GetRequiredComponent<PlayerSpawner>();
            spawner.DebugWarp(spawnPointTarget);

            // Update objects
            _museum.SetActive(false);
            _starLight.SetActive(true);

            // Open eyes
            yield return new WaitForSeconds(Constants.BLINK_STAY_CLOSED_TIME);
            yield return StartCoroutine(FifthModJam.Instance.OpenEyesCoroutine());
        }
    }
}
