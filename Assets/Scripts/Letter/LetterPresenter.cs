using System;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace Letter
{
    public class LetterPresenter : IDisposable
    {
        private ILetterView _view;
        private LetterModel _model;

        public LetterPresenter(ILetterView view, LetterModel model)
        {
            _view = view;
            _model = model;
        }

        public LetterPresenter Init()
        {
            _model.MoveCommandReceived += MoveLetter;
            _model.LetterReceived += OnLetterReceived;
            _model.ActiveStateChanged += OnActiveStateChanged;
            _view.ObjectDestroyed += Dispose;
            return this;
        }

        private void OnLetterReceived(Vector2 position, string letter, float[] size)
        {
            _view.RectTransform.anchoredPosition = position;
            _view.Letter.text = letter;
            _view.Letter.rectTransform.sizeDelta = new Vector2(size[0], size[1]);
            _view.Letter.fontSize = Mathf.Min((int) size[0], (int) size[1]);
        }

        private void OnActiveStateChanged(bool isActive)
        {
            _view.SetViewActive(isActive);
        }
        
        private async void MoveLetter(Vector2 destinationPosition)
        {
            var token = _model.Source?.Token ?? _model.CreateCancellationTokenSource().Token;
            await StartMovement(destinationPosition, token);
        }

        private async Task StartMovement(Vector2 destinationPosition, CancellationToken token)
        {
            var startTime = Time.time;
            var movementTime = _model.MovementTime;
            var initialPosition = _view.RectTransform.anchoredPosition;

            while (Time.time <= startTime + movementTime)
            {
                if (token.IsCancellationRequested)
                {
                    return;
                }
                
                _view.RectTransform.anchoredPosition = Vector2.Lerp(initialPosition, destinationPosition,
                    (Time.time - startTime) / movementTime);
                await Task.Yield();
            }
        }

        public void Dispose()
        {
            _model.MoveCommandReceived -= MoveLetter;
            _model.LetterReceived -= OnLetterReceived;
            _model.ActiveStateChanged -= OnActiveStateChanged;
            _view.ObjectDestroyed -= Dispose;
            _model.Source?.Cancel();
            _model.DisposeSource();
            _model = default;
            _view = default;
        }
    }
}