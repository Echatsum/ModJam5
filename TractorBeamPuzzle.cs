using System;
using UnityEngine;

namespace FifthModJam
{

    public class TractorBeamPuzzle : MonoBehaviour
    {
        [SerializeField]
        private CustomItemSocket customItemSocket;
        [SerializeField]
        private GameObject tractorBeam;

        private void Awake()
        {
            customItemSocket.OnSocketablePlaced = (OWItemSocket.SocketEvent)Delegate.Combine(customItemSocket.OnSocketablePlaced, new OWItemSocket.SocketEvent(OnSocketFilled));
            customItemSocket.OnSocketableRemoved = (OWItemSocket.SocketEvent)Delegate.Combine(customItemSocket.OnSocketableRemoved, new OWItemSocket.SocketEvent(OnSocketRemoved));
        }

        private void Start()
        {
            tractorBeam.SetActive(false);
        }

        private void OnSocketFilled(OWItem item)
        {
            tractorBeam.SetActive(true);
        }

        private void OnSocketRemoved(OWItem item)
        {
            tractorBeam.SetActive(false);
        }
    }
}