using UnityEngine;

namespace FifthModJam
{
    public class ModelArtifactItem : SpeciesItem
    {
        public override void Awake()
        {
            GlobalMessenger.AddListener("StopSleepingAtCampfire", OnStopSleepingAtCampfire);
            base.Awake();
        }

        public override void OnDestroy()
        {
            GlobalMessenger.RemoveListener("StopSleepingAtCampfire", OnStopSleepingAtCampfire);
            base.OnDestroy();
        }

        private void OnStopSleepingAtCampfire()
        {
            if (this.IsItemHeld)
            {
                FifthModJam.AchievementsAPI.EarnAchievement(Constants.ACHIEVEMENT_ITS_ONLY_A_MODEL);
            }
        }
    }
}
