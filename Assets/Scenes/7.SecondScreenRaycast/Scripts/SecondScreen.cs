using R3;
using R3.Triggers;
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
        [SerializeField] private TextMeshProUGUI _textStatus = null;
        [SerializeField] private CameraRaycaster _cameraRaycaster = null;

        void Start()
        {
            if(TryGetComponent<RectTransform>(out var rectTransform))
            {
                _rawImage.OnPointerClickAsObservable().Subscribe(e =>
                {
                    var localPosition = rectTransform.InverseTransformPoint(e.position);
                    var screePoint = localPosition;
                    _text.text = $"localPosition:{localPosition}\nscreePoint:{screePoint}\ne:{e.position}, {e.pressPosition}";
                    _cameraRaycaster.SetClickPosition(rectTransform.sizeDelta, screePoint);
                }).AddTo(this);
            }
            _cameraRaycaster.Status.Subscribe(status => _textStatus.text = status).AddTo(this);
        }
    }
}
