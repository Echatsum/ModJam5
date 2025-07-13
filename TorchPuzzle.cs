
using UnityEngine;

namespace FifthModJam
{
    public class TorchPuzzle : MonoBehaviour
    {
        [SerializeField]
        private FlameTrigger[] torches;
        [SerializeField]
        private GameObject[] objectsToEnable;
        [SerializeField]
        private OWAudioSource audio;
        [SerializeField]
        private Animator beamAnim;

        public bool isPuzzleComplete;

        private void Start()
        {
            isPuzzleComplete = false;
            foreach (var enabledObject in objectsToEnable)
            {
                enabledObject.SetActive(false);
            }
        }

        private bool AreAllIgnited()
        {
            int amountEnabled = 0;
            for (int i = 0; i < torches.Length; i++)
            {
                if (torches[i].hasIgnitedTorch)
                {
                    amountEnabled++;
                }
            }

            if (amountEnabled ==  torches.Length)
            {
                return true;
            } else
            {
                return false;
            }
        }
        private void Update()
        {
            if (AreAllIgnited() && !isPuzzleComplete)
            {
                foreach (var disabledObject in objectsToEnable){
                    disabledObject.SetActive(true);
                    audio.PlayOneShot(global::AudioType.NomaiTractorBeamActivate, 0.5f);
                    beamAnim.Play("BEAM", 0);
                    isPuzzleComplete = true;
                }
            }
        }
    }
}