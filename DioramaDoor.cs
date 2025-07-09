using NewHorizons.Utility;
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
        public bool areAllItemsPresent;
        private bool hasOpened;

        private void Start()
        {
            hasOpened = false;
        }

        private bool AllSocketsFilled()
        {
            int i = 0;
            foreach (var socket in customItemSockets)
            {
                if (socket.isActive)
                {
                    i++;
                }
            }

            if (strangerItemSocket.IsSocketOccupied())
            {
                i++;
            }

            if (i > customItemSockets.Length)
            {
                return true;
            } else
            {
                return false;
            }
        }

        private void Update()
        {
            if (AllSocketsFilled() && !hasOpened)
            {
                hasOpened = true;
                StartCoroutine(PlayAnim());   
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