using System;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace MainScreen
{
    public class ScreenPresenter : IDisposable
    {
        private IScreenView _view;
        private ScreenModel _model;

        public ScreenPresenter(IScreenView view, ScreenModel model)
        {
            _view = view;
            _model = model;
        }

        public ScreenPresenter Init()
        {
            _view.GenerateButtonPressed += GenerateField;
            _view.ShuffleButtonPressed += ShuffleLetters;
            _view.ObjectDestroyed += Dispose;
            return this;
        }

        private void GenerateField(string width, string height)
        {
            if (int.TryParse(width, out var parsedWidth) && int.TryParse(height, out var parsedHeight))
            {
                var sizeDelta = _view.Panel.sizeDelta;
                var cellWidth = sizeDelta.x / parsedWidth;
                var cellHeight = sizeDelta.y / parsedHeight;

                if (_model.SetSizes(parsedWidth, parsedHeight, cellWidth, cellHeight))
                {
                    _model.GenerateCells();
                    _view.SetInfoPanelActive(false);
                }
                else
                {
                    _view.SetInfoPanelActive(true);
                }
            }
            else
            {
                _view.SetInfoPanelActive(true);
            }
        }

        private async void ShuffleLetters()
        {
            if (_model.IsLettersEmpty)
            {
                return;
            }
            
            _view.SetInputActive(false);
            _model.ShuffleLetters();
            var token = _model.Source?.Token ?? _model.CreateCancellationTokenSource().Token;
            await StartTimer(token);
            _view?.SetInputActive(true);
        }

        private async Task StartTimer(CancellationToken token)
        {
            var shuffleDuration = ScreenModel.ShuffleDuration;
            var startTime = Time.time;

            while (Time.time <= startTime + shuffleDuration)
            {
                if (token.IsCancellationRequested)
                {
                    return;
                }

                await Task.Yield();
            }
        }

        public void Dispose()
        {
            _view.GenerateButtonPressed -= GenerateField;
            _view.ShuffleButtonPressed -= ShuffleLetters;
            _view.ObjectDestroyed -= Dispose;
            _model.Source?.Cancel();
            _model.DisposeSource();
            _model = default;
            _view = default;
        }
    }
}