using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace FifthModJam.Controllers
{
    /// <summary>
    /// Controls the collapse of the small tower (when the player outside the exhibits, looking at them).
    /// </summary>
    public class TowerSmallCollapse : MonoBehaviour
    {
        // Animator
        [SerializeField]
        private Animator _towerAnim;

        private void VerifyUnityParameters()
        {
            if (_towerAnim == null)
            {
                FifthModJam.WriteLine("[TowerSmallCollapse] tower animator is null", OWML.Common.MessageType.Error);
            }
        }

        private void Awake()
        {
            TowerCollapseManager.Instance.OnTowerCollapse = (TowerCollapseManager.TowerCollapseEvent)Delegate.Combine(TowerCollapseManager.Instance.OnTowerCollapse, new TowerCollapseManager.TowerCollapseEvent(OnTowerCollapse));
        }
        private void OnDestroy()
        {
            TowerCollapseManager.Instance.OnTowerCollapse = (TowerCollapseManager.TowerCollapseEvent)Delegate.Remove(TowerCollapseManager.Instance.OnTowerCollapse, new TowerCollapseManager.TowerCollapseEvent(OnTowerCollapse));
        }

        private void Start()
        {
            VerifyUnityParameters();
        }

        private void OnTowerCollapse()
        {
            _towerAnim.Play("TOWER_AFTER", 0);
        }
    }
}
