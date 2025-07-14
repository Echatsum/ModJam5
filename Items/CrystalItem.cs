using UnityEngine;

namespace FifthModJam
{
    /// <summary>
    /// SpeciesItem specific to the Knowledge Crystal, in order to host the animator.
    /// </summary>
    public class CrystalItem : SpeciesItem
    {
        // Audio and animator
        [SerializeField]
        private OWAudioSource _oneShotAudio; // [Note: Unused? Don't forget to add in the VerifyUnityParameters if that changes]
        [SerializeField]
        private Animator _animator;

        protected override void VerifyUnityParameters()
        {
            base.VerifyUnityParameters();

            if (_animator == null)
            {
                FifthModJam.WriteLine("[CrystalItem] animator is null", OWML.Common.MessageType.Error);
            }
        }

        public override void DropItem(Vector3 position, Vector3 normal, Transform parent, Sector sector, IItemDropTarget customDropTarget)
        {
            _animator?.Play("KAV_CRYSTAL", 0);

            base.DropItem(position, normal, parent, sector, customDropTarget);
        }

        public override void PickUpItem(Transform holdTranform)
        {
            _animator?.Play("KAV_CRYSTAL_STATIC", 0);

            base.PickUpItem(holdTranform);
        }
    }
}
