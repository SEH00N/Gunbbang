using DG.Tweening;
using UnityEngine;

namespace GB.Tweens
{
    [CreateAssetMenu(menuName = "SO/Tween/SlideTween")]
    public partial class SlideTweenSO : TweenSO
    {
        [Space(15f)]
        [SerializeField] PivotType pivotType = 0;
        [SerializeField] SlideType slideType = 0;
        [SerializeField] float endValue = 0f;

        private RectTransform rectBody = null;

        public override TweenSO CreateInstance(Transform body)
        {
            TweenSO instance = base.CreateInstance(body);
            if (pivotType == PivotType.AnchoredPosition)
                (instance as SlideTweenSO).rectBody = body as RectTransform;

            return instance;
        }

        protected override void OnTween(Sequence sequence)
        {
            TweenParam param;
            Tween tween = null;

            for (int i = 0; i < tweenParams.Count; ++i)
            {
                param = GetParam(i);
                switch(pivotType)
                {
                    case PivotType.LocalPosition:
                        tween = MoveLocal(param);
                        break;
                    case PivotType.AnchoredPosition:
                        tween = MoveAnchored(param);
                        break;
                }

                sequence.Append(tween);
            }
        }

        private Tween MoveAnchored(TweenParam param)
        {
            Tween tween = null;
            switch (slideType)
            {
                case SlideType.Vertical:
                    tween = rectBody.DOAnchorPosY(param.Value, param.Duration).SetDelay(param.Delay).SetEase(param.Ease);
                    break;
                case SlideType.Horizontal:
                    tween = rectBody.DOAnchorPosX(param.Value, param.Duration).SetDelay(param.Delay).SetEase(param.Ease);
                    break;
            }

            return tween;
        }

        private Tween MoveLocal(TweenParam param)
        {
            Tween tween = null;
            switch (slideType)
            {
                case SlideType.Vertical:
                    tween = body.DOLocalMoveY(param.Value, param.Duration).SetDelay(param.Delay).SetEase(param.Ease);
                    break;
                case SlideType.Horizontal:
                    tween = body.DOLocalMoveX(param.Value, param.Duration).SetDelay(param.Delay).SetEase(param.Ease);
                    break;
            }

            return tween;
        }

        protected override void HandleTweenCompleted()
        {
            base.HandleTweenCompleted();
            ApplyEndValue();
        }

        protected override void HandleTweenForceKilled()
        {
            base.HandleTweenForceKilled();
            ApplyEndValue();
        }

        private void ApplyEndValue()
        {
            switch (pivotType)
            {
                case PivotType.LocalPosition:
                    body.localPosition = GetEndValue(body.localPosition);
                    break;
                case PivotType.AnchoredPosition:
                    rectBody.anchoredPosition = GetEndValue(rectBody.anchoredPosition);
                    break;
            }
        }

        private Vector3 GetEndValue(Vector3 origin)
        {
            switch (slideType)
            {
                case SlideType.Vertical:
                    origin.y = endValue;
                    break;
                case SlideType.Horizontal:
                    origin.x = endValue;
                    break;
            }

            return origin;
        }
    }
}
