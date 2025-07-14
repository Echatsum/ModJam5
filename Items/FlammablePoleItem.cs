using UnityEngine;

namespace FifthModJam
{
    /// <summary>
    /// CustomItem specific to the Boat pole. Controls a flame component, for use with the torch puzzle.
    /// </summary>
    public class FlammablePoleItem : CustomItem
    {
        // The toggable fire on the pole item
        [SerializeField]
        private GameObject _flames;
        public bool IsIgnited => _flames?.activeSelf ?? false; // based on the activation of the flames. If gameObject is null, defaults to false

        // Audio and animator
        [SerializeField]
        private OWAudioSource _oneShotAudio;
        [SerializeField]
        private Animator _animator;

        protected override void VerifyUnityParameters()
        {
            base.VerifyUnityParameters();

            if (_flames == null)
            {
                FifthModJam.WriteLine($"[FlammablePoleItem] flames object is null", OWML.Common.MessageType.Error);
            }
            if (_oneShotAudio == null)
            {
                FifthModJam.WriteLine("[FlammablePoleItem] audio is null", OWML.Common.MessageType.Error);
            }
            if (_animator == null)
            {
                FifthModJam.WriteLine("[FlammablePoleItem] animator is null", OWML.Common.MessageType.Error);
            }
        }

        public override void Awake()
        {
            base.Awake();
            GlobalMessenger<float>.AddListener("PlayerCameraEnterWater", OnCameraEnterWater);
        }
        public override void OnDestroy()
        {
            base.OnDestroy();
            GlobalMessenger<float>.RemoveListener("PlayerCameraEnterWater", OnCameraEnterWater);
        }

        protected override void Start()
        {
            base.Start();
            _flames?.SetActive(false); // Item starts extinguished
        }

        private void OnCameraEnterWater(float _)
        {
            // [Note to self@Stache: This trigger even when the player doesn't hold the item. Polish when have time so that we toggle flames only if item is held]

            ToggleFlames(false);
        }

        public void ToggleFlames(bool isIgniting)
        {
            if (_flames == null) return; // Ignore method if parameter hasn't been set

            if (isIgniting == IsIgnited) return; // Trying to lit an already lit flame, or extinguish an already extinguished flame

            if (isIgniting)
            {
                _flames.SetActive(true);
                _animator?.Play("FLAME", 0);
                _oneShotAudio?.PlayOneShot(global::AudioType.TH_Campfire_Ignite, 0.5f);
            }
            else
            {
                _flames.SetActive(false);
                _oneShotAudio?.PlayOneShot(global::AudioType.Artifact_Extinguish, 0.5f);
            }
        }
    }
}
