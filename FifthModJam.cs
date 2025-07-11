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

        // museum and star gameobjects
        private GameObject _museum;
        private GameObject _starLight;
        // TowerCollapse controller
        private TowerCollapse _collapseHandler;

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

            RegisterGameobjects(); // Find game objects, for use on coroutines and other methods

            ModHelper.Console.WriteLine("Loaded into solar system!", MessageType.Success);
        }

        public bool IsInJamFiveSystem()
        {
            return NewHorizonsAPI.GetCurrentStarSystem() == "Jam5";
        }

        public void RegisterGameobjects()
        {
            // [Note: museum-dependent objects might be inactive during Start(), so SearchUtilities is used to catch these cases that GameObject cannot]
            bool isReady = true;
            string classLocation = "FifthModJam root";

            // Get starlight
            _starLight = GameObject.Find(Constants.UNITYPATH_STARLIGHT);
            if (_starLight == null)
            {
                FifthModJam.WriteLineObjectOrComponentNotFound(classLocation, Constants.UNITYPATH_STARLIGHT);
                isReady = false;
            }

            // Get museum
            _museum = SearchUtilities.Find(Constants.UNITYPATH_MUSEUM);
            if (_museum == null)
            {
                FifthModJam.WriteLineObjectOrComponentNotFound(classLocation, Constants.UNITYPATH_MUSEUM);
                isReady = false;
            }

            // Get the tower collapse handler
            _collapseHandler = SearchUtilities.Find(Constants.UNITYPATH_SCALEDMUSEUM)?.GetComponent<TowerCollapse>();
            if (_collapseHandler == null)
            {
                FifthModJam.WriteLineObjectOrComponentNotFound(classLocation, Constants.UNITYPATH_SCALEDMUSEUM, nameof(TowerCollapse));
                isReady = false;
            }

            // Everything registered correctly
            if (isReady)
            {
                FifthModJam.WriteLineReady(classLocation);
            }
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
            FifthModJam.WriteLine($"[{classLocation}] Ready", MessageType.Success); // [Note: Could make this a Debug message if we want]
        }

        // COROUTINES
        public void EnterDiorama(SpawnPoint spawnPointTarget)
        {
            StartCoroutine(EnterDioramaCoroutine(spawnPointTarget));
        }
        private IEnumerator EnterDioramaCoroutine(SpawnPoint spawnPointTarget)
        {
            // Close eyes
            yield return StartCoroutine(CloseEyesCoroutine());
            yield return new WaitForSeconds(Constants.BLINK_STAY_CLOSED_TIME);

            // Update objects
            _museum.SetActive(true);
            _starLight.SetActive(false);
            if (_collapseHandler.hasFallen)
            {
                _collapseHandler.ForceTowerFallenState(); // forces the tower to fallen down state if it has before
            }

            // Warp
            var spawner = GameObject.FindGameObjectWithTag("Player").GetRequiredComponent<PlayerSpawner>();
            spawner.DebugWarp(spawnPointTarget);

            // Open eyes
            yield return StartCoroutine(OpenEyesCoroutine());
        }

        public void ExitDiorama(SpawnPoint spawnPointTarget)
        {
            StartCoroutine(ExitDioramaCoroutine(spawnPointTarget));
        }
        private IEnumerator ExitDioramaCoroutine(SpawnPoint spawnPointTarget)
        {
            // Close eyes
            yield return StartCoroutine(CloseEyesCoroutine());

            // Warp
            var spawner = GameObject.FindGameObjectWithTag("Player").GetRequiredComponent<PlayerSpawner>();
            spawner.DebugWarp(spawnPointTarget);

            // Update objects
            _museum.SetActive(false);
            _starLight.SetActive(true);

            // Open eyes
            yield return new WaitForSeconds(Constants.BLINK_STAY_CLOSED_TIME);
            yield return StartCoroutine(OpenEyesCoroutine());
        }

        public IEnumerator CloseEyesCoroutine()
        {
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
        }
    }

}
