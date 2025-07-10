using NewHorizons.Utility;
using System.Collections;
using UnityEngine;

namespace FifthModJam
{
    public class ExitDioramaTrigger : MonoBehaviour
    {
        private GameObject museum;
        private GameObject starLight;
        private SpawnPoint returnPoint;

        protected PlayerSpawner _spawner; // for spawning the player
        public const float blinkTime = 0.5f; // constant for blink time
        public const float animTime = blinkTime / 2f; // constant for blink animation time

        public void Start()
        {
            if (FifthModJam.Instance.IsInJamFiveSystem())
            {
                museum = GameObject.Find("ScaledMuseum_Body/Sector"); // get museum
                starLight = GameObject.Find("SilverLining_Body/Sector/Star/StarLight"); // get starlight
                returnPoint = GameObject.Find("OminousOrbiter_Body/Sector/KarviShip_Interior/Interactibles/SpawnReturn/SpawnKAV5").GetComponent<SpawnPoint>();
            }
        }
        

     

        private IEnumerator SetupExit()
        {
            // close eyes
            var cameraEffectController = FindObjectOfType<PlayerCameraEffectController>(); // gets camera controller
            cameraEffectController.CloseEyes(animTime); // closes eyes
            yield return new WaitForSeconds(animTime);  // waits until animation stops to proceed to next line
            GlobalMessenger.FireEvent("PlayerBlink"); // fires an event for the player blinking

            // warp
            _spawner = GameObject.FindGameObjectWithTag("Player").GetRequiredComponent<PlayerSpawner>(); // gets player spawner
            _spawner.DebugWarp(returnPoint); // warps you to desired spawn point

            // open eyes
            yield return new WaitForSeconds(1);
            cameraEffectController.OpenEyes(animTime, false); // open eyes
            yield return new WaitForSeconds(animTime); //  waits until animation stops to proceed to next line
            museum.SetActive(false); // disables museum when in trigger
            starLight.SetActive(true); // disables museum when in trigger
        }

        public virtual void OnTriggerExit(Collider hitCollider)
        {
            //checks if player collides with the trigger volume
            if (hitCollider.CompareTag("PlayerDetector") && enabled && museum != null)
            {
                StartCoroutine(SetupExit());
            }
        }
    }
}