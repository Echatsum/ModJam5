using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace FifthModJam
{
    public class FlameStruct : MonoBehaviour
    {
        [SerializeField]
        private FlameColorEnum _color;
        public FlameColorEnum Color => _color;

        [SerializeField]
        private Animator _animator;
        public Animator Animator => _animator;
    }

    /// <summary>
    /// CustomItem specific to the Boat pole. Controls a flame component, for use with the torch puzzle.
    /// </summary>
    public class FlammablePoleItem : CustomItem
    {
        // The toggable fire on the pole item
        private List<FlameStruct> _flames;
        private List<string> _animatorStateNames = new()
        {
            { "FLAME_MAIN" },
            { "FLAME_2" },
            { "FLAME_3" },
            { "FLAME_4" },
            { "FLAME_5" },
            { "FLAME_6" }
        };

        private int _ignitedCount;
        public bool IsIgnited => _ignitedCount > 0;

        // Audio
        [SerializeField]
        private OWAudioSource _oneShotAudio;

        protected override void VerifyUnityParameters()
        {
            base.VerifyUnityParameters();

            _flames = GetComponentsInChildren<FlameStruct>().ToList();

            if (_oneShotAudio == null)
            {
                FifthModJam.WriteLine("[FlammablePoleItem] audio is null", OWML.Common.MessageType.Error);
            }

            if (_flames == null)
            {
                FifthModJam.WriteLine("[FlammablePoleItem] no flame found", OWML.Common.MessageType.Error);
            }
            else
            {
                foreach (var flame in _flames)
                {
                    if (flame.Animator == null)
                    {
                        FifthModJam.WriteLine("[FlammablePoleItem] animator null on a flame", OWML.Common.MessageType.Error);
                    }
                }
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

            _ignitedCount = 0;
            if (_flames != null)
            {
                foreach(var flame in _flames)
                {
                    flame?.gameObject.SetActive(false);
                }
            }
        }

        private void OnCameraEnterWater(float _)
        {
            if (this.IsItemHeld)
            {
                ToggleFlames(false);
            }
        }

        public void ToggleFlames(bool isIgniting, FlameColorEnum flameColor = FlameColorEnum.PURPLE)
        {
            if (_flames == null) return; // Ignore method if parameter hasn't been set

            if (isIgniting)
            {
                var flame = FindFlame(flameColor);
                if (flame == null || flame.gameObject.activeSelf) return; // Igniting an already lit color = do nothing

                // Ignite flame: turn on game object, play correct animator, play audio
                flame.gameObject.SetActive(true);
                var stateToPlay = _animatorStateNames[Mathf.Min(_ignitedCount,_animatorStateNames.Count-1)];
                flame.Animator?.Play(stateToPlay, 0);
                _oneShotAudio?.PlayOneShot(global::AudioType.TH_Campfire_Ignite, 0.5f);

                // Update flame count and check for achievement
                _ignitedCount++;
                FifthModJam.WriteLine("[FlammablePoleItem] ignitedCount is now " + _ignitedCount + ". Color added: " + flameColor, OWML.Common.MessageType.Warning);
                if (_ignitedCount >= 6)
                {
                    FifthModJam.AchievementsAPI.EarnAchievement(Constants.ACHIEVEMENT_INFINITY_STICK);
                }
            }
            else
            {
                if (!IsIgnited) return; // Extinguishing an already extinguished pole = do nothing

                foreach (var flame in _flames)
                {
                    flame?.gameObject.SetActive(false);
                }
                _oneShotAudio?.PlayOneShot(global::AudioType.Artifact_Extinguish, 0.5f);

                _ignitedCount = 0;
                FifthModJam.WriteLine("[FlammablePoleItem] ignitedCount is now " + _ignitedCount, OWML.Common.MessageType.Warning);
            }
        }

        private FlameStruct FindFlame(FlameColorEnum flameColor)
        {
            if (_flames == null) return null;

            foreach (var flame in _flames)
            {
                if (flame.Color == flameColor) return flame;
            }

            FifthModJam.WriteLine("[FlammablePoleItem] could not find flame of color " + flameColor, OWML.Common.MessageType.Warning);
            return null;
        }
    }
}
