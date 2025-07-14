using UnityEngine;

namespace FifthModJam
{
    public class SpeciesTypeData : MonoBehaviour
    {
        [SerializeField]
        private SpeciesEnum _species;
        public SpeciesEnum Species => _species;
    }
}
