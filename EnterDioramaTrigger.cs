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
        private SpawnPoint _spawnPointTarget;

        // museum and star gameobjects
        private GameObject _museum;
        private GameObject _starLight;

        // TowerCollapse controller
        private TowerCollapse _collapseHandler;

        // constants [Note-TODO: put in another file?]
        public const float blinkTime = 0.5f; // constant for blink time
        public const float animTime = blinkTime / 2f; // constant for blink animation time

        public void Start()
        {
            // Get starlight
            _starLight = GameObject.Find("SilverLining_Body/Sector/Star/StarLight");

            // [Note: museum-dependent objects might be inactive during Start(), so SearchUtilities is used to catch these cases that GameObject cannot]

            _museum = SearchUtilities.Find("ScaledMuseum_Body/Sector"); // get museum
            _collapseHandler = SearchUtilities.Find("ScaledMuseum_Body/Sector/ScaledMuseum").GetComponent<TowerCollapse>(); // get the tower collapse handler

            // Get the SpawnPoint matching the target diorama
            const string pathPrefix = "ScaledMuseum_Body/Sector/ScaledMuseum/Offset/Exhibits/";
            string pathSuffix = GetTargetDioramaPathSuffix();
            _spawnPointTarget = SearchUtilities.Find(pathPrefix + pathSuffix).GetComponent<SpawnPoint>();

            FifthModJam.WriteLine($"[{targetDiorama}] {pathPrefix + pathSuffix}", OWML.Common.MessageType.Success);
        }

        private string GetTargetDioramaPathSuffix()
        {
            return targetDiorama switch
            {
                SpeciesEnum.STRANGER    => "Exhibit_STR/Spawn/SpawnKAV1",
                SpeciesEnum.NOMAI       => "Exhibit_NOM/Spawn/SpawnKAV2",
                SpeciesEnum.HEARTHIAN   => "Exhibit_HEA/Spawn/SpawnKAV3",
                SpeciesEnum.KARVI       => "Exhibit_KAV/Spawn/SpawnKAV4",
                _                       => "Exhibit_STR/Spawn/SpawnKAV1", // Defaults to stranger if issue
            };
        }

        private IEnumerator SetupEntry()
        {
            // close eyes
            _museum.SetActive(true); // enables museum when in trigger
            _starLight.SetActive(false); // disables star when in trigger

            if (_collapseHandler.hasFallen)
            {
                _collapseHandler.ForceTowerFallenState(); // forces the tower to fallen down state if it has before
            }

            var cameraEffectController = FindObjectOfType<PlayerCameraEffectController>(); // gets camera controller
            cameraEffectController.CloseEyes(animTime); // closes eyes
            yield return new WaitForSeconds(animTime);  // waits until animation stops to proceed to next line
            GlobalMessenger.FireEvent("PlayerBlink"); // fires an event for the player blinking
            
            // warp
            yield return new WaitForSeconds(1);
            var spawner = GameObject.FindGameObjectWithTag("Player").GetRequiredComponent<PlayerSpawner>(); // gets player spawner
            spawner.DebugWarp(_spawnPointTarget); // warps you to desired spawn point

            // open eyes
            cameraEffectController.OpenEyes(animTime, false); // open eyes
            yield return new WaitForSeconds(animTime); //  waits until animation stops to proceed to next line
        }

        public virtual void OnTriggerEnter(Collider hitCollider)
        {
            //checks if player collides with the trigger volume
            if (hitCollider.CompareTag("PlayerDetector") && enabled && CheckParameters())
            {
                StartCoroutine(SetupEntry());
            }
        }

        private bool CheckParameters()
        {
            // Check that all game objects are good for use
            if (_museum == null)
            {
                FifthModJam.WriteLine("[EnterDioramaTrigger] _museum object was null", OWML.Common.MessageType.Warning);
                return false;
            }
            if (_starLight == null)
            {
                FifthModJam.WriteLine("[EnterDioramaTrigger] _starLight object was null", OWML.Common.MessageType.Warning);
                return false;
            }
            if (_spawnPointTarget == null)
            {
                FifthModJam.WriteLine("[EnterDioramaTrigger] _spawnPointTarget object was null", OWML.Common.MessageType.Warning);
                return false;
            }
            
            // Check the tower handler is good for use
            if (_collapseHandler == null)
            {
                FifthModJam.WriteLine("[EnterDioramaTrigger] _collapseHandler component was not found", OWML.Common.MessageType.Warning);
                return false;
            }

            // Everything good
            return true;
        }
    }
}