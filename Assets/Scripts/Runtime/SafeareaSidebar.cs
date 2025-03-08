using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Kunnymann.Utility
{
    /// <summary>
    /// UI component that automatically creates a TopBar according to SafeArea specifications
    /// </summary>
    public class SafeareaSidebar : MonoBehaviour
    {
        /// <summary>
        /// Sidebar position
        /// </summary>
        [SerializeField] private SafeareaSidebarPosition _position;

        /// <summary>
        /// Canvas scaler of parent (Canvas)
        /// </summary>
        private CanvasScaler _rootReference;
        /// <summary>
        /// Sidebar's RectTransform
        /// </summary>
        private RectTransform _rectTransform;

        /// <summary>
        /// Dynamically controls the size of sidebar transform by calculating the ratio to fill the exception area of SafeArea
        /// </summary>
        private void Awake()
        {
            _rootReference = transform.root.GetComponent<CanvasScaler>();
            _rectTransform = GetComponent<RectTransform>();

            switch (_position)
            {
                case SafeareaSidebarPosition.Top:
                    SetTopSide();
                    break;
                case SafeareaSidebarPosition.Bottom:
                    SetBottomSide();
                    break;
                default:
                    throw new NotImplementedException();
            }
        }

        /// <summary>
        /// Adjusts the top side of the UI element to respect the safe area.
        /// This method:
        /// 1. Calculates the height of the area excluded from the safe area.
        /// 2. Creates a new content object to hold child elements.
        /// 3. Moves existing child objects to the new content object.
        /// 4. Adjusts the position and size of child elements.
        /// 5. Modifies the RectTransform of the current object to fit the safe area.
        /// </summary>
        private void SetTopSide()
        {
            float excludedSafeAreaPixelHeight = ((Screen.height - Screen.safeArea.max.y) / Screen.height) * _rootReference.referenceResolution.y;

            List<Transform> children = new List<Transform>();
            for (int index = 0; index < transform.childCount; index++)
            {
                children.Add(transform.GetChild(index));
            }

            GameObject contentObject = new GameObject("Content");
            contentObject.transform.SetParent(transform);
            contentObject.transform.localScale = Vector3.one;

            RectTransform contentRect = contentObject.AddComponent<RectTransform>();
            contentRect.anchoredPosition = new Vector2(0, 0);
            contentRect.anchorMin = new Vector2(0, 0);
            contentRect.anchorMax = new Vector2(1, 1);
            contentRect.pivot = new Vector2(0.5f, 0.5f);
            contentRect.sizeDelta = new Vector2(0, 0);
            
            foreach (Transform child in children)
            {
                child.SetParent(contentObject.transform);
                RectTransform childRect = child.GetComponent<RectTransform>();
                childRect.position = childRect.position - 2 * Vector3.up * excludedSafeAreaPixelHeight;
            }

            _rectTransform.sizeDelta = new Vector2(_rectTransform.sizeDelta.x, _rectTransform.sizeDelta.y + excludedSafeAreaPixelHeight);
        }

        /// <summary>
        /// Adjusts the bottom side of the UI element to respect the safe area.
        /// This method:
        /// 1. Calculates the height of the area excluded from the safe area.
        /// 2. Creates a new content object to hold child elements.
        /// 3. Moves existing child objects to the new content object.
        /// 4. Adjusts the position and size of child elements.
        /// 5. Modifies the RectTransform of the current object to fit the safe area.
        private void SetBottomSide()
        {
            float excludedSafeAreaPixelHeight = (Screen.safeArea.min.y / Screen.height) * _rootReference.referenceResolution.y;

            List<Transform> children = new List<Transform>();
            for (int index = 0; index < transform.childCount; index++)
            {
                children.Add(transform.GetChild(index));
            }

            GameObject contentObject = new GameObject("Content");
            contentObject.transform.SetParent(transform);
            contentObject.transform.localScale = Vector3.one;

            RectTransform contentRect = contentObject.AddComponent<RectTransform>();
            contentRect.anchoredPosition = new Vector2(0, 0);
            contentRect.anchorMin = new Vector2(0, 0);
            contentRect.anchorMax = new Vector2(1, 1);
            contentRect.pivot = new Vector2(0.5f, 0.5f);
            contentRect.sizeDelta = new Vector2(0, 0);

            foreach (Transform child in children)
            {
                child.SetParent(contentObject.transform);
                RectTransform childRect = child.GetComponent<RectTransform>();
                childRect.position = childRect.position + 2 * Vector3.up * excludedSafeAreaPixelHeight;
            }

            _rectTransform.sizeDelta = new Vector2(_rectTransform.sizeDelta.x, _rectTransform.sizeDelta.y + excludedSafeAreaPixelHeight);
        }
    }
}