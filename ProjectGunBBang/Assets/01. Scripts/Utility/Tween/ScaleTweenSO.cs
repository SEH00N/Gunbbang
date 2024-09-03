using DG.Tweening;
using GB.Extensions;
using UnityEngine;

namespace GB.Tweens
{
    [CreateAssetMenu(menuName = "SO/Tween/ScaleTween")]
    public class ScaleTweenSO : TweenSO
    {
        [Space(15f)]
        [SerializeField] float endValue = 1f;
        [SerializeField] bool keepSign = false;

        protected override void OnTween(Sequence sequence)
        {
            TweenParam param;
            Tween tween;

            for(int i = 0; i < tweenParams.Count; ++i)
            {
                param = GetParam(i);
                Vector3 value = (keepSign ? body.localScale.Sign() : Vector3.one) * param.Value;
                tween = body.DOScale(value, param.Duration).SetDelay(param.Delay).SetEase(param.Ease);
                sequence.Append(tween);
            }
        }

        protected override void HandleTweenCompleted()
        {
            base.HandleTweenCompleted();
            Vector3 value = (keepSign ? body.localScale.Sign() : Vector3.one) * endValue;
            body.localScale = value;
        }

        protected override void HandleTweenForceKilled()
        {
            base.HandleTweenForceKilled();
            Vector3 value = (keepSign ? body.localScale.Sign() : Vector3.one) * endValue;
            body.localScale = value;
        }
    }
}
