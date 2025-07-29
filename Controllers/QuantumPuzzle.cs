using System.Collections.Generic;
using UnityEngine;

namespace FifthModJam
{
    public class QuantumPuzzle : MonoBehaviour
    {
        // Rings of stone where you can place items
        [SerializeField]
        private CustomItemSocket[] _customItemSockets;

        // The quantum geyser parent object
        [SerializeField]
        private GameObject _quantumGeyserParent;
        private SocketedQuantumObject _quantumGeyser;

        // The quantum sockets. first the ones the player can interact with, then the hidden one (meant to free the interactable ones), then the solution one.
        private QuantumSocket[] _quantumSockets;
        private QuantumSocket _quantumSocketHidden;
        private QuantumSocket _quantumSocketSolution;

        protected void VerifyUnityParameters()
        {
            if (_quantumGeyserParent == null)
            {
                FifthModJam.WriteLine("[QuantumPuzzle] quantumGeyser parent object is null", OWML.Common.MessageType.Error);
            }
            if (_customItemSockets == null || _customItemSockets.Length == 0)
            {
                FifthModJam.WriteLine("[QuantumPuzzle] itemSockets array is null or empty", OWML.Common.MessageType.Error);
            }
        }

        private void Start()
        {
            VerifyUnityParameters();

            _quantumGeyser = _quantumGeyserParent?.GetComponentInChildren<SocketedQuantumObject>();
            if (_quantumGeyser == null)
            {
                FifthModJam.WriteLine("[QuantumPuzzle] Could not find quantumGeyser component", OWML.Common.MessageType.Error);
            }
            else if (_quantumGeyser._sockets == null || _quantumGeyser._sockets.Length < 2)
            {
                FifthModJam.WriteLine("[QuantumPuzzle] QuantumGeyser has too few (less than two) sockets", OWML.Common.MessageType.Error);
            }
            else if (_quantumGeyser._sockets.Length-2 != _customItemSockets.Length)
            {
                FifthModJam.WriteLine("[QuantumPuzzle] number of quantumSockets and itemSockets do not match", OWML.Common.MessageType.Error);
            }
            else
            {
                RegisterSockets();
                UpdateGeyser();
            }
        }

        private void RegisterSockets()
        {
            var count = _quantumGeyser._sockets.Length;
            _quantumSockets = new QuantumSocket[count-2]; // [Note the -2 here. the last two are the hidden and solution]
            for (int i = 0; i < count-2; i++)
            {
                _quantumSockets[i] = _quantumGeyser._sockets[i];
            }

            _quantumSocketHidden = _quantumGeyser._sockets[count - 2];
            _quantumSocketSolution = _quantumGeyser._sockets[count - 1];
        }
        private QuantumSocket[] ComputeActiveSockets()
        {
            if (_quantumSockets == null) return new QuantumSocket[0];
            if (_quantumSocketHidden == null) return new QuantumSocket[0];
            if (_quantumSocketSolution == null) return new QuantumSocket[0];

            // Make list of sockets that are not occupied by items
            var list = new List<QuantumSocket>();
            for (int i = 0; i < _customItemSockets.Length; i++)
            {
                if (!(_customItemSockets[i].IsSocketOccupied() || IsSocketOccupiedByOtherStuff(socketIndex: i)))
                {
                    list.Add(_quantumSockets[i]);
                }
            }

            // Case 1: all sockets are filled. The returned list will be the single element of the solution socket.
            if (list.Count == 0)
            {
                Locator.GetShipLogManager().RevealFact("COSMICCURATORS_QUANTUM_GEYSERS_FILLSOCKET2");
                Locator.GetShipLogManager().RevealFact("COSMICCURATORS_GHOST_MATTER_CAVE_GEYSER_R");
                return new QuantumSocket[1] { _quantumSocketSolution };
            }

            // Case 2: some sockets are empty. Add the hidden socket to the list.
            list.Add(_quantumSocketHidden);
            return list.ToArray();
        }

        private bool IsSocketOccupiedByOtherStuff(int socketIndex)
        {
            return false; // Placeholder
        }

        private void Awake()
        {
            if (_customItemSockets != null)
            {
                foreach (var socket in _customItemSockets)
                {
                    socket.OnSocketablePlaced += OnSocketFilled;
                    socket.OnSocketableRemoved += OnSocketRemoved;
                }
            }
        }
        private void OnDestroy()
        {
            if (_customItemSockets != null)
            {
                foreach (var socket in _customItemSockets)
                {
                    socket.OnSocketablePlaced -= OnSocketFilled;
                    socket.OnSocketableRemoved -= OnSocketRemoved;
                }
            }
        }

        private void OnSocketFilled(OWItem item)
        {
            Locator.GetShipLogManager().RevealFact("COSMICCURATORS_QUANTUM_GEYSERS_FILLSOCKET1");
            UpdateGeyser();
        }
        private void OnSocketRemoved(OWItem item)
        {
            UpdateGeyser();
        }

        private void UpdateGeyser()
        {
            var newSockets = ComputeActiveSockets();
            _quantumGeyser.SetQuantumSockets(newSockets);
            if (newSockets.Length == 1)
            {
                _quantumGeyser.MoveToSocket(newSockets[0]); // Immediately move to solution socket (only reason why newSockets would have only one element)
            }
        }
    }
}