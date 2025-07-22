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
                _beam?.SetActive(true);
            }
        }
        private void OnSocketRemoved(OWItem item)
        {
            _beam?.SetActive(false);
        }
    }
}