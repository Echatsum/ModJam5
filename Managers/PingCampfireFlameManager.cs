using NewHorizons.Utility;
using UnityEngine;

namespace FifthModJam
{
    public class PingCampfireFlameManager : AbstractManager<PingCampfireFlameManager>
    {
        // campfire
        private GameObject _pingCampfire;

        // trigger info
        private Vector3 _relativePosition = new(0, 1.75f, 0);
        private float _radius = 1.5f;

        private bool _hasRegisteredObjects = false;

        private bool RegisterGameobjects()
        {
            bool flag1 = true;

            // Get museum
            _pingCampfire = SearchUtilities.Find(Constants.UNITYPATH_PINGCAMPFIRE);
            if (_pingCampfire == null)
            {
                FifthModJam.WriteLineObjectOrComponentNotFound("PingCampfireFlameManager", Constants.UNITYPATH_PINGCAMPFIRE);
                flag1 = false;
            }

            return flag1;
        }

        public void Start()
        {
            if (!FifthModJam.Instance.IsInJamFiveSystem()) return;

            _hasRegisteredObjects = RegisterGameobjects();
            if (_hasRegisteredObjects)
            {
                FifthModJam.WriteLineReady("PingCampfireFlameManager");
            }

            var igniteTrigger = GenerateIgniteTrigger();
            igniteTrigger.transform.position = _pingCampfire.transform.TransformPoint(_relativePosition);
            igniteTrigger.transform.SetParent(_pingCampfire.transform);
        }

        private GameObject GenerateIgniteTrigger()
        {
            var obj = new GameObject("IgniteTrigger");
            obj.layer = LayerMask.NameToLayer("BasicEffectVolume"); // Reminder: need to be on that correct layer!

            // Collider component
            var collider = obj.AddComponent<SphereCollider>();
            collider.isTrigger = true;
            collider.radius = _radius;

            // OWCollider component
            var owCollider = obj.AddComponent<OWCollider>();

            // FlameTrigger script
            var flameTrigger = obj.AddComponent<FlameTrigger>();
            flameTrigger.Init(isIgnited: true, FlameColorEnum.TEAL);

            return obj;
        }
    }
}
