using NewHorizons.Utility;
using UnityEngine;

namespace FifthModJam
{
    public class MuseumDisabler : MonoBehaviour
    {
        private GameObject museum;

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
                museum.SetActive(false); // disables museum every loop
            }
        }
    }
}