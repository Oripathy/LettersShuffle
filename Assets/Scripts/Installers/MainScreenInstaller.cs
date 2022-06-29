using MainScreen;
using ObjectPool;
using UnityEngine;

namespace Installers
{
    public class MainScreenInstaller : MonoBehaviour
    {
        [SerializeField] private ScreenView _view;
        [SerializeField] private LettersPool _lettersPool;

        private ScreenPresenter _presenter;
        private ScreenModel _model;

        private void Awake()
        {
            var parentView = _view as IScreenView;
            _lettersPool.Init(parentView.Panel);
            _model = new ScreenModel(_lettersPool);
            _presenter = new ScreenPresenter(_view, _model).Init();
        }

        private void OnDestroy()
        {
            _presenter = default;
            _model = default;
        }
    }
}