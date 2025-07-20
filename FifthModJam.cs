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
    }
}
