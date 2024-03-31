using R3;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace ShirayuriMeshibe
{
    public sealed class ColorPresenter : MonoBehaviour
    {
        [SerializeField] private ColorModel _model;
        [SerializeField] private ColorSlider _colorSlider;
        [SerializeField] private TextMeshProUGUI _text;
        [SerializeField] private Image _imageColorPreview;

        private void Start()
        {
            // Model -> View
            _model.Color.Subscribe(x =>
            {
                _text.text = x.ToString();
                _colorSlider.Color.Value = x;
                _imageColorPreview.color = x;
            })
            .AddTo(this);

            // View -> Model
            _colorSlider.Color.Subscribe(x =>
            {
                _model.Color.Value = x;
            })
            .AddTo(this);
        }
    }
}
