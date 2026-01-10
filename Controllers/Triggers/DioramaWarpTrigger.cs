using System;
using System.Collections.Generic;
using UnityEngine;

namespace FifthModJam.Controllers.Triggers
{
    public class DioramaWarpTrigger : MonoBehaviour
    {
        // Which target this trigger should warp the player to, and whether it is an entry or exit
        [SerializeField]
        private DioramaSpawnPointEnum _warpTargetDiorama;
        [SerializeField]
        private bool _isExhibitEntry;
        [SerializeField]
        private bool _isSoftTrigger; // For EXIT types: "hard" is when warping happens all the time, "soft" is when warping only happens when out of all overlapping triggers

        private List<Tuple<Collider, bool, int>> _triggerList = new List<Tuple<Collider, bool, int>>();
        private const int FRAME_COUNTDOWN = 2;

        private void VerifyUnityParameters()
        {
            if (_warpTargetDiorama == DioramaSpawnPointEnum.INVALID)
            {
                FifthModJam.WriteLine("[DioramaWarpTrigger] targetDiorama left on invalid", OWML.Common.MessageType.Error);
            }
        }
        private void Start()
        {
            VerifyUnityParameters();
            this.enabled = false;
        }
        private void OnTriggerEntryRecheck(Collider hitCollider)
        {
            if (hitCollider.CompareTag("PlayerDetector"))
            {
                if (!_isExhibitEntry)
                {
                    if (_isSoftTrigger)
                    {
                        DioramaWarpManager.Instance.OnEnteringExitSoftTrigger(); // Notify manager about entering a 'soft exit' type
                    }
                    return; // Continue only if this trigger is an ENTRY
                }

                DioramaWarpManager.Instance.WarpTo(_warpTargetDiorama, isEnteringDiorama: true);
            }
        }
        private void OnTriggerExitRecheck(Collider hitCollider)
        {
            if (_isExhibitEntry) return; // Continue only if this trigger is an EXIT

            if (hitCollider.CompareTag("PlayerDetector"))
            {
                var shouldWarp = true;
                if (_isSoftTrigger)
                {
                    shouldWarp = DioramaWarpManager.Instance.OnExitingExitSoftTrigger(); // Notify manager about exiting a 'soft exit' type. If not last overlap, then do not warp
                }

                if (shouldWarp)
                {
                    DioramaWarpManager.Instance.WarpTo(_warpTargetDiorama, isEnteringDiorama: false);
                }
            }
        }

        public virtual void OnTriggerEnter(Collider hitCollider)
        {
            if (hitCollider.CompareTag("PlayerDetector"))
            {
                var addedTuple = UpdateTriggerList(hitCollider, isEntry:true, FRAME_COUNTDOWN);
                if (addedTuple)
                {
                    this.enabled = true;
                }
            }
        }
        public virtual void OnTriggerExit(Collider hitCollider)
        {
            if (hitCollider.CompareTag("PlayerDetector"))
            {
                var addedTuple = UpdateTriggerList(hitCollider, isEntry:false, FRAME_COUNTDOWN);
                if (addedTuple)
                {
                    this.enabled = true;
                }
            }
        }

        private bool UpdateTriggerList(Collider collider, bool isEntry, int frameCountdown)
        {
            var addNewTuple = true;
            for (int i = 0; i < _triggerList.Count; i++)
            {
                var triggerTag = _triggerList[i].Item1.tag;
                var wasEntry = _triggerList[i].Item2;
                if (collider.CompareTag(triggerTag) && (isEntry != wasEntry)) // new trigger cancels a previous trigger
                {
                    _triggerList.RemoveAt(i);
                    addNewTuple = false;
                    break;
                }
            }

            if (addNewTuple)
            {
                _triggerList.Add(new Tuple<Collider, bool, int>(collider, isEntry, frameCountdown));
            }
            return addNewTuple;
        }

        private void Update()
        {
            for (int i = 0; i < _triggerList.Count; i++)
            {
                var trigger = _triggerList[i];
                var collider = trigger.Item1;
                var isEntry = trigger.Item2;
                var frameCountdown = trigger.Item3;

                if (frameCountdown > 0)
                {
                    _triggerList[i] = new Tuple<Collider, bool, int>(collider, isEntry, frameCountdown - 1); // keep waiting some frames
                    continue;
                }

                _triggerList.RemoveAt(i);
                i--;

                if (isEntry)
                {
                    OnTriggerEntryRecheck(collider);
                }
                else
                {
                    OnTriggerExitRecheck(collider);
                }
            }

            if(_triggerList.Count == 0)
            {
                this.enabled = false;
            }
        }

    }
}
