using UnityEngine;

// [TODO: Move to Controllers/ folder once safe for push/pull]

namespace FifthModJam
{
    public class MuseumDisabler : MonoBehaviour
    {
        public void Start()
        {
            if (!FifthModJam.Instance.IsInJamFiveSystem())
            {
                return;
            }

            // Get museum
            const string museumPath = Constants.UNITYPATH_MUSEUM;
            var museum = GameObject.Find(museumPath);
            if (museum == null)
            {
                FifthModJam.WriteLineObjectOrComponentNotFound("MuseumDisabler", museumPath);
            }
            else
            {
                FifthModJam.WriteLineReady("MuseumDisabler");
            }

            // Disables museum every loop
            if (museum != null)
            {
                museum.SetActive(false);
            }
        }
    }
}