using R3;
using R3.Triggers;
using UnityEngine;
using UnityEngine.Assertions;

namespace ShirayuriMeshibe
{
    [RequireComponent(typeof(Camera))]
    public sealed class CameraRaycaster : MonoBehaviour
    {
        private Camera _camera;
        private RenderTexture _renderTexture;
        private bool _isClicked = false;
        private Vector3 _clickedPosition = Vector3.zero;

        private ReactiveProperty<string> _propertyStatus = new ReactiveProperty<string>();
        public ReadOnlyReactiveProperty<string> Status => _propertyStatus;

        private void Start()
        {
            _camera = GetComponent<Camera>();
            Assert.IsNotNull(_camera, $"Failed to get camera component.");
            _renderTexture = _camera.targetTexture;
            Assert.IsNotNull(_renderTexture, $"Failed to get camera render texture.");

            this.FixedUpdateAsObservable()
            .Subscribe(_ =>
            {
                if(_isClicked)
                {
                    _isClicked = false;
                    var ray = _camera.ScreenPointToRay(_clickedPosition);
                    if (Physics.Raycast(ray, out var raycastHit))
                    {
                        _propertyStatus.Value = $"Hit. {raycastHit.transform.name}";
                    }
                    else
                        _propertyStatus.Value = string.Empty;
                }
            }).AddTo(this);
        }
        public void SetClickPosition(Vector2 size, Vector3 screenPoint)
        {
            _isClicked = true;
            screenPoint.x *= _renderTexture.width / size.x;
            screenPoint.y *= _renderTexture.height / size.y;
            _clickedPosition = screenPoint;
        }
    }
}
