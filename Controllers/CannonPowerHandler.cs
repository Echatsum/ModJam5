using System;
using UnityEngine;

namespace FifthModJam
{
    public class CannonPowerHandler : MonoBehaviour
    {
        // The power socket
        [SerializeField]
        private SpeciesItemSocket _powerSocket;
        [SerializeField]
        private OWAudioSource _audio; // [Note: Currently unused, but it's there if we want an extra sound]
        [SerializeField]
        private GameObject _beam;
        [SerializeField]
        private MeshRenderer[] lightsRender;
        [SerializeField]
        private GameObject[] lights;
        private readonly Color defaultColor = new Color(1.5f, 0.96f, 0.5699999f);

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
            HandleLights(false);
            _beam?.SetActive(false);
        }

        private void Awake()
        {
            if (_powerSocket != null)
            {
                _powerSocket.OnSocketablePlaced = (OWItemSocket.SocketEvent)Delegate.Combine(_powerSocket.OnSocketablePlaced, new OWItemSocket.SocketEvent(OnSocketFilled));
                _powerSocket.OnSocketableRemoved = (OWItemSocket.SocketEvent)Delegate.Combine(_powerSocket.OnSocketableRemoved, new OWItemSocket.SocketEvent(OnSocketRemoved));
            }
        }
        private void OnDestroy()
        {
            if (_powerSocket != null)
            {
                _powerSocket.OnSocketablePlaced = (OWItemSocket.SocketEvent)Delegate.Remove(_powerSocket.OnSocketablePlaced, new OWItemSocket.SocketEvent(OnSocketFilled));
                _powerSocket.OnSocketableRemoved = (OWItemSocket.SocketEvent)Delegate.Remove(_powerSocket.OnSocketableRemoved, new OWItemSocket.SocketEvent(OnSocketRemoved));
            }
        }

        private void OnSocketFilled(OWItem item)
        {
            if (_powerSocket.HasCorrectSpeciesItem())
            {
                Locator.GetShipLogManager().RevealFact("COSMICCURATORS_NOMAI_CANNON_POWERED");
                _audio.PlayOneShot(global::AudioType.NomaiPowerOn, 1f);
                HandleLights(true);
                _beam?.SetActive(true);
            }
        }
        private void OnSocketRemoved(OWItem item)
        {
            _audio.PlayOneShot(global::AudioType.NomaiPowerOff, 1f);
            HandleLights(false);
            _beam?.SetActive(false);
        }

        private void HandleLights(bool isTurningOn)
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