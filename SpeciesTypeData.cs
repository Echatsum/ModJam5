using UnityEngine;

// [TODO: Move to Utils/ folder once safe for push/pull]

namespace FifthModJam
{
    public class SpeciesTypeData : MonoBehaviour
    {
        [SerializeField]
        private SpeciesEnum _species;
        public SpeciesEnum Species => _species;
    }
}
