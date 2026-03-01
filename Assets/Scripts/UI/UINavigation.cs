using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class UINavigation : MonoBehaviour
    {
        [SerializeField] private Button[] _navButtons;
        [SerializeField] private int[] _targetIds;
        [SerializeField] private SwipeController _swipeController;

        private void Start()
        {
            for (int i = 0; i < _navButtons.Length; i++)
            {
                int id = _targetIds != null && _targetIds.Length > i ? _targetIds[i] : i;
                _navButtons[i].onClick.RemoveAllListeners();
                _navButtons[i].onClick.AddListener(() => OnNavButtonClicked(id));
            }
            UpdateButtonsVisual();
        }

        private void OnNavButtonClicked(int id)
        {
            if (_swipeController != null) _swipeController.MoveToId(id);
            UpdateButtonsVisual();
        }

        private void UpdateButtonsVisual()
        {
            int currentIndex = _swipeController != null ? _swipeController.ContainerChildCount > 0 ? _swipeController.GetIndexForId(_swipeControllerDefaultId()) : 0 : 0;
            int visibleIndex = GetVisibleIndex();
            for (int i = 0; i < _navButtons.Length; i++)
            {
                ColorBlock cb = _navButtons[i].colors;
                int id = _targetIds != null && _targetIds.Length > i ? _targetIds[i] : i;
                cb.normalColor = (GetIndexForId(id) == visibleIndex) ? Color.white : Color.gray;
                cb.highlightedColor = Color.white;
                _navButtons[i].colors = cb;
            }
        }

        private int GetVisibleIndex()
        {
            if (_swipeController == null) return 0;
            var prop = _swipeController.GetType().GetProperty("CurrentIndex");
            if (prop != null) return (int)prop.GetValue(_swipeController);
            return 0;
        }

        private int GetIndexForId(int id)
        {
            if (_swipeController == null) return 0;
            return _swipeController.GetIndexForIdPublic(id);
        }

        private int _swipeControllerDefaultId()
        {
            var field = _swipeController.GetType().GetField("_defaultScreenId", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            if (field != null) return (int)field.GetValue(_swipeController);
            return 1;
        }

        private void OnDestroy()
        {
            for (int i = 0; i < _navButtons.Length; i++) _navButtons[i].onClick.RemoveAllListeners();
        }
    }
}
