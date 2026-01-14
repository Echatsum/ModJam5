using UnityEngine;

namespace FifthModJam
{
    public class AchievementVolumeTrigger : MonoBehaviour
    {
        [SerializeField]
        private AchievementVolumeEnum _achievement;

        public virtual void OnTriggerEnter(Collider hitCollider)
        {
            if (hitCollider.CompareTag("PlayerDetector") && enabled)
            {
                if (_achievement == AchievementVolumeEnum.WALK_THE_PLANK)
                {
                    FifthModJam.AchievementsAPI?.EarnAchievement(Constants.ACHIEVEMENT_WALK_THE_PLANK);
                    return;
                }
                if (_achievement == AchievementVolumeEnum.FAT_SHAMING)
                {
                    FifthModJam.AchievementsAPI?.EarnAchievement(Constants.ACHIEVEMENT_FAT_SHAMING);
                    return;
                }
                if (_achievement == AchievementVolumeEnum.ONE_RING_TO_RULE_THEM_ALL)
                {
                    FifthModJam.AchievementsAPI?.EarnAchievement(Constants.ACHIEVEMENT_ONE_RING_TO_RULE_THEM_ALL);
                    return;
                }
            }
        }
    }
}
