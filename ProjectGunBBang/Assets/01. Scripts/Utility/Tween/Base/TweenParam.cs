using DG.Tweening;
using UnityEngine;

namespace GB.Tweens
{
    [System.Serializable]
    public struct TweenParam
    {
        public string Caption;
        public float Delay;
        public float Duration;
        public float Value;
        public Ease Ease;
        public Color Color;
    }
}