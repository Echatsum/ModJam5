using System;
using UnityEngine;

namespace FifthModJam
{
    public class TheatreLightsHandler : MonoBehaviour
    {
        [SerializeField]
        private GameObject[] lights;
        [SerializeField]
        private SlideProjectorSocket _socket;

        private void VerifyUnityParameters()
        {
            if (lights == null || lights.Length == 0)
            {
                FifthModJam.WriteLine("[TheatreLightsHandler] light array is null or empty", OWML.Common.MessageType.Error);
            }
            if (_socket == null)
            {
                FifthModJam.WriteLine("[TheatreLightsHandler] socket is null", OWML.Common.MessageType.Error);
            }
        }
        private void Start()
        {
            VerifyUnityParameters();
        }

        private void Awake()
        {
            _socket.OnSocketablePlaced = (OWItemSocket.SocketEvent)Delegate.Combine(_socket.OnSocketablePlaced, new OWItemSocket.SocketEvent(OnSocketFilled));
            _socket.OnSocketableRemoved = (OWItemSocket.SocketEvent)Delegate.Combine(_socket.OnSocketableRemoved, new OWItemSocket.SocketEvent(OnSocketRemoved));
        }
        private void OnDestroy()
        {
            _socket.OnSocketablePlaced = (OWItemSocket.SocketEvent)Delegate.Remove(_socket.OnSocketablePlaced, new OWItemSocket.SocketEvent(OnSocketFilled));
            _socket.OnSocketableRemoved = (OWItemSocket.SocketEvent)Delegate.Remove(_socket.OnSocketableRemoved, new OWItemSocket.SocketEvent(OnSocketRemoved));
        }

        private void OnSocketFilled(OWItem item)
        {
            ToggleLights(turnOn: false); // Socket filled = lights OFF
        }
        private void OnSocketRemoved(OWItem item)
        {
            ToggleLights(turnOn: true); // Socket empty = lights ON
        }

        private void ToggleLights(bool turnOn)
        {
            foreach (var light in lights)
            {
                light.SetActive(turnOn);
            }
        }
    }
}