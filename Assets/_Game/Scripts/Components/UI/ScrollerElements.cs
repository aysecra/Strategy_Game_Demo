using UnityEngine;

namespace StrategyDemo.Component
{
    public class ScrollerElements : MonoBehaviour
    {
        [SerializeField] private float itemSpacing;
        [SerializeField] private float horizontalMargin;
        [SerializeField] private float verticalMargin;
        [SerializeField] private bool horizontal;
        [SerializeField] private bool vertical;
        
        private RectTransform _rectTransform;
        private RectTransform[] _rtChildren;
        private float _width;
        private float _height;
        private float _childWidth;
        private float _childHeight;

        public float ItemSpacing => itemSpacing;
        public float HorizontalMargin => horizontalMargin;
        public float VerticalMargin => verticalMargin;
        public bool Horizontal => horizontal;
        public bool Vertical => vertical;
        public float Width => _width;
        public float Height => _height;
        public float ChildWidth => _childWidth;
        public float ChildHeight => _childHeight;
        

        private void Start()
        {
            _rectTransform = GetComponent<RectTransform>();
            _rtChildren = new RectTransform[_rectTransform.childCount];

            for (int i = 0; i < _rectTransform.childCount; i++)
            {
                _rtChildren[i] = _rectTransform.GetChild(i) as RectTransform;
            }

            _width = _rectTransform.rect.width - (2 * horizontalMargin);

            _height = _rectTransform.rect.height - (2 * verticalMargin);

            _childWidth = _rtChildren[0].rect.width;
            _childHeight = _rtChildren[0].rect.height;

            horizontal = !vertical;
            if (vertical)
                InitializeContentVertical();
            else
                InitializeContentHorizontal();
        }

        private void InitializeContentHorizontal()
        {
            float originX = 0 - (_width * 0.5f);
            float posOffset = _childWidth * 0.5f;
            for (int i = 0; i < _rtChildren.Length; i++)
            {
                Vector2 childPos = _rtChildren[i].localPosition;
                childPos.x = originX + posOffset + i * (_childWidth + itemSpacing);
                _rtChildren[i].localPosition = childPos;
            }
        }

        private void InitializeContentVertical()
        {
            float originY = 0 - (_height * 0.5f);
            float posOffset = _childHeight * 0.5f;
            for (int i = 0; i < _rtChildren.Length; i++)
            {
                Vector2 childPos = _rtChildren[i].localPosition;
                childPos.y = originY + posOffset + i * (_childHeight + itemSpacing);
                _rtChildren[i].localPosition = childPos;
            }
        }
    }
}