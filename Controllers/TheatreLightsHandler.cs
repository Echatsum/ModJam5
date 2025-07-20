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

        private bool _shouldLightsBeOn = true;

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
            if(_socket != null)
            {
                _socket.OnSocketablePlaced = (OWItemSocket.SocketEvent)Delegate.Combine(_socket.OnSocketablePlaced, new OWItemSocket.SocketEvent(OnSocketFilled));
                _socket.OnSocketableRemoved = (OWItemSocket.SocketEvent)Delegate.Combine(_socket.OnSocketableRemoved, new OWItemSocket.SocketEvent(OnSocketRemoved));
            }
        }
        private void OnDestroy()
        {
            if(_socket != null)
            {
                _socket.OnSocketablePlaced = (OWItemSocket.SocketEvent)Delegate.Remove(_socket.OnSocketablePlaced, new OWItemSocket.SocketEvent(OnSocketFilled));
                _socket.OnSocketableRemoved = (OWItemSocket.SocketEvent)Delegate.Remove(_socket.OnSocketableRemoved, new OWItemSocket.SocketEvent(OnSocketRemoved));
            }
        }

        private void OnSocketFilled(OWItem item)
        {
            if (item.GetItemType() == ItemType.Lantern)
            {
                _shouldLightsBeOn = false;
                ToggleLights(turnOn: false); // lantern in = lights OFF
            }
        }
        private void OnSocketRemoved(OWItem item)
        {
            if (item.GetItemType() == ItemType.Lantern)
            {
                _shouldLightsBeOn = true;
                ToggleLights(turnOn: true); // lantern out = lights ON
            }
        }
        private void ToggleLights(bool turnOn)
        {
            foreach (var light in lights)
            {
                light.SetActive(turnOn); // [Note: this is where we would add any gradual fading if we want. Right now this is just a simple SetActive on/off switch]
            }
        }

        public void OnEnterHouse()
        {
            ToggleLights(turnOn: _shouldLightsBeOn);
        }
        public void OnExitHouse()
        {
            ToggleLights(turnOn: true);
        }
    }
}