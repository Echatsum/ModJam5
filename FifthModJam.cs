using HarmonyLib;
using NewHorizons.Utility;
using OWML.Common;
using OWML.ModHelper;
using System.Collections;
using System.Reflection;
using UnityEngine;

namespace FifthModJam
{
    public class FifthModJam : ModBehaviour
    {
        public static INewHorizons NewHorizonsAPI { get; private set; }
        public static IAchievements AchievementsAPI { get; private set; }
        private static FifthModJam _instance;
        public static FifthModJam Instance
        {
            get
            {
                if (_instance == null) _instance = FindObjectOfType<FifthModJam>();
                return _instance;
            }
        }

        public void Start()
        {
            // Starting here, you'll have access to OWML's mod helper.
            ModHelper.Console.WriteLine($"My mod {nameof(FifthModJam)} is loaded!", MessageType.Success);

            // Get the New Horizons API and load configs
            NewHorizonsAPI = ModHelper.Interaction.TryGetModApi<INewHorizons>("xen.NewHorizons");
            NewHorizonsAPI.LoadConfigs(this);

            // Get the Achievement+ API
            AchievementsAPI = ModHelper.Interaction.TryGetModApi<IAchievements>("xen.AchievementTracker");
            if(AchievementsAPI != null)
            {
                RegisterAllAchievements();
            }

            // Harmony patching
            new Harmony("TheSignalJammers.FifthModJam").PatchAll(Assembly.GetExecutingAssembly());

            // Example of accessing game code.
            NewHorizonsAPI.GetStarSystemLoadedEvent().AddListener(OnCompleteSceneLoad);
        }

        public void OnCompleteSceneLoad(string newScene)
        {
            if (newScene != "Jam5") return;
            ModHelper.Console.WriteLine("Loaded into jam5 system!", MessageType.Success);

            DioramaWarpManager.Spawn();
            MuseumDisablerManager.Spawn();
            LanguageManager.Spawn();
            TowerCollapseManager.Spawn();
            ScoutCodeManager.Spawn();
            TotemCodePromptManager.Spawn();
            PingCampfireFlameManager.Spawn();
            ItemsReturnedAchievementManager.Spawn();
            FrequencyUpgradeManager.Spawn();
        }

        public bool IsInJamFiveSystem()
        {
            return NewHorizonsAPI.GetCurrentStarSystem() == "Jam5";
        }

        // UTILS
        public static void WriteLine(string text, MessageType messageType = MessageType.Message)
        {
            Instance.ModHelper.Console.WriteLine(text, messageType);
        }
        public static void WriteLineObjectOrComponentNotFound(string classLocation, string pathAttempted, string componentName = null)
        {
            string componentAddonStr = componentName == null ? "" : $" or component ({componentName})"; // Add this snippet if the component name is specified
            FifthModJam.WriteLine($"[{classLocation}] Could not find an object{componentAddonStr}. Check path attempted: {pathAttempted}", MessageType.Error);
        }
        public static void WriteLineReady(string classLocation)
        {
            FifthModJam.WriteLine($"[{classLocation}] Ready", MessageType.Debug);
        }

        // COROUTINES
        public IEnumerator CloseEyesCoroutine()
        {
            OWInput.ChangeInputMode(InputMode.None); // stop player input for a while
            var cameraEffectController = FindObjectOfType<PlayerCameraEffectController>();
            cameraEffectController.CloseEyes(Constants.BLINK_CLOSE_ANIM_TIME);
            yield return new WaitForSeconds(Constants.BLINK_CLOSE_ANIM_TIME);  // waits until animation stops to proceed to next line
            GlobalMessenger.FireEvent("PlayerBlink"); // fires an event for the player blinking
        }
        public IEnumerator OpenEyesCoroutine()
        {
            var cameraEffectController = FindObjectOfType<PlayerCameraEffectController>();
            cameraEffectController.OpenEyes(Constants.BLINK_OPEN_ANIM_TIME, false);
            yield return new WaitForSeconds(Constants.BLINK_OPEN_ANIM_TIME); // Waits until animation stops to proceed to next line
            OWInput.ChangeInputMode(InputMode.Character); // gives the player back input
        }

        // ACHIEVEMENTS
        public void RegisterAllAchievements()
        {
            // Story Achievements (fully secret)
            AchievementsAPI.RegisterAchievement(Constants.ACHIEVEMENT_SHRUNK_HATCHLING, secret: true, this);
            AchievementsAPI.RegisterAchievement(Constants.ACHIEVEMENT_WHATS_THIS_BUTTON, secret: true, this);
            AchievementsAPI.RegisterAchievement(Constants.ACHIEVEMENT_KNOCK_KNOCK, secret: true, this);
            AchievementsAPI.RegisterAchievement(Constants.ACHIEVEMENT_THE_COSMIC_CURATORS, secret: true, this);

            // Other Achievements (not hidden)
            AchievementsAPI.RegisterAchievement(Constants.ACHIEVEMENT_ERNESTO, secret: false, this);
            AchievementsAPI.RegisterAchievement(Constants.ACHIEVEMENT_YOU_FOUND_US, secret: false, this);
            AchievementsAPI.RegisterAchievement(Constants.ACHIEVEMENT_SCOUTLESS, secret: false, this);

            // Other Achievements (semi-hidden, with hint)
            AchievementsAPI.RegisterAchievement(Constants.ACHIEVEMENT_ONE_RING_TO_RULE_THEM_ALL, secret: false, showDescriptionNotAchieved: true, this);
            AchievementsAPI.RegisterAchievement(Constants.ACHIEVEMENT_INFINITY_STICK, secret: false, showDescriptionNotAchieved: true, this);
            AchievementsAPI.RegisterAchievement(Constants.ACHIEVEMENT_MIXED_PASSWORDS, secret: false, showDescriptionNotAchieved: true, this);
            AchievementsAPI.RegisterAchievement(Constants.ACHIEVEMENT_WALK_THE_PLANK, secret: false, showDescriptionNotAchieved: true, this);
            AchievementsAPI.RegisterAchievement(Constants.ACHIEVEMENT_ITS_ONLY_A_MODEL, secret: false, showDescriptionNotAchieved: true, this);
            AchievementsAPI.RegisterAchievement(Constants.ACHIEVEMENT_ALL_IN_ITS_PLACE, secret: false, showDescriptionNotAchieved: true, this);
            AchievementsAPI.RegisterAchievement(Constants.ACHIEVEMENT_FAT_SHAMING, secret: false, showDescriptionNotAchieved: true, this);

            // Add translations
            AchievementsAPI.RegisterTranslationsFromFiles(this, "translations/");
        }
    }
}
