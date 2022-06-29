using System;
using TMPro;
using UnityEngine;

namespace Letter
{
    public class LetterView : MonoBehaviour, ILetterView
    {
        [SerializeField] private TMP_Text _letter;

        public RectTransform RectTransform => _letter.rectTransform;
        public TMP_Text Letter => _letter;
        
        public event Action ObjectDestroyed;

        public void SetViewActive(bool isActive)
        {
            gameObject.SetActive(isActive);
        }
        
        public void OnDestroy()
        {
            ObjectDestroyed?.Invoke();
        }
    }
}
