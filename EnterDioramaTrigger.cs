using NewHorizons.Utility;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FifthModJam
{
    public class EnterDioramaTrigger : MonoBehaviour
    {
        [SerializeField]
        private SpeciesEnum targetDiorama; // Which target this triggers should warp the player to
        private SpawnPoint spawnPointTarget;

        private GameObject museum;
        private GameObject starLight;
        private TowerCollapse collapseHandler;

        protected PlayerSpawner _spawner; // for spawning the player
        public const float blinkTime = 0.5f; // constant for blink time
        public const float animTime = blinkTime / 2f; // constant for blink animation time

        public void Start()
        {
            museum = GameObject.Find("ScaledMuseum_Body/Sector"); // get museum
            starLight = GameObject.Find("SilverLining_Body/Sector/Star/StarLight"); // get starlight

            // Get the SpawnPoint matching the target diorama
            const string pathPrefix = "ScaledMuseum_Body/Sector/ScaledMuseum/Offset/Exhibits/";
            string pathSuffix = GetTargetDioramaPathSuffix();
            spawnPointTarget = SearchUtilities.Find(pathPrefix + pathSuffix).GetComponent<SpawnPoint>();
            FifthModJam.WriteLine(pathPrefix + pathSuffix, OWML.Common.MessageType.Success);

            collapseHandler = SearchUtilities.Find("ScaledMuseum_Body/Sector/ScaledMuseum").GetComponent<TowerCollapse>();
        }

        private string GetTargetDioramaPathSuffix()
        {
            return targetDiorama switch
            {
                SpeciesEnum.STRANGER   => "Exhibit_STR/Spawn/SpawnKAV1",
                SpeciesEnum.NOMAI      => "Exhibit_NOM/Spawn/SpawnKAV2",
                SpeciesEnum.HEARTHIAN  => "Exhibit_HEA/Spawn/SpawnKAV3",
                SpeciesEnum.KARVI      => "Exhibit_KAV/Spawn/SpawnKAV4",
                _                      => "Exhibit_STR/Spawn/SpawnKAV1", // Defaults to stranger if issue
            };
        }

        private IEnumerator SetupEntry()
        {
            // close eyes
            museum.SetActive(true); // enables museum when in trigger
            starLight.SetActive(false); // disables star when in trigger
            if (collapseHandler.hasFallen)
            {
                collapseHandler.ForceTowerFall(); // forces the tower to fall down if it has before
            }
            var cameraEffectController = FindObjectOfType<PlayerCameraEffectController>(); // gets camera controller
            cameraEffectController.CloseEyes(animTime); // closes eyes
            yield return new WaitForSeconds(animTime);  // waits until animation stops to proceed to next line
            GlobalMessenger.FireEvent("PlayerBlink"); // fires an event for the player blinking
            
            // warp
            yield return new WaitForSeconds(1);
            _spawner = GameObject.FindGameObjectWithTag("Player").GetRequiredComponent<PlayerSpawner>(); // gets player spawner
            _spawner.DebugWarp(spawnPointTarget); // warps you to desired spawn point

            // open eyes
            cameraEffectController.OpenEyes(animTime, false); // open eyes
            yield return new WaitForSeconds(animTime); //  waits until animation stops to proceed to next line
        }

        public virtual void OnTriggerEnter(Collider hitCollider)
        {
            //checks if player collides with the trigger volume
            if (hitCollider.CompareTag("PlayerDetector") && enabled && museum != null)
            {
                StartCoroutine(SetupEntry());
            }
        }
    }
}