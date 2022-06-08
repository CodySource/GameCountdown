using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Events;
using CodySource;

namespace CodySource
{
    /// <summary>
    /// A simple countdown clock which can have custom text values for each available value
    /// </summary>
    public class GameCountdown : MonoBehaviour
    {

        #region PROPERTIES

        /// <summary>
        /// The countdown text
        /// </summary>
        [SerializeField] private TMP_Text _text = null;

        /// <summary>
        /// The max countdown time
        /// </summary>
        [SerializeField] private float _time = 3f;

        [Header("ANIMATION")]
        [SerializeField] private List<CountdownValue> _values = new List<CountdownValue>();
        [SerializeField] private bool _animateTextSize = true;
        [SerializeField] private int _minSize = 150;
        [SerializeField] private int _maxSize = 300;

        /// <summary>
        /// Triggered whenever the rounded int value of the countdown changes (can be used for audio / visual cues)
        /// </summary>
        public UnityEvent<int> onCountdownValueChanged = new UnityEvent<int>();

        /// <summary>
        /// Triggered whenever the countdown is completed
        /// </summary>
        public UnityEvent onCountdownComplete = new UnityEvent();

        /// <summary>
        /// The current countdown value
        /// </summary>
        private float _current = 0f;

        #endregion

        #region PUBLIC METHODS

        /// <summary>
        /// Begins the countdown
        /// </summary>
        public void Begin() => _current = _time;

        #endregion

        #region PRIVATE METHODS

        /// <summary>
        /// Runs the countdown operations
        /// </summary>
        private void Update()
        {
            // Breakout if is paused
            if (GamePause.isPaused) return;

            // Breakout if timer is complete
            if (_current <= 0f) return;

            // Update timer
            _current -= Time.deltaTime;
            if (_current <= 0f) onCountdownComplete?.Invoke();

            //  Update Text
            if (_text == null) return;
            int _newValue = Mathf.CeilToInt(_current);

            //  Animate the text / replace the text with text value replacements if they are available
            if (_animateTextSize) _text.fontSize = Mathf.Lerp(_minSize, _maxSize, 1f - (_newValue - _current));
            string _newText = (_values.Count > _newValue) ? _values[_newValue].text : _newValue.ToString();
            _text.fontSize = (_values.Count > _newValue && _values[_newValue].fontSize > 0f) ? _values[_newValue].fontSize : _text.fontSize;
            _text.text = _newText;

            //  Run audio cues if desired
            if (_newText != _text.text) onCountdownValueChanged?.Invoke(_newValue);
        }

        #endregion

        #region PUBLIC STRUCTS

        /// <summary>
        /// A countdown text option that replaces the simple integer numbers in the countdown
        /// </summary>
        [System.Serializable]
        public struct CountdownValue
        {
            public string text;
            public float fontSize;
        }

        #endregion

    }
}