using OWML.ModHelper;

namespace FifthModJam
{
    public interface IAchievements
    {
        void RegisterAchievement(string uniqueID, bool secret, ModBehaviour mod);
        void RegisterAchievement(string uniqueID, bool secret, bool showDescriptionNotAchieved, ModBehaviour mod);
        void RegisterTranslation(string uniqueID, TextTranslation.Language language, string name, string description);
        void RegisterTranslation(string uniqueID, TextTranslation.Language language, string name, string description, string descriptionNotAchieved);
        void RegisterTranslationsFromFiles(ModBehaviour mod, string folderPath);
        void EarnAchievement(string uniqueID);
        bool HasAchievement(string uniqueID);
    }
}
