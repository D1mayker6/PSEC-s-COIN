using UnityEngine;
using UnityEngine.EventSystems;

namespace UI
{
    [RequireComponent(typeof(RectTransform))]
    public class SwipeController : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        [Header("References")]
        [SerializeField] private RectTransform _container; 
        [Header("Screens")]
        [SerializeField] private int _screensCount = 3;
        [Header("Snap / Swipe tuning")]
        [SerializeField] private float _snapSpeed = 10f;           
        [SerializeField] private float _swipeThreshold = 0.18f;    
        [SerializeField] private float _velocityThreshold = 900f; 
        [Header("Behavior")]
        [SerializeField] private int _defaultScreenId = 1;         
        [SerializeField] private bool _useSave = false;            

        private int _currentIndex;
        private float _screenWidth;
        private bool _isDragging;
        private float _releaseVelocity;
        private Vector2 _velocity;
        private bool _isSnapping;

        public int ContainerChildCount => _container != null ? _container.childCount : 0;
        public int CurrentIndex => _currentIndex;

        private void Start()
        {
            if (_container == null)
            {
                Debug.LogError("SwipeController: _container is not assigned in inspector.");
                return;
            }

            if (_container.parent == null)
            {
                Debug.LogError("SwipeController: _container has no parent RectTransform.");
                return;
            }

            _screenWidth = ((RectTransform)_container.parent).rect.width;

            int startIndex = GetIndexForId(_defaultScreenId);
            _currentIndex = Mathf.Clamp(startIndex, 0, Mathf.Max(0, _screensCount - 1));
            SetContainerToIndexImmediate(_currentIndex);
            UpdatePanelsRaycast(); 
        }

        private void Update()
        {
            if (!_isDragging && _isSnapping)
            {
                Vector2 target = GetAnchoredPositionForIndex(_currentIndex);
                _container.anchoredPosition = Vector2.SmoothDamp(_container.anchoredPosition, target, ref _velocity, 1f / _snapSpeed, float.MaxValue, Time.unscaledDeltaTime);
                if (Vector2.SqrMagnitude(_container.anchoredPosition - target) < 0.25f)
                {
                    _container.anchoredPosition = target;
                    _isSnapping = false;
                    UpdatePanelsRaycast();
                }
            }
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            if (_container == null) return;
            _isDragging = true;
            _releaseVelocity = 0f;
            _isSnapping = false;
            SetAllPanelsBlocksRaycasts(true);
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (_container == null) return;
            Vector2 delta = eventData.position - eventData.pressPosition;
            Vector2 newPos = new Vector2(_container.anchoredPosition.x + delta.x, _container.anchoredPosition.y);
            newPos = (Vector2)_container.anchoredPosition + new Vector2(eventData.delta.x, 0f);
            _container.anchoredPosition = newPos;
            _releaseVelocity = eventData.delta.x / Mathf.Max(Time.unscaledDeltaTime, 0.0001f);
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            if (_container == null) return;
            _isDragging = false;

            float deltaX = _container.anchoredPosition.x - GetAnchoredPositionForIndex(_currentIndex).x;
            float normalizedDelta = -deltaX / _screenWidth;

            if (Mathf.Abs(normalizedDelta) > _swipeThreshold || Mathf.Abs(_releaseVelocity) > _velocityThreshold)
            {
                if (normalizedDelta > 0f) MoveToIndex(_currentIndex + 1);
                else MoveToIndex(_currentIndex - 1);
            }
            else
            {
                MoveToIndex(_currentIndex);
            }
        }


        public void MoveToIndex(int index)
        {
            int clamped = Mathf.Clamp(index, 0, Mathf.Max(0, _screensCount - 1));
            _currentIndex = clamped;
            _isSnapping = true;
            UpdatePanelsRaycast(); 

        }


        public void MoveToId(int id)
        {
            int index = GetIndexForId(id);
            MoveToIndex(index);
        }


        public ScreenId GetChildScreenId(int childIndex)
        {
            if (_container == null) return null;
            if (childIndex < 0 || childIndex >= _container.childCount) return null;
            return _container.GetChild(childIndex).GetComponent<ScreenId>();
        }


        public int GetIndexForId(int id)
        {
            if (_container == null) return 0;
            for (int i = 0; i < _container.childCount; i++)
            {
                var sid = _container.GetChild(i).GetComponent<ScreenId>();
                if (sid != null && sid.Id == id) return i;
            }
            return 0;
        }


        public int GetIndexForIdPublic(int id)
        {
            if (_container == null) return -1;
            for (int i = 0; i < _container.childCount; i++)
            {
                var sid = _container.GetChild(i).GetComponent<ScreenId>();
                if (sid != null && sid.Id == id) return i;
            }
            return -1;
        }

        private Vector2 GetAnchoredPositionForIndex(int index)
        {
            return new Vector2(-index * _screenWidth, 0f);
        }

        public void SetContainerToIndexImmediate(int index)
        {
            if (_container == null) return;
            _container.anchoredPosition = GetAnchoredPositionForIndex(index);
        }

        private void OnRectTransformDimensionsChange()
        {
            if (_container == null) return;
            _screenWidth = ((RectTransform)_container.parent).rect.width;
            SetContainerToIndexImmediate(_currentIndex);
        }
    

        private void UpdatePanelsRaycast()
        {

            SetAllPanelsBlocksRaycasts(false);
            var target = GetChildCanvasGroup(_currentIndex);
            if (target != null) target.blocksRaycasts = true;
        }

        private void SetAllPanelsBlocksRaycasts(bool value)
        {
            if (_container == null) return;
            for (int i = 0; i < _container.childCount; i++)
            {
                var cg = GetChildCanvasGroup(i);
                if (cg != null) cg.blocksRaycasts = value;
            }
        }

        private CanvasGroup GetChildCanvasGroup(int childIndex)
        {
            if (_container == null) return null;
            if (childIndex < 0 || childIndex >= _container.childCount) return null;
            return _container.GetChild(childIndex).GetComponent<CanvasGroup>();
        }
    }
}
