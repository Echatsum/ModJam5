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

        private void Update()
        {
            if (AreAllSocketsCorrect())
            {
                StartCoroutine(PlayAnim());

                this.enabled = false; // We don't need to update anymore, since this door won't change after opening
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