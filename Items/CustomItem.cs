using UnityEngine;

namespace FifthModJam
{
    /// <summary>
    /// The basic expanded OWItem class for custom mod items.
    /// Allows for custom name and choosing ItemType equivalence.
    /// </summary>
    public class CustomItem : NewHorizons.Components.Props.NHItem
    {
        [SerializeField]
        protected string _customItemType;
        [SerializeField]
        protected string[] _entryLogs; // NHItem only allows for one entryLog on pickup. This class allows for more.

        private bool _isItemHeld;
        public bool IsItemHeld => _isItemHeld;

        protected virtual void VerifyUnityParameters()
        {
            var hasBaseType = ItemType != ItemType.Invalid;
            var hasCustomType = _customItemType != null && _customItemType.Length > 0;

            if (DisplayName == null || DisplayName.Length == 0)
            {
                FifthModJam.WriteLine($"[CustomItem] DisplayName null or empty", OWML.Common.MessageType.Error);
            }
            if (_sector == null) // Base code works but gets annoyed if we don't set a sector for the items
            {
                FifthModJam.WriteLine($"[CustomItem ({DisplayName})] sector is null", OWML.Common.MessageType.Warning);
            }

            if (!hasBaseType && !hasCustomType)
            {
                FifthModJam.WriteLine($"[CustomItem] itemType/customItemType has no accepted value", OWML.Common.MessageType.Error);
            }
            else if (hasCustomType)
            {
                ItemType = NewHorizons.Builder.Props.ItemBuilder.GetOrCreateItemType(_customItemType);
            }
        }

        protected virtual void Start()
        {
            VerifyUnityParameters();
            _isItemHeld = false;
        }

        public override void PickUpItem(Transform holdTranform)
        {
            base.PickUpItem(holdTranform);
            _isItemHeld = true;

            foreach(var entryLog in _entryLogs)
            {
                Locator.GetShipLogManager().RevealFact(entryLog);
            }
        }

        public override void DropItem(Vector3 position, Vector3 normal, Transform parent, Sector sector, IItemDropTarget customDropTarget)
        {
            base.DropItem(position, normal, parent, sector, customDropTarget);
            _isItemHeld = false;
        }
        public override void SocketItem(Transform socketTransform, Sector sector)
        {
            base.SocketItem(socketTransform, sector);
            _isItemHeld = false;
        }
    }
}