using NewHorizons.Utility;
using OWML.Utils;
using UnityEngine;

namespace FifthModJam
{
    public class QuantumPuzzle : MonoBehaviour
    {
        [SerializeField]
        private CustomItemSocket[] customItemSockets = new CustomItemSocket[2];
        [SerializeField]
        private GameObject[] quantumGeysers = new GameObject[4];
        private bool hasCompletedQuantumPuzzle;

        private void Start()
        {
            hasCompletedQuantumPuzzle = false;
            SwapGeyser(0);
        }

        public void SwapGeyser(int index)
        {
            for (int i = 0; i < quantumGeysers.Length; i++)
            {
                if (i != index)
                {
                    quantumGeysers[i].SetActive(false);
                } else
                {
                    quantumGeysers[i].SetActive(true);
                }
            }
        }

        private bool AreAllSocketsCorrect()
        {
            for (int i = 0; i < customItemSockets.Length; i++)
            {
                if (!customItemSockets[i].IsSocketOccupied())
                {
                    return false;
                }
            }

            // If we haven't found any wrong socket, then they're all filled correctly
            return true;
        }

        private bool AreAllSocketsEmpty()
        {
            for (int i = 0; i < customItemSockets.Length; i++)
            {
                if (customItemSockets[i].IsSocketOccupied())
                {
                    return false;
                }
            }

            // If we haven't found any wrong socket, then they're all filled correctly
            return true;
        }

        private void Update()
        {
            if (AreAllSocketsEmpty())
            {
                SwapGeyser(0);
            }
            else if (AreAllSocketsCorrect())
            {
                SwapGeyser(3);
            }
            else
            {
                for (int i = 0; i < customItemSockets.Length; i++)
                {
                    if (customItemSockets[i].IsSocketOccupied())
                    {
                        if (i == 0)
                        {
                            SwapGeyser(2);
                        } else if (i == 1)
                        {
                            SwapGeyser(1);
                        }
                    }
                }
            }
        }
    }
}