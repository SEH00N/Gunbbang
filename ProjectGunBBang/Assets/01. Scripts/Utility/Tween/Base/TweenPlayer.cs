using UnityEngine;

namespace GB.Tweens
{
    public class TweenPlayer : MonoBehaviour
    {
        [SerializeField] TweenSO tween = null;
        [SerializeField] Transform body = null;

        [Space(15f)]
        [SerializeField] bool playOnAwake = true;
        [SerializeField] bool loop = false;

        private void Awake()
        {
            if(body == null)
                body = transform;

            tween = tween.CreateInstance(body);

            if(playOnAwake)
                tween.PlayTween();
        }

        private void OnDestroy()
        {
            if (tween.IsTweening)
                tween.ForceKillTween();
        }

        public void PlayTween()
        {
            if(loop)
                tween.PlayTween(PlayTween);
            else
                tween.PlayTween();
        }
    }
}
