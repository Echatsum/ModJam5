using UnityEngine;

namespace FifthModJam
{
    public class CustomItemSocket : OWItemSocket
    {
        [SerializeField]
        public SpeciesEnum desiredSpecies; // This is for solving the door puzzle
        [SerializeField]
        public ItemType desiredType; // This is for item-socket compatibility
        public bool isActive;

        public override void Awake()
        {
            base.Awake();
            _acceptableType = desiredType;
        }

        public override bool PlaceIntoSocket(OWItem item)
        {
            if (base.PlaceIntoSocket(item) && item.GetComponent<CustomItem>().speciesItem == desiredSpecies)
            {
                isActive = true;
                return true;
            }
            isActive = false;
            return false;
        }

        public override OWItem RemoveFromSocket()
        {
            isActive = false;
            OWItem oWItem = base.RemoveFromSocket();
            return oWItem;
        }
    }
}