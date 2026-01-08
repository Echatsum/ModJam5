using UnityEngine;

namespace FifthModJam
{
    public class ItemsReturnedAchievementTrigger : MonoBehaviour
    {
        [SerializeField]
        private SpeciesEnum _diorama;

        private void Start()
        {
            this.enabled = false;
        }


        public virtual void OnTriggerEnter(Collider hitCollider)
        {
            if (hitCollider.CompareTag("PlayerDetector"))
            {
                ItemsReturnedAchievementManager.Instance.SetCurrentDiorama(_diorama);
            }
        }
        public virtual void OnTriggerExit(Collider hitCollider)
        {
            if (hitCollider.CompareTag("PlayerDetector"))
            {
                ItemsReturnedAchievementManager.Instance.SetCurrentDiorama(SpeciesEnum.INVALID); // No overlap between the dioramas, so this is fine
            }
        }
    }
}
