using HarmonyLib;
using OWML.Common;
using OWML.ModHelper;
using System.Reflection;
using UnityEngine;

namespace FifthModJam
{
    public class FifthModJam : ModBehaviour
    {
        public static INewHorizons NewHorizonsAPI { get; private set; }

        public static FifthModJam Instance
        {
            get
            {
                if (instance == null) instance = FindObjectOfType<FifthModJam>();
                return instance;
            }
        }

        private static FifthModJam instance;

        public static void WriteLine(string text, MessageType messageType = MessageType.Message)
        {
            Instance.ModHelper.Console.WriteLine(text, messageType);
        }

        public void Start()
        {
            // Starting here, you'll have access to OWML's mod helper.
            ModHelper.Console.WriteLine($"My mod {nameof(FifthModJam)} is loaded!", MessageType.Success);

            // Get the New Horizons API and load configs
            NewHorizonsAPI = ModHelper.Interaction.TryGetModApi<INewHorizons>("xen.NewHorizons");
            NewHorizonsAPI.LoadConfigs(this);

            new Harmony("TheSignalJammers.FifthModJam").PatchAll(Assembly.GetExecutingAssembly());

            // Example of accessing game code.
            OnCompleteSceneLoad(OWScene.TitleScreen, OWScene.TitleScreen); // We start on title screen
            LoadManager.OnCompleteSceneLoad += OnCompleteSceneLoad;
        }

        public void OnCompleteSceneLoad(OWScene previousScene, OWScene newScene)
        {
            if (newScene != OWScene.SolarSystem) return;
            ModHelper.Console.WriteLine("Loaded into solar system!", MessageType.Success);
        }

        public bool IsInJamFiveSystem()
        {
            if (NewHorizonsAPI.GetCurrentStarSystem() == "Jam5")
            {
                return true;
            } else
            {
                return false;
            }
        }
    }

}
