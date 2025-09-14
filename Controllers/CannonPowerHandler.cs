using System;
using UnityEngine;

namespace FifthModJam
{
    public class CannonPowerHandler : MonoBehaviour
    {
        // The power socket
        [SerializeField]
        private SpeciesItemSocket _powerSocket;

        // Audio to play when change
        [SerializeField]
        private OWAudioSource _audio;

        // Nomai beam
        [SerializeField]
        private GameObject _beam;

        // Lights
        [SerializeField]
        private MeshRenderer[] lightsRender;
        [SerializeField]
        private GameObject[] lights;
        private readonly Color defaultColor = new Color(1.5f, 0.96f, 0.5699999f);

        // Property for other scripts, using the beam activity as boolean
        public bool IsCannonPowered => _beam?.activeSelf ?? false;

        private void VerifyUnityParameters()
        {
            if (_powerSocket == null)
            {
                FifthModJam.WriteLine("[CannonPowerHandler] power socket is null", OWML.Common.MessageType.Error);
            }
            if (_audio == null)
            {
                FifthModJam.WriteLine("[CannonPowerHandler] audio is null", OWML.Common.MessageType.Error);
            }
            if (_beam == null)
            {
                FifthModJam.WriteLine("[CannonPowerHandler] beam is null", OWML.Common.MessageType.Error);
            }
        }

        private void Start()
        {
            VerifyUnityParameters();

            TogglePower(isTurningOn: false, silent: true);
        }

        private void Awake()
        {
            if (_powerSocket != null)
            {
                _powerSocket.OnSocketablePlaced += OnSocketFilled;
                _powerSocket.OnSocketableRemoved += OnSocketRemoved;
            }
        }
        private void OnDestroy()
        {
            if (_powerSocket != null)
            {
                _powerSocket.OnSocketablePlaced -= OnSocketFilled;
                _powerSocket.OnSocketableRemoved -= OnSocketRemoved;
            }
        }

        private void OnSocketFilled(OWItem item)
        {
            if (_powerSocket.HasCorrectSpeciesItem())
            {
                Locator.GetShipLogManager().RevealFact("COSMICCURATORS_NOMAI_CANNON_POWERED");
                TogglePower(isTurningOn: true);
            }
        }
        private void OnSocketRemoved(OWItem item)
        {
            if (IsCannonPowered)
            {
                TogglePower(isTurningOn: false);
            }
        }

        private void TogglePower(bool isTurningOn, bool silent = false)
        {
            // Play audio if not silent
            if (!silent)
            {
                if (isTurningOn)
                {
                    _audio?.PlayOneShot(global::AudioType.NomaiPowerOn, 1f);
                }
                else
                {
                    _audio?.PlayOneShot(global::AudioType.NomaiPowerOff, 1f);
                }
            }

            // Update game objects
            _beam?.SetActive(isTurningOn);
            ToggleLights(isTurningOn);
        }
        private void ToggleLights(bool isTurningOn)
        {
            foreach (GameObject light in lights)
            {
                light.SetActive(isTurningOn);
            }
            foreach (MeshRenderer render in lightsRender)
            {
                Material[] mats = render.materials;
                if (isTurningOn)
                {
                    mats[0].SetColor("_Color", Color.white);
                    mats[0].SetColor("_EmissionColor", defaultColor);
                } else
                {
                    mats[0].SetColor("_Color", Color.black);
                    mats[0].SetColor("_EmissionColor", Color.black);
                }
                render.materials = mats;
            }
        }
    }
}