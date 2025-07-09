using UnityEngine;

namespace FifthModJam
{
    public class CustomItemSocket : OWItemSocket
    {
        [SerializeField]
        public SpeciesEnum desiredSpecies; // This is for solving the door puzzle
        [SerializeField]
        public ItemType acceptableTypesMask; // This is for item-socket compatibility

        public override void Awake()
        {
            base.Awake();
            _acceptableType = acceptableTypesMask;
        }

        public bool HasCorrectSpeciesItem()
        {
            if (!this.IsSocketOccupied())
            {
                return false;
            }

            var item = this.GetSocketedItem();
            var speciesTypeData = item.GetComponent<SpeciesTypeData>();
            if (speciesTypeData != null)
            {
                return speciesTypeData.species == desiredSpecies;
            }

            return false;
        }
    }
}