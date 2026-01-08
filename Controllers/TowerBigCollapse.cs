using System.Collections;
using UnityEngine;

namespace FifthModJam
{
    /// <summary>
    /// Controls the collapse of the big tower and shuttle launch (when the player is in the exhibits).
    /// </summary>
    public class TowerBigCollapse : MonoBehaviour
    {
        // Audio and animator
        [SerializeField]
        private Animator _towerAnim;
        [SerializeField]
        private OWAudioSource _towerAudio;
        [SerializeField]
        private Animator _shuttleAnim;
        [SerializeField]
        private OWAudioSource _shuttleAudio;

        // The return trigger that allows the player to walk between dioramas on the fallen tower
        [SerializeField]
        private GameObject _towerReturnTrigger;
        [SerializeField]
        private GameObject _achievementVolumeTrigger;

        private void VerifyUnityParameters()
        {
            if (_towerAnim == null)
            {
                FifthModJam.WriteLine("[TowerBigCollapse] tower animator is null", OWML.Common.MessageType.Error);
            }
            if (_towerAudio == null)
            {
                FifthModJam.WriteLine("[TowerBigCollapse] tower audio is null", OWML.Common.MessageType.Error);
            }
            if (_shuttleAnim == null)
            {
                FifthModJam.WriteLine("[TowerBigCollapse] shuttle animator is null", OWML.Common.MessageType.Error);
            }
            if (_shuttleAudio == null)
            {
                FifthModJam.WriteLine("[TowerBigCollapse] shuttle audio is null", OWML.Common.MessageType.Error);
            }
            if (_towerReturnTrigger == null)
            {
                FifthModJam.WriteLine("[TowerBigCollapse] tower returnTrigger is null", OWML.Common.MessageType.Error);
            }
            if (_achievementVolumeTrigger == null)
            {
                FifthModJam.WriteLine("[TowerBigCollapse] achievement volumeTrigger is null", OWML.Common.MessageType.Error);
            }
        }

        private void Awake()
        {
            TowerCollapseManager.Instance.OnTowerCollapse += OnTowerCollapse;
        }
        private void OnDestroy()
        {
            TowerCollapseManager.Instance.OnTowerCollapse -= OnTowerCollapse;
        }

        private void Start()
        {
            VerifyUnityParameters();

            // Since the diorama can be disabled and re-enabled, make sure the animators keep their state
            if(_towerAnim != null)
            {
                _towerAnim.keepAnimatorControllerStateOnDisable = true;
            }
            if (_shuttleAnim != null)
            {
                _shuttleAnim.keepAnimatorControllerStateOnDisable = true;
            }
            _towerReturnTrigger?.SetActive(false);
            _achievementVolumeTrigger?.SetActive(false);
        }

        private void OnTowerCollapse()
        {
            _towerReturnTrigger?.SetActive(true);
            _achievementVolumeTrigger?.SetActive(true);
            StartCoroutine(PlayAnim());
        }
        private IEnumerator PlayAnim()
        {
            // Launch shuttle
            _shuttleAudio?.PlayOneShot(global::AudioType.NomaiVesselPowerUp, 1f);
            _shuttleAnim?.Play("SHUTTLE", 0);
            yield return new WaitForSeconds(2f);
            _shuttleAudio?.PlayOneShot(global::AudioType.ShipImpact_LightDamage, 1f);

            // Tower is hit, creaks and falls
            _towerAnim?.Play("TOWER", 0);
            _towerAudio?.PlayOneShot(global::AudioType.Tower_RW_Fall_1, 1f);
            yield return new WaitForSeconds(0.75f);
            _towerAudio?.PlayOneShot(global::AudioType.Tower_RW_Fall_2, 1f);
            yield return new WaitForSeconds(1.4f);
            _towerAudio?.PlayOneShot(global::AudioType.GeneralDestruction, 1f);
        }
    }
}