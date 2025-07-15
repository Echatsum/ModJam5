using System;
using System.Collections;
using UnityEngine;

namespace FifthModJam
{
    /// <summary>
    /// Controller of the diorama door puzzle.
    /// </summary>
    public class DioramaDoor : MonoBehaviour
    {
        // Sockets that act as locks for the door
        [SerializeField]
        private SpeciesItemSocket[] _speciesItemSockets;

        // Audio and animator
        [SerializeField]
        private OWAudioSource _audio;
        [SerializeField]
        private Animator _doorAnim;

        // Door starts closed
        private bool _isDoorClosed = true;

        private void VerifyUnityParameters()
        {
            if (_speciesItemSockets == null || _speciesItemSockets.Length == 0)
            {
                FifthModJam.WriteLine("[DioramaDoor] socket array is null or empty", OWML.Common.MessageType.Error);
            }
            if (_audio == null)
            {
                FifthModJam.WriteLine("[DioramaDoor] audio is null", OWML.Common.MessageType.Error);
            }
            if (_doorAnim == null)
            {
                FifthModJam.WriteLine("[DioramaDoor] animator is null", OWML.Common.MessageType.Error);
            }
        }

        private void Start()
        {
            VerifyUnityParameters();
        }

        private bool AreAllSocketsCorrect()
        {
            foreach (var socket in _speciesItemSockets)
            {
                if (!socket.HasCorrectSpeciesItem()) // [Note: HasCorrectSpeciesItem also accounts for empty sockets]
                {
                    return false;
                }
            }

            // If we haven't found any wrong socket, then they're all filled correctly
            return true;
        }

        private void Awake()
        {
            // Link each socket to trigger OnSocketFilled [Note: this is so that we check for door opening only when needed and not on every Update]
            foreach (var socket in _speciesItemSockets)
            {
                // This big line is the equivalent of a += but with events
                socket.OnSocketablePlaced = (OWItemSocket.SocketEvent)Delegate.Combine(socket.OnSocketablePlaced, new OWItemSocket.SocketEvent(OnSocketFilled));
            }
        }
        private void OnDestroy()
        {
            // Cleanly removes links on destruction. The big line is the equivalent of a -= but with events
            foreach (var socket in _speciesItemSockets)
            {
                socket.OnSocketablePlaced = (OWItemSocket.SocketEvent)Delegate.Remove(socket.OnSocketablePlaced, new OWItemSocket.SocketEvent(OnSocketFilled));
            }
        }

        private void OnSocketFilled(OWItem item)
        {
            if (_isDoorClosed) // No need to check again if the door has alread swung open
            {
                CheckActivation();
            }
        }

        private void CheckActivation()
        {
            if (AreAllSocketsCorrect())
            {
                StartCoroutine(PlayAnim());
                _isDoorClosed = false;
            }
        }

        private IEnumerator PlayAnim()
        {
            _doorAnim.Play("DOOR", 0);
            yield return new WaitForSeconds(1f);
            _audio.Play(); // Audio plays after the door slams on the ground
            Locator.GetShipLogManager().RevealFact("SHIP_DOOR_E");
        }
    }
}