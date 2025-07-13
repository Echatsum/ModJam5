using UnityEngine;

namespace FifthModJam
{
    public class EndingTrigger : MonoBehaviour
    {
        [SerializeField]
        private GameObject music;
        [SerializeField]
        private TorchPuzzle torchPuzzle;

        private GameObject nhPlanet;
        private Sector desiredSector;
        private bool hasEnteredTrigger;

        private void Start()
        {
            hasEnteredTrigger = false;
            nhPlanet = FifthModJam.NewHorizonsAPI.GetPlanet("ScaledMuseum");
            desiredSector = nhPlanet.transform.Find("Sector").gameObject.GetComponent<Sector>();
            music.SetActive(false);
        }

        public virtual void OnTriggerEnter(Collider hitCollider)
        {
            //checks if player collides with the trigger volume
            if (hitCollider.CompareTag("PlayerDetector") && enabled && !hasEnteredTrigger && torchPuzzle.isPuzzleComplete)
            {
                music.SetActive(true);
                Locator.GetToolModeSwapper().GetItemCarryTool().DropItemInstantly(desiredSector, nhPlanet.transform);
                Locator.GetPlayerSuit().RemoveSuit();
            } 
        }
    }
}