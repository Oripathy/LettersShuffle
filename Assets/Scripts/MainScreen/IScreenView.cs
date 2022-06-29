using System;
using UnityEngine;

namespace MainScreen
{
    public interface IScreenView
    {
        public RectTransform Panel { get; }

        public event Action<string, string> GenerateButtonPressed;
        public event Action ShuffleButtonPressed;
        public event Action ObjectDestroyed;

        public void SetInfoPanelActive(bool isActive);
        public void SetInputActive(bool isActive);
    }
}