using UnityEngine;

namespace FifthModJam
{
    /// <summary>
    /// The basic expanded OWItemSocket class for custom mod itemSockets.
    /// Allows for custom mask on the accepted item types.
    /// </summary>
    public class CustomItemSocket : OWItemSocket
    {
        [SerializeField]
        private ItemType _acceptableTypesMask; // This is for item-socket compatibility

        protected virtual void VerifyUnityParameters()
        {
            if (_acceptableTypesMask == ItemType.Invalid)
            {
                FifthModJam.WriteLine("[CustomItemSocket] type mask has no accepted value", OWML.Common.MessageType.Error);
            }
        }

        public override void Awake()
        {
            base.Awake();
            _acceptableType = _acceptableTypesMask;
        }

        protected virtual void Start()
        {
            VerifyUnityParameters();
        }
    }
}