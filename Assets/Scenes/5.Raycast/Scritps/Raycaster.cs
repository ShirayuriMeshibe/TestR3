using R3;
using R3.Triggers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ShirayuriMeshibe
{
    [RequireComponent(typeof(Camera))]
    public sealed class Raycaster : MonoBehaviour
    {
        void Start()
        {
            if(TryGetComponent<Camera>(out var camera))
            {
                var mousePosition = Vector3.zero;
                var isMouseDown = false;

                this.UpdateAsObservable()
                .Where(_ => Input.GetMouseButtonDown(0))
                .Subscribe(_ =>
                {
                    isMouseDown = true;
                    mousePosition = Input.mousePosition;
                })
                .AddTo(this);

                this.FixedUpdateAsObservable()
                .Where(_ => isMouseDown)
                .Subscribe(_ =>
                {
                    var ray = camera.ScreenPointToRay(mousePosition);
                    if(Physics.Raycast(ray, out var raycastHit))
                    {
                        Debug.Log($"Hit. {raycastHit.transform.name}");
                    }
                    isMouseDown = false;
                })
                .AddTo(this);
            }
        }
    }
}
