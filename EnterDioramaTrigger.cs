using NewHorizons.Utility;
using System.Collections;
using UnityEngine;

namespace FifthModJam
{
    public class EnterDioramaTrigger : MonoBehaviour
    {
        [SerializeField]
        private int spawnIndex;
        private GameObject museum;
        private GameObject starLight;
        private SpawnPoint[] spawnPoints = new SpawnPoint[4];

        protected PlayerSpawner _spawner; // for spawning the player
        public const float blinkTime = 0.5f; // constant for blink time
        public const float animTime = blinkTime / 2f; // constant for blink animation time

        public void Start()
        {
            OnCompleteSceneLoad(OWScene.TitleScreen, OWScene.TitleScreen);
            LoadManager.OnCompleteSceneLoad += OnCompleteSceneLoad;
        }

        public void OnCompleteSceneLoad(OWScene previousScene, OWScene newScene)
        {
            if (FifthModJam.Instance.IsInJamFiveSystem())
            {
                museum = SearchUtilities.Find("ScaledMuseum_Body/Sector"); // get museum
                starLight = SearchUtilities.Find("SilverLining_Body/Sector/Star/StarLight"); // get starlight
                spawnPoints[0] = SearchUtilities.Find("ScaledMuseum_Body/Sector/ScaledMuseum/Offset/Exhibits/Exhibit_STR/Spawn/SpawnKAV1").GetComponent<SpawnPoint>();
                spawnPoints[1] = SearchUtilities.Find("ScaledMuseum_Body/Sector/ScaledMuseum/Offset/Exhibits/Exhibit_NOM/Spawn/SpawnKAV2").GetComponent<SpawnPoint>();
                spawnPoints[2] = SearchUtilities.Find("ScaledMuseum_Body/Sector/ScaledMuseum/Offset/Exhibits/Exhibit_HEA/Spawn/SpawnKAV3").GetComponent<SpawnPoint>();
                spawnPoints[3] = SearchUtilities.Find("ScaledMuseum_Body/Sector/ScaledMuseum/Offset/Exhibits/Exhibit_KAV/Spawn/SpawnKAV4").GetComponent<SpawnPoint>();
            }
        }

        private IEnumerator SetupEntry()
        {
            // close eyes
            museum.SetActive(true); // disables museum when in trigger
            starLight.SetActive(false); // disables museum when in trigger
            var cameraEffectController = FindObjectOfType<PlayerCameraEffectController>(); // gets camera controller
            cameraEffectController.CloseEyes(animTime); // closes eyes
            yield return new WaitForSeconds(animTime);  // waits until animation stops to proceed to next line
            GlobalMessenger.FireEvent("PlayerBlink"); // fires an event for the player blinking

            // warp
            yield return new WaitForSeconds(1);
            _spawner = GameObject.FindGameObjectWithTag("Player").GetRequiredComponent<PlayerSpawner>(); // gets player spawner
            _spawner.DebugWarp(spawnPoints[spawnIndex]); // warps you to desired spawn point

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