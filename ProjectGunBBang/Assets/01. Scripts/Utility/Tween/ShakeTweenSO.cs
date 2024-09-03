using DG.Tweening;
using UnityEngine;

namespace GB.Tweens
{
    [CreateAssetMenu(menuName = "SO/Tween/ShakeTween")]
    public class ShakeTweenSO : TweenSO
    {
        [Space(15f)]
        [SerializeField] float strength = 1f;
        [SerializeField] int vibrato = 10;

        protected override void OnTween(Sequence sequence)
        {
            Tween tween;
            TweenParam param = GetParam(0);
            tween = body.DOShakePosition(param.Duration, strength, vibrato).SetDelay(param.Delay);
            sequence.Append(tween);
        }
    }
}