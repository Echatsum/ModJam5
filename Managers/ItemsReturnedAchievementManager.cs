using System.Collections.Generic;

namespace FifthModJam
{
    public class ItemsReturnedAchievementManager : AbstractManager<ItemsReturnedAchievementManager>
    {
        private readonly Dictionary<SpeciesEnum, bool> _isItemReturned = new() {
            { SpeciesEnum.KARVI, false },
            { SpeciesEnum.STRANGER, false },
            { SpeciesEnum.NOMAI, false },
            { SpeciesEnum.HEARTHIAN, false },
        };

        private SpeciesEnum _currentDiorama = SpeciesEnum.INVALID;
        public void SetCurrentDiorama(SpeciesEnum species)
        {
            FifthModJam.WriteLine("[ItemsReturnedAchievementManager] SET DIORAMA: " + species, OWML.Common.MessageType.Warning);


            _currentDiorama = species;
        }

        public void RegisterSpeciesItemNoLongerHeld(SpeciesEnum species)
        {
            FifthModJam.WriteLine("[ItemsReturnedAchievementManager] DROPPED: " + species, OWML.Common.MessageType.Warning);

            if (!_isItemReturned.ContainsKey(species)) return; // Ignore what isn't in our dict (catch for the invalid value)

            if (species == _currentDiorama)
            {
                _isItemReturned[species] = true;
                CheckForAchievement();
            }
        }
        public void RegisterSpeciesItemHeld(SpeciesEnum species)
        {
            FifthModJam.WriteLine("[ItemsReturnedAchievementManager] PICKED UP: " + species, OWML.Common.MessageType.Warning);

            if (!_isItemReturned.ContainsKey(species)) return; // Ignore what isn't in our dict (catch for the invalid value)

            _isItemReturned[species] = false;
        }

        private void Start()
        {
            if (!FifthModJam.Instance.IsInJamFiveSystem()) return;

            FifthModJam.WriteLineReady("ItemsReturnedAchievementManager");
        }

        private void CheckForAchievement()
        {
            bool flag = true;
            foreach (var key in _isItemReturned.Keys)
            {
                flag = flag && _isItemReturned[key];
            }
            if (flag)
            {
                FifthModJam.AchievementsAPI.EarnAchievement(Constants.ACHIEVEMENT_ALL_IN_ITS_PLACE);
            }
        }
    }
}
