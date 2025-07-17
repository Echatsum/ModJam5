using NewHorizons.Utility;
using UnityEngine;

namespace FifthModJam
{
    public class MuseumDisablerManager : AbstractManager<MuseumDisablerManager>
    {
        // museum gameobject
        private GameObject _museum;

        private bool _hasRegisteredObjects = false;

        private bool RegisterGameobjects()
        {
            bool flag1 = true;

            // Get museum
            _museum = SearchUtilities.Find(Constants.UNITYPATH_MUSEUM);
            if (_museum == null)
            {
                FifthModJam.WriteLineObjectOrComponentNotFound("DioramaWarpManager", Constants.UNITYPATH_MUSEUM);
                flag1 = false;
            }

            return flag1;
        }

        public void Start()
        {
            _hasRegisteredObjects = RegisterGameobjects();

            if (_hasRegisteredObjects)
            {
                FifthModJam.WriteLineReady("MuseumDisablerManager");
            }

            // Disables museum at the start of every loop
            _museum?.SetActive(false);
        }
    }
}
