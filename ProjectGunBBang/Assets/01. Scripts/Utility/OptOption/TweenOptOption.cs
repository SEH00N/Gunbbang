using System;
using GB.Utility;
using UnityEngine;

namespace GB.Tweens
{
    [System.Serializable]
    public class TweenOptOption : OptOption<TweenSO>
    {
        public void Init(Transform body)
        {
            PositiveOption = PositiveOption?.CreateInstance(body);
            NegativeOption = NegativeOption?.CreateInstance(body);
        }

        public void PlayPositiveTween(Action callback = null)
        {
            PositiveOption?.PlayTween(callback);
        }

        public void PlayNegativeTween(Action callback = null)
        {
            NegativeOption?.PlayTween(callback);
        }

        public void ClearTween()
        {
            if(PositiveOption?.IsTweening == true)
                PositiveOption.ForceKillTween();

            if(NegativeOption?.IsTweening == true)
                NegativeOption.ForceKillTween();
        }
    }
}
