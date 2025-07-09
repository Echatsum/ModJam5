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
        private string itemName;
        [SerializeField]
        private ItemType itemType; // This is for item-socket compatibility
        [SerializeField]
        private Animator _animator;
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
        }

        public override void OnDestroy()
        {
            base.OnDestroy();
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
                /*foreach (var playerSector in Locator.GetPlayerSectorDetector()._sectorList)
                {
                    if (this.GetSector() != playerSector)
                    {
                        this.SetSector(playerSector);
                    }
                }*/
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

        public override void UpdateCollisionLOD()
        {
            base.UpdateCollisionLOD();
        }
    }
}