using R3;
using R3.Triggers;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ShirayuriMeshibe
{
    [RequireComponent(typeof(RawImage))]
    public sealed class SecondScreen : MonoBehaviour
    {
        [SerializeField] private RawImage _rawImage = null;
        [SerializeField] private TextMeshProUGUI _text = null;
        [SerializeField] private GameObject _prefab = null;

        void Start()
        {
            if(TryGetComponent<RectTransform>(out var rectTransform))
            {
                var size = rectTransform.sizeDelta;
                Debug.Log($"size:{size}");

                _rawImage.OnPointerClickAsObservable().Subscribe(e =>
                {
                    var localPosition = rectTransform.InverseTransformPoint(e.position);
                    var o = Instantiate(_prefab, rectTransform);
                    o.transform.localPosition = localPosition;
                    o.SetActive(true);

                    var screePoint = localPosition / size;

                    _text.text = $"localPosition:{localPosition}\nscreePoint:{screePoint}\ne:{e.position}, {e.pressPosition}";
                }).AddTo(this);
            }
        }
    }
}
