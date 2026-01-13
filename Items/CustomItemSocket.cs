using UnityEngine;

namespace FifthModJam
{
    /// <summary>
    /// The basic expanded OWItemSocket class for custom mod itemSockets.
    /// Allows for custom mask on the accepted item types.
    /// </summary>
    public class CustomItemSocket : NewHorizons.Components.Props.NHItemSocket
    {
        [SerializeField]
        protected ItemType _acceptedDefaultItemTypes;
        [SerializeField]
        protected string[] _acceptedCustomItemTypes;

        protected virtual void VerifyUnityParameters()
        {
            ItemType = _acceptedDefaultItemTypes;

            var hasBaseType = ItemType != ItemType.Invalid;
            var hasCustomType = _acceptedCustomItemTypes != null && _acceptedCustomItemTypes.Length > 0;
            if (!hasBaseType && !hasCustomType)
            {
                FifthModJam.WriteLine("[CustomItemSocket] type has no accepted value (default nor custom)", OWML.Common.MessageType.Error);
            }
            else
            {
                foreach (var customType in _acceptedCustomItemTypes)
                {
                    if (customType == null || customType.Length == 0)
                    {
                        FifthModJam.WriteLine("[CustomItemSocket] a CustomItem type is null or length zero", OWML.Common.MessageType.Error);
                    }
                    else
                    {
                        ItemType |= NewHorizons.Builder.Props.ItemBuilder.GetOrCreateItemType(customType);
                    }
                }
            }
        }

        protected virtual void Start()
        {
            base.Start();
            VerifyUnityParameters();
        }
        public override bool AcceptsItem(OWItem item)
        {
            if (item == null || item._type == ItemType.Invalid)
            {
                return false;
            }
            return (item._type & _acceptableType) == item._type; // Base game allows for a mask of accepted types, but NH makes a check for exact ==, so this class makes it a mask again!
        }
    }
}