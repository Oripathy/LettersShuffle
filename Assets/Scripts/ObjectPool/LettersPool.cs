using System.Collections.Generic;
using Letter;
using UnityEngine;

namespace ObjectPool
{
    public class LettersPool : MonoBehaviour
    {
        [SerializeField] private LetterView _letterPrefab;

        private const int PoolCapacity = 144;
        private readonly Stack<LetterModel> _letters = new Stack<LetterModel>(PoolCapacity);

        public void Init(RectTransform parent)
        {
            for (var i = 0; i < PoolCapacity; i++)
            {
                var view = Instantiate(_letterPrefab, parent, false);
                var model = new LetterModel();
                var presenter = new LetterPresenter(view, model);
                presenter.Init();
                model.SetActive(false);
                _letters.Push(model);
            }
        }

        public bool TryReleaseObject(out LetterModel pooledLetter)
        {
            if (_letters.Count != 0)
            {
                pooledLetter = _letters.Pop();
                pooledLetter.SetActive(true);
                return true;
            }

            pooledLetter = null;
            return false;
        }

        public void ReturnToPool(LetterModel pooledLetter)
        {
            _letters.Push(pooledLetter);
            pooledLetter.SetActive(false);
        }

        private void OnDestroy()
        {
            _letters.Clear();
        }
    }
}