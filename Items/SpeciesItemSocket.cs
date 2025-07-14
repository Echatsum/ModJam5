using UnityEngine;

namespace FifthModJam
{
    /// <summary>
    /// A CustomItem associated with a Species component.
    /// Used for the diorama door puzzle and the Nomai beam puzzle.
    /// </summary>
    public class SpeciesItemSocket : CustomItemSocket
    {
        // This is for solving the door puzzle
        [SerializeField]
        private SpeciesEnum _desiredSpecies;

        protected override void VerifyUnityParameters()
        {
            base.VerifyUnityParameters();

            if (_desiredSpecies == SpeciesEnum.INVALID)
            {
                FifthModJam.WriteLine("[SpeciesItemSocket] desiredSpecies left on invalid", OWML.Common.MessageType.Error);
            }
        }

        public bool HasCorrectSpeciesItem()
        {
            if (!this.IsSocketOccupied())
            {
                return false;
            }

            // Try to get Species data from the socketed item
            var item = this.GetSocketedItem();
            var speciesTypeData = item.GetComponent<SpeciesTypeData>();
            if (speciesTypeData == null)
            {
                return false; // Defaults to false if not found
            }

            return speciesTypeData.Species == _desiredSpecies;
        }
    }
}
