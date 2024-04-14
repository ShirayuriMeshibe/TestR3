using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ShirayuriMeshibe
{
    [RequireComponent(typeof(RectTransform))]
    public sealed class SafeArea : MonoBehaviour
    {
        void Start()
        {
            var rectTransform = GetComponent<RectTransform>();
            var safeArea = Screen.safeArea;

            rectTransform.anchoredPosition = Vector2.zero;
            rectTransform.sizeDelta = Vector2.zero;
            rectTransform.anchorMin = new Vector2(safeArea.xMin / Screen.width, safeArea.yMin / Screen.height);
            rectTransform.anchorMax = new Vector2(safeArea.xMax / Screen.width, safeArea.yMax / Screen.height);
        }
    }
}
