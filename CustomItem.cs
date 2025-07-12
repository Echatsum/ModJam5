using NewHorizons.Utility;
using System;
using UnityEngine;

namespace FifthModJam
{
    [RequireComponent(typeof(SpeciesTypeData))]
    public class CustomItem : OWItem
    {
        [SerializeField]
        private OWAudioSource _oneShotAudio;
        [SerializeField]
        public string itemName;
        [SerializeField]
        public ItemType itemType; // This is for item-socket compatibility
        [SerializeField]
        private Animator _animator;
        [SerializeField]
        public GameObject flames;
        [SerializeField]
        public SpeciesTypeData speciesTypeData; // This is for solving the door puzzle, as well as some other stuff (animators, etc?)

        public override void Awake()
        {
            _type = itemType;
            base.Awake();
        }

        private void Start()
        {
            base.enabled = false;
            if (speciesTypeData.species == SpeciesEnum.STRANGER && itemType == ItemType.VisionTorch && flames != null)
            {
                GlobalMessenger<float>.AddListener("PlayerCameraEnterWater", OnCameraEnterWater);
                flames.SetActive(false);
            }
        }

        public override void OnDestroy()
        {
            base.OnDestroy();
        }

        private void OnCameraEnterWater(float _)
        {
            if (speciesTypeData.species == SpeciesEnum.STRANGER && itemType == ItemType.VisionTorch && flames != null && flames.activeSelf)
            {
                ToggleFlames(false);
            }
        }

        public override string GetDisplayName()
        {
            return itemName;
        }

        public override void SocketItem(Transform socketTransform, Sector sector)
        {
            base.SocketItem(socketTransform, sector);
        }

        public override void DropItem(Vector3 position, Vector3 normal, Transform parent, Sector sector, IItemDropTarget customDropTarget)
        {
            if (_animator != null && speciesTypeData.species == SpeciesEnum.KARVI)
            {
                _animator.Play("KAV_CRYSTAL", 0);
            }
            if (speciesTypeData.species == SpeciesEnum.NOMAI)
            {
                Sector desiredSector;
                if (Locator.GetPlayerSectorDetector().IsWithinSector("ScaledMuseum")) {
                    desiredSector = SearchUtilities.Find("ScaledMuseum_Body/Sector").GetComponent<Sector>();
                } else
                {
                    desiredSector = SearchUtilities.Find("OminousOrbiter_Body/Sector").GetComponent<Sector>();
                }
                this.SetSector(desiredSector);
            }
            base.DropItem(position, normal, parent, sector, customDropTarget);
        }

        public override void PickUpItem(Transform holdTranform)
        {
            if (_animator != null && speciesTypeData.species == SpeciesEnum.KARVI)
            {
                _animator.Play("KAV_CRYSTAL_STATIC", 0);
            }
            base.PickUpItem(holdTranform);
        }

        public bool IsBoatPoleLit()
        {
            if (speciesTypeData.species == SpeciesEnum.STRANGER && itemType == ItemType.VisionTorch && flames != null && flames.activeSelf)
            {
                return true;
            } else
            {
                return false;
            }
        }

        public void ToggleFlames(bool isIgniting)
        {
            if (speciesTypeData.species == SpeciesEnum.STRANGER && itemType == ItemType.VisionTorch && flames != null)
            {
                flames.SetActive(isIgniting);
                if (isIgniting)
                {
                    _animator.Play("FLAME", 0);
                    _oneShotAudio.PlayOneShot(global::AudioType.TH_Campfire_Ignite, 0.5f);
                } else
                {
                    _oneShotAudio.PlayOneShot(global::AudioType.Artifact_Extinguish, 0.5f);
                }
            }
        }

        public override void UpdateCollisionLOD()
        {
            base.UpdateCollisionLOD();
        }
    }
}