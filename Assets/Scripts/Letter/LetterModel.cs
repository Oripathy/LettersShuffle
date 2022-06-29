using System;
using System.Threading;
using UnityEngine;

namespace Letter
{
    public class LetterModel
    {
        public readonly float MovementTime = 2f;
        public CancellationTokenSource Source { get; private set; }

        public event Action<Vector2> MoveCommandReceived;
        public event Action<Vector2, string, float[]> LetterReceived;
        public event Action<bool> ActiveStateChanged; 

        public void MoveLetter(Vector2 destinationPosition)
        {
            MoveCommandReceived?.Invoke(destinationPosition);
        }

        public CancellationTokenSource CreateCancellationTokenSource()
        {
            return Source = new CancellationTokenSource();
        }

        public void SetLetter(Vector2 position, string letter, float[] size)
        {
            LetterReceived?.Invoke(position, letter, size);
        }

        public void SetActive(bool isActive)
        {
            ActiveStateChanged?.Invoke(isActive);
        }

        public void DisposeSource()
        {
            Source?.Dispose();
            Source = default;
        }
    }
}