using NewHorizons.Utility;
using UnityEngine;

namespace FifthModJam
{
    /// <summary>
    /// A CustomItem associated with a Species component.
    /// Used for the diorama door puzzle and the Nomai beam puzzle.
    /// </summary>
    [RequireComponent(typeof(SpeciesTypeData))]
    public class SpeciesItem : CustomItem
    {
        // This is for solving the diorama door puzzle, as well as the nomai beam in the exhibit
        private SpeciesEnum _species;
        public SpeciesEnum Species => _species;

        protected override void VerifyUnityParameters()
        {
            base.VerifyUnityParameters();

            var speciesData = this.GetComponent<SpeciesTypeData>();
            if (speciesData == null || speciesData.Species == SpeciesEnum.INVALID)
            {
                FifthModJam.WriteLine("[SpeciesItem] speciesData null or invalid", OWML.Common.MessageType.Error);
            }
            else
            {
                _species = speciesData.Species;
            }
        }
    }
}
