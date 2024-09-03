using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace GB.Tweens
{
    [CreateAssetMenu(menuName = "SO/Tween/ColorTween")]
    public class ColorTweenSO : TweenSO
    {
        [SerializeField] Color endValue = Color.white;
        private Graphic target = null;

        public override TweenSO CreateInstance(Transform body)
        {
            ColorTweenSO instance = base.CreateInstance(body) as ColorTweenSO;
            instance.target = body.GetComponent<Graphic>();
           
            return instance;
        }

        protected override void OnTween(Sequence sequence)
        {
            TweenParam param;
            Tween tween;

            for(int i = 0; i < tweenParams.Count; ++i)
            {
                param = GetParam(i);
                tween = target.DOColor(param.Color, param.Duration).SetDelay(param.Delay).SetEase(param.Ease);
                sequence.Append(tween);
            }
        }

        protected override void HandleTweenCompleted()
        {
            base.HandleTweenCompleted();
            target.color = endValue;
        }

        protected override void HandleTweenForceKilled()
        {
            base.HandleTweenForceKilled();
            target.color = endValue;
        }
    }
}
