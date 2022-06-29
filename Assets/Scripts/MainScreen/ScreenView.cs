using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace MainScreen
{
    public class ScreenView : MonoBehaviour, IScreenView
    {
        [SerializeField] private Image _panel;
        [SerializeField] private Button _generateButton;
        [SerializeField] private Button _shuffleButton;
        [SerializeField] private TMP_InputField _width;
        [SerializeField] private TMP_InputField _height;
        [SerializeField] private TMP_Text _infoPanelText;

        public RectTransform Panel => _panel.rectTransform;
        
        public event Action<string, string> GenerateButtonPressed;
        public event Action ShuffleButtonPressed;
        public event Action ObjectDestroyed;

        private void Awake()
        {
            _generateButton.onClick.AddListener(() => GenerateButtonPressed?.Invoke(_width.text, _height.text));
            _shuffleButton.onClick.AddListener(() => ShuffleButtonPressed?.Invoke());
            _infoPanelText.gameObject.SetActive(false);
        }
        
        public void SetInfoPanelActive(bool isActive)
        {
            _infoPanelText.gameObject.SetActive(isActive);
        }

        public void SetInputActive(bool isActive)
        {
            _generateButton.enabled = isActive;
            _shuffleButton.enabled = isActive;
        }

        public void OnDestroy()
        {
            ObjectDestroyed?.Invoke();
        }
    }
}