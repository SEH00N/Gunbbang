using GB.Attributes;
using GB.Tweens;
using UnityEngine;
using UnityEngine.EventSystems;

namespace GB.UI
{
    public class UIElement : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] bool ownBody = true;
        [SerializeField, ConditionalField("ownBody", false)] Transform body = null;

        [SerializeField] TweenOptOption displayTween = null;
        [SerializeField] TweenOptOption hoverTween = null;

        public RectTransform RectTransform => transform as RectTransform;

        protected virtual void Awake()
        {
            if(ownBody == true)
                body = transform;

            displayTween?.Init(body);
            hoverTween?.Init(body);
        }

        public virtual void Display(bool active = true)
        {
            if (active == true)
                gameObject.SetActive(true);

            if (displayTween == null)
                return;

            displayTween.ClearTween();
            displayTween.PlayPositiveTween();
        }

        public virtual void Hide(bool active = false)
        {
            if(displayTween == null)
            {
                if(active == false)
                    gameObject.SetActive(false);
                return;
            }

            displayTween.ClearTween();
            displayTween.PlayNegativeTween(() => {
                if(active == false)
                    gameObject.SetActive(false);
            });
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if(hoverTween == null)
                return;

            hoverTween.ClearTween();
            hoverTween.PlayPositiveTween();
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (hoverTween == null)
                return;

            hoverTween.ClearTween();
            hoverTween.PlayNegativeTween();
        }
    }
}
