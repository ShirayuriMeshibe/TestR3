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
                _rawImage.OnPointerDownAsObservable().Subscribe(e =>
                {
                    var screePoint = rectTransform.InverseTransformPoint(e.position);
                    _text.text = $"[Down] screePoint:{screePoint}\ne:{e.position}, {e.pressPosition}";
                    _cameraRaycaster.SetPointerDownPosition(rectTransform.sizeDelta, screePoint);
                }).AddTo(this);
                _rawImage.OnDragAsObservable().Subscribe(e =>
                {
                    var screePoint = rectTransform.InverseTransformPoint(e.position);
                    if(RectTransformUtility.RectangleContainsScreenPoint(rectTransform, e.position))
                    {
                        _text.text = $"[Drag Inside] screePoint:{screePoint}\ne:{e.position}, {e.pressPosition}";
                        _cameraRaycaster.SetDragPosition(rectTransform.sizeDelta, screePoint);
                    }
                    else
                        _text.text = $"[Drag Outside] screePoint:{screePoint}\ne:{e.position}, {e.pressPosition}";
                });
                _rawImage.OnPointerUpAsObservable().Subscribe(e =>
                {
                    var screePoint = rectTransform.InverseTransformPoint(e.position);
                    _text.text = $"[Up] screePoint:{screePoint}\ne:{e.position}, {e.pressPosition}";
                    _cameraRaycaster.PointerUp();
                }).AddTo(this);
            }
            _cameraRaycaster.Status.Subscribe(status => _textStatus.text = status).AddTo(this);
        }
    }
}
