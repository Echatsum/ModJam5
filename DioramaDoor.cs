using System.Collections;
using UnityEngine;

namespace FifthModJam
{
    public class DioramaDoor : MonoBehaviour
    {
        [SerializeField]
        private CustomItemSocket[] customItemSockets;
        [SerializeField]
        private DreamLanternSocket strangerItemSocket;
        [SerializeField]
        private Animator doorAnim;
        [SerializeField]
        private OWAudioSource audio;

        private bool AreAllSocketsFilled()
        {
            foreach (var socket in customItemSockets)
            {
                if (!socket.isActive)
                {
                    return false;
                }
            }

            if (!strangerItemSocket.IsSocketOccupied())
            {
                return false;
            }

            // If we haven't found any empty socket, then they're all filled
            return true;
        }

        private void Update()
        {
            if (AreAllSocketsFilled())
            {
                StartCoroutine(PlayAnim());

                this.enabled = false; // We don't need to update anymore, since this door won't change after opening
            }
        }

        private IEnumerator PlayAnim()
        {
            doorAnim.Play("DOOR", 0);
            yield return new WaitForSeconds(0.5f);
            audio.Play();
        }
    }
}