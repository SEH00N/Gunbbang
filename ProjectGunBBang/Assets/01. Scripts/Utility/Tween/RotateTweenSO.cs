using DG.Tweening;
using UnityEngine;

namespace GB.Tweens
{
    [CreateAssetMenu(menuName = "SO/Tween/RotateTween")]
    public class RotateTweenSO : TweenSO
    {
        [Space(15f)]
        [SerializeField] float endValue = 1f;

        protected override void OnTween(Sequence sequence)
        {
            TweenParam param;
            Tween tween;

            for(int i = 0; i < tweenParams.Count; ++i)
            {
                param = GetParam(i);
                Vector3 value = body.localEulerAngles + (Vector3.forward * param.Value);
                tween = body.DOLocalRotate(value, param.Duration).SetDelay(param.Delay).SetEase(param.Ease);
                sequence.Append(tween);
            }
        }

        protected override void HandleTweenCompleted()
        {
            base.HandleTweenCompleted();
            Vector3 value = body.localEulerAngles + (Vector3.forward * endValue);
            body.localEulerAngles = value;
        }

        protected override void HandleTweenForceKilled()
        {
            base.HandleTweenForceKilled();
            Vector3 value = body.localEulerAngles + (Vector3.forward * endValue);
            body.localEulerAngles = value;
        }
    }
}
