using R3;
using R3.Triggers;
using UnityEngine;
using UnityEngine.Assertions;

namespace ShirayuriMeshibe
{
    [RequireComponent(typeof(Camera))]
    public sealed class CameraRaycaster : MonoBehaviour
    {
        [SerializeField, Range(0f, 0.1f)] private float _dragSpeed = 0.03f;

        private Camera _camera;
        private RenderTexture _renderTexture;
        private bool _isPointerDown = false;
        private Vector3 _pointerDownPosition = Vector3.zero;
        private bool _isDragging = false;
        private Vector3 _lastMousePosition = Vector3.zero;
        private Vector3 _dragPosition = Vector3.zero;
        private Rigidbody _rigidbody = null;

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
                if(_isPointerDown)
                {
                    _isPointerDown = false;
                    var ray = _camera.ScreenPointToRay(_pointerDownPosition);
                    if (Physics.Raycast(ray, out var raycastHit))
                    {
                        _propertyStatus.Value = $"Hit. {raycastHit.transform.name}";
                        _rigidbody = raycastHit.transform.GetComponent<Rigidbody>();

                        if(_rigidbody != null)
                            _rigidbody.isKinematic = false;
                    }
                    else
                        _propertyStatus.Value = string.Empty;
                }
                if(_isDragging && _rigidbody != null)
                {
                    var delta = _dragPosition - _lastMousePosition;
                    _propertyStatus.Value = $"Dragging. {_rigidbody.name}\ndelta:{delta}";
                    var z = delta.y * _dragSpeed;
                    var screenPoint1 = _camera.WorldToScreenPoint(_rigidbody.position);
                    var screenPoint2 = new Vector3(_dragPosition.x, screenPoint1.y, screenPoint1.z);
                    var position2 = _camera.ScreenToWorldPoint(screenPoint2);
                    position2.z += z;
                    _rigidbody.MovePosition(position2);
                    _lastMousePosition = _dragPosition;
                }
            }).AddTo(this);
        }
        public void SetPointerDownPosition(Vector2 size, Vector3 screenPoint)
        {
            _isPointerDown = true;
            screenPoint.x *= _renderTexture.width / size.x;
            screenPoint.y *= _renderTexture.height / size.y;
            _pointerDownPosition = screenPoint;
            _lastMousePosition = screenPoint;
        }
        public void SetDragPosition(Vector2 size, Vector3 screenPoint)
        {
            _isDragging = true;
            screenPoint.x *= _renderTexture.width / size.x;
            screenPoint.y *= _renderTexture.height / size.y;
            _dragPosition = screenPoint;
        }
        public void PointerUp()
        {
            if(_rigidbody != null)
            {
                _rigidbody.isKinematic = true;
                _rigidbody = null;
            }
            _isDragging = false;
            _propertyStatus.Value = string.Empty;
        }
    }
}
