using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace FifthModJam
{
    // Dummy class because unity is bad at deserializing
    public class FlameStruct : MonoBehaviour
    {
        public FlameColorEnum color;
    }

    /// <summary>
    /// CustomItem specific to the Boat pole. Controls a flame component, for use with the torch puzzle.
    /// </summary>
    public class FlammablePoleItem : CustomItem
    {
        // The toggable fire on the pole item
        private List<FlameStruct> _flames;

        private bool _isIgnited;
        public bool IsIgnited => _isIgnited;

        // Audio and animator
        [SerializeField]
        private OWAudioSource _oneShotAudio;
        [SerializeField]
        private Animator _animator;

        protected override void VerifyUnityParameters()
        {
            base.VerifyUnityParameters();

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

            _isIgnited = false; // Item starts extinguished

            _flames = GetComponentsInChildren<FlameStruct>().ToList();
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

            if (isIgniting == IsIgnited) return; // Trying to lit an already lit flame, or extinguish an already extinguished flame

            _isIgnited = isIgniting;
            if (isIgniting)
            {
                foreach (var flame in _flames)
                {
                    if(flame.color == flameColor)
                    {
                        flame?.gameObject.SetActive(true);
                    }

                }
                _animator?.Play("FLAME", 0);
                _oneShotAudio?.PlayOneShot(global::AudioType.TH_Campfire_Ignite, 0.5f);
            }
            else
            {
                foreach (var flame in _flames)
                {
                    flame?.gameObject.SetActive(false);
                }
                _oneShotAudio?.PlayOneShot(global::AudioType.Artifact_Extinguish, 0.5f);
            }
        }
    }
}
