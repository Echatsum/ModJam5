using NewHorizons.Utility;
using UnityEngine;

namespace FifthModJam
{
    public class MuseumDisabler : MonoBehaviour
    {
        private GameObject museum;

        public void Start()
        {
            if (FifthModJam.Instance.IsInJamFiveSystem())
            {
                museum = GameObject.Find("ScaledMuseum_Body/Sector"); // get museum
                museum.SetActive(false); // disables museum every loop
            }
        }
    }
}