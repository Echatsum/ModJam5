using UnityEngine;

namespace FifthModJam
{
    public class TeamPictureAchievement : MonoBehaviour
    {
		[SerializeField]
		protected RotatingDoor _door;

		protected virtual void Awake()
		{
			_door.OnOpenFinish += OnOpenFinish;
		}

		protected virtual void OnDestroy()
		{
			_door.OnOpenFinish -= OnOpenFinish;
		}

		private void OnOpenFinish()
		{
			FifthModJam.AchievementsAPI?.EarnAchievement(Constants.ACHIEVEMENT_YOU_FOUND_US);
		}
	}
}
