using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Letter;
using ObjectPool;
using UnityEngine;
using Random = UnityEngine.Random;

namespace MainScreen
{
    public class ScreenModel
    {
        public const float ShuffleDuration = 2f;

        private readonly int[] _widthRange = { 3, 12 };
        private readonly int[] _heightRange = { 3, 12 };
        private readonly List<string> _alphabet = new List<string>();
        private LettersPool _lettersPool;
        private List<List<Vector2>> _cells = new List<List<Vector2>>();
        private List<LetterModel> _letters = new List<LetterModel>();
        private int _width;
        private int _height;
        private float _cellWidth;
        private float _cellHeight;

        public CancellationTokenSource Source { get; private set; }
        public bool IsLettersEmpty => _letters.Count == 0;

        public ScreenModel(LettersPool lettersPool)
        {
            _lettersPool = lettersPool;
            
            for (var symbol = 'A'; symbol <= 'Z'; symbol++)
            {
                _alphabet.Add("" + symbol);
            }
        }

        public bool SetSizes(int width, int height, float cellWidth, float cellHeight)
        {
            if (width < _widthRange[0] || width > _widthRange[1] ||
                height < _heightRange[0] || height > _heightRange[1])
            {
                return false;
            }

            _width = width;
            _height = height;
            _cellWidth = cellWidth;
            _cellHeight = cellHeight;
            return true;
        }
        
        public void GenerateCells()
        {
            if (_letters.Count != 0)
            {
                foreach (var letter in _letters)
                {
                    _lettersPool.ReturnToPool(letter);
                }
                _letters.Clear();
            }

            if (_cells.Count != 0)
            {
                _cells.Clear();
            }
            
            var position = Vector2.zero;
            
            for (var i = 0; i < _width; i++)
            {
                _cells.Add(new List<Vector2>());
                
                for (var j = 0; j < _height; j++)
                {
                    _cells[i].Add(position);
                    position.y -= _cellHeight;
                }

                position.x += _cellWidth;
                position.y = 0;
            }

            SetLetters();
        }

        private void SetLetters()
        {
            foreach (var cell in _cells.SelectMany(cellList => cellList))
            {
                if (_lettersPool.TryReleaseObject(out var letter))
                {
                    var randomIndex = Random.Range(0, _alphabet.Count);
                    _letters.Add(letter);
                    letter.SetLetter(cell, _alphabet[randomIndex], new []{ _cellWidth, _cellHeight});
                }
                else
                {
                    throw new Exception("Pool is empty");
                }
            }
        }

        public void ShuffleLetters()
        {
            var positions = _cells.SelectMany(cellList => cellList).ToList();

            foreach (var letter in _letters)
            {
                var randomIndex = Random.Range(0, positions.Count);
                var position = positions[randomIndex];
                letter.MoveLetter(position);
                positions.Remove(position);
            }
        }

        public CancellationTokenSource CreateCancellationTokenSource()
        {
            return Source = new CancellationTokenSource();
        }
        
        public void DisposeSource()
        {
            Source?.Dispose();
            Source = default;
            _letters.Clear();
        }
    }
}