using System;
using TMPro;
using UnityEngine;

namespace Letter
{
    public interface ILetterView
    {
        public RectTransform RectTransform { get; }
        public TMP_Text Letter { get; }

        public event Action ObjectDestroyed;

        public void SetViewActive(bool isActive);
    }
}