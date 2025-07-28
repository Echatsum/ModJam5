using UnityEngine;

namespace FifthModJam.Controllers
{
    /// <summary>
    /// Controls the collapse of the small tower and shuttle launch (when the player outside the exhibits, looking at them).
    /// </summary>
    public class TowerSmallCollapse : MonoBehaviour
    {
        // Animator
        [SerializeField]
        private Animator _towerAnim;
        [SerializeField]
        private Animator _shuttleAnim;

        private void VerifyUnityParameters()
        {
            if (_towerAnim == null)
            {
                FifthModJam.WriteLine("[TowerSmallCollapse] tower animator is null", OWML.Common.MessageType.Error);
            }
            if (_shuttleAnim == null)
            {
                FifthModJam.WriteLine("[TowerSmallCollapse] shuttle animator is null", OWML.Common.MessageType.Error);
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
        }

        private void OnTowerCollapse()
        {
            // The small tower/shuttle do not need animation, just switching to the post-collapse state
            _towerAnim?.Play("TOWER_AFTER", 0);
            _shuttleAnim?.Play("SHUTTLE_DONE", 0);
        }
    }
}
