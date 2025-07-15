using UnityEngine;

namespace FifthModJam
{
    /// <summary>
    /// The basic expanded OWItem class for custom mod items.
    /// Allows for custom name and choosing ItemType equivalence.
    /// </summary>
    public class CustomItem : OWItem
    {
        // Name and socket type
        [SerializeField]
        protected string _itemName;
        [SerializeField]
        protected string entryLog;
        [SerializeField]
        protected ItemType _itemType; // This is for item-socket compatibility [Note: While this can be set as mask, try to keep it as only one flag]

        protected virtual void VerifyUnityParameters()
        {
            if (_itemName == null || _itemName.Length == 0)
            {
                FifthModJam.WriteLine($"[CustomItem] itemname null or empty", OWML.Common.MessageType.Error);
            }
            if (_itemType == ItemType.Invalid)
            {
                FifthModJam.WriteLine($"[CustomItem ({_itemName})] itemType has no accepted value", OWML.Common.MessageType.Error);
            }

            if (_sector == null) // Base code works but gets annoyed if we don't set a sector for the items
            {
                FifthModJam.WriteLine($"[CustomItem ({_itemName})] sector is null", OWML.Common.MessageType.Warning);
            }
        }

        public override void Awake()
        {
            _type = _itemType;
            base.Awake();
        }

        protected virtual void Start()
        {
            VerifyUnityParameters();
        }

        public override string GetDisplayName()
        {
            return _itemName;
        }

        public override void PickUpItem(Transform holdTranform)
        {
            if (!Locator.GetShipLogManager().IsFactRevealed(entryLog) && entryLog != null)
            {
                Locator.GetShipLogManager().RevealFact(entryLog);
            }
            base.PickUpItem(holdTranform);
        }
    }
}