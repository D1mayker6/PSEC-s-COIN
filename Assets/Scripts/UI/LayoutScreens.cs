using UnityEngine;

namespace UI
{
    public class LayoutScreens : MonoBehaviour
    {
        [SerializeField] private RectTransform _container;
        private float _screenWidth;

        private void Start()
        {
            _screenWidth = ((RectTransform)_container.parent).rect.width;
            LayoutChildren();
        }

        private void OnRectTransformDimensionsChange()
        {
            _screenWidth = ((RectTransform)_container.parent).rect.width;
            LayoutChildren();
        }

        private void LayoutChildren()
        {
            int count = _container.childCount;
            for (int i = 0; i < count; i++)
            {
                RectTransform child = _container.GetChild(i) as RectTransform;
                if (child == null) continue;
                child.anchorMin = new Vector2(0f, 0f);
                child.anchorMax = new Vector2(1f, 1f);
                child.pivot = new Vector2(0.5f, 0.5f);
                child.sizeDelta = Vector2.zero;
                child.anchoredPosition = new Vector2(i * _screenWidth, 0f);
            }
        }
    }
}