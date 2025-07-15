using System;
using UnityEngine;

namespace FifthModJam
{
    /// <summary>
    /// Controller of the beam puzzle.
    /// Solved when its socket has the correct item.
    /// </summary>
    public class TractorBeamPuzzle : MonoBehaviour
    {
        [SerializeField]
        private SpeciesItemSocket _speciesItemSocket;
        [SerializeField]
        private GameObject _tractorBeam;

        private void VerifyUnityParameters()
        {
            if (_speciesItemSocket == null)
            {
                FifthModJam.WriteLine("[TractorBeamPuzzle] socket is null", OWML.Common.MessageType.Error);
            }
            if (_tractorBeam == null)
            {
                FifthModJam.WriteLine("[TractorBeamPuzzle] tractorBeam is null", OWML.Common.MessageType.Error);
            }
        }

        private void Awake()
        {
            if (_speciesItemSocket != null)
            {
                _speciesItemSocket.OnSocketablePlaced = (OWItemSocket.SocketEvent)Delegate.Combine(_speciesItemSocket.OnSocketablePlaced, new OWItemSocket.SocketEvent(OnSocketFilled));
                _speciesItemSocket.OnSocketableRemoved = (OWItemSocket.SocketEvent)Delegate.Combine(_speciesItemSocket.OnSocketableRemoved, new OWItemSocket.SocketEvent(OnSocketRemoved));
            }
        }
        private void OnDestroy()
        {
            if (_speciesItemSocket != null)
            {
                _speciesItemSocket.OnSocketablePlaced = (OWItemSocket.SocketEvent)Delegate.Remove(_speciesItemSocket.OnSocketablePlaced, new OWItemSocket.SocketEvent(OnSocketFilled));
                _speciesItemSocket.OnSocketableRemoved = (OWItemSocket.SocketEvent)Delegate.Remove(_speciesItemSocket.OnSocketableRemoved, new OWItemSocket.SocketEvent(OnSocketRemoved));
            }
        }

        private void Start()
        {
            VerifyUnityParameters();

            _tractorBeam?.SetActive(false);
        }

        private void OnSocketFilled(OWItem item)
        {
            CheckActivation();
        }
        private void CheckActivation()
        {
            if (_speciesItemSocket.HasCorrectSpeciesItem())
            {
                _tractorBeam?.SetActive(true);
            }
        }

        private void OnSocketRemoved(OWItem item)
        {
            _tractorBeam?.SetActive(false);
        }
    }
}