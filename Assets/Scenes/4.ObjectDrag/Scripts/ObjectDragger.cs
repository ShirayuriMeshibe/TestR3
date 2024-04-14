using R3;
using R3.Triggers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ShirayuriMeshibe
{
    [RequireComponent(typeof(MeshRenderer))]
    public sealed class ObjectDragger : MonoBehaviour
    {
        [SerializeField, Range(0f, 0.1f)] private float _dragSpeed = 0.03f;
        [SerializeField] private MeshRenderer _mesehRenderer = null;
        [SerializeField] private Camera _camera = null;

        private void Start()
        {
            if (gameObject.TryGetComponent<Rigidbody>(out var rigidbody))
            {
                var eventTrigger = gameObject.AddComponent<ObservableEventTrigger>();

                var lastMousePosition = Vector3.zero;
                var position = transform.position;

                eventTrigger.OnPointerDownAsObservable().Subscribe(_ =>
                {
                    lastMousePosition = Input.mousePosition;
                    rigidbody.isKinematic = false;
                }).AddTo(this);
                eventTrigger.OnDragAsObservable().Subscribe(_ =>
                {
                    var delta = Input.mousePosition - lastMousePosition;
                    var z = delta.y * _dragSpeed;
                    var screenPoint = _camera.WorldToScreenPoint(transform.position);
                    var screenPoint2 = new Vector3(Input.mousePosition.x, screenPoint.y, screenPoint.z);
                    var position2 = Camera.main.ScreenToWorldPoint(screenPoint2);
                    position2.z += z;
                    position = position2;
                    //transform.position = position2;
                    lastMousePosition = Input.mousePosition;
                }).AddTo(this);
                eventTrigger.OnPointerUpAsObservable().Subscribe(_ =>
                {
                    rigidbody.isKinematic = true;
                }).AddTo(this);
                this.FixedUpdateAsObservable().Subscribe(_ =>
                {
                    rigidbody.MovePosition(position);
                }).AddTo(this);
            }
            else
                Debug.LogError("Failed get Rigidbody component.");
        }
    }
}
