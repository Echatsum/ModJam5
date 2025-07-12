using UnityEngine;

namespace FifthModJam
{
    public class FlameTrigger : MonoBehaviour
    {
        [SerializeField]
        private bool isIgniter;
        [SerializeField]
        private GameObject torchFlame;
        [SerializeField]
        private OWAudioSource audio;
        [SerializeField]
        private Animator _animator;

        public bool hasIgnitedTorch;
        public bool hasIgnitedPole;

        private void Start()
        {
            hasIgnitedTorch = false;
            hasIgnitedPole = false;
            if (!isIgniter)
            {
                torchFlame.SetActive(false);
            }
        }

        public virtual void OnTriggerEnter(Collider hitCollider)
        {
            //checks if player collides with the trigger volume
            if (hitCollider.CompareTag("PlayerDetector") && enabled)
            {
                if (HasBoatPole())
                {
                    if (isIgniter && GetPole().IsBoatPoleLit() == false && !hasIgnitedPole)
                    {
                        GetPole().ToggleFlames(true);
                        hasIgnitedPole = true;
                    }
                    else if (!isIgniter && GetPole().IsBoatPoleLit() && !hasIgnitedTorch)
                    {
                        torchFlame.SetActive(true);
                        _animator.Play("FLAME", 0);
                        PlayAudio(true);
                        hasIgnitedTorch = true;
                    }
                }
            } 
        }

        public CustomItem GetPole()
        {
            if (Locator.GetToolModeSwapper()?.GetItemCarryTool()?.GetHeldItem() is CustomItem item &&
                item.speciesTypeData.species == SpeciesEnum.STRANGER &&
                item.itemType == ItemType.VisionTorch)
            {
                return item;
            } else
            {
                return null;
            }
        }

        public bool HasBoatPole()
        {
            if (Locator.GetToolModeSwapper()?.GetItemCarryTool()?.GetHeldItem() is CustomItem item &&
                item.speciesTypeData.species == SpeciesEnum.STRANGER &&
                item.itemType == ItemType.VisionTorch)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private void PlayAudio(bool isIgniting)
        {
            if (isIgniting)
            {
                audio.PlayOneShot(global::AudioType.TH_Campfire_Ignite, 0.5f);
            }
            else
            {
                audio.PlayOneShot(global::AudioType.Artifact_Extinguish, 0.5f);
            }
        }
    }
}