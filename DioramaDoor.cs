using System;
using System.Collections;
using UnityEngine;

namespace FifthModJam
{
    public class DioramaDoor : MonoBehaviour
    {
        [SerializeField]
        private CustomItemSocket[] customItemSockets;
        [SerializeField]
        private Animator doorAnim;
        [SerializeField]
        private OWAudioSource audio;

        private bool _isDoorClosed = true;

        private bool AreAllSocketsCorrect()
        {
            foreach (var socket in customItemSockets)
            {
                if (!socket.IsSocketOccupied())
                {
                    return false;
                }
                if (!socket.HasCorrectSpeciesItem())
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
            foreach (var socket in customItemSockets)
            {
                // This big line is the equivalent of a += but with events
                socket.OnSocketablePlaced = (OWItemSocket.SocketEvent)Delegate.Combine(socket.OnSocketablePlaced, new OWItemSocket.SocketEvent(OnSocketFilled));
            }
        }
        private void OnSocketFilled(OWItem item)
        {
            if (_isDoorClosed)
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
            doorAnim.Play("DOOR", 0);
            yield return new WaitForSeconds(1f);
            audio.Play(); // Audio plays after the door slams on the ground
        }
    }
}