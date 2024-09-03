using DG.Tweening;
using GB.Extensions;
using UnityEngine;

namespace GB.Tweens
{
    [CreateAssetMenu(menuName = "SO/Tween/Utility/List", order = -1)]
    public class TweenListSO : TweenSO
    {
        [SerializeField] TweenSO[] tweenList = null; 

        public override TweenSO CreateInstance(Transform body)
        {
            TweenListSO instance = ScriptableObject.Instantiate(this);

            instance.tweenList.ForEach((i, index) => {
                instance.tweenList[index] = i.CreateInstance(body);
            });

            return instance;
        }

        protected override void OnTween(Sequence sequence)
        {
            tweenList.ForEach(i => {
                sequence.Join(i.CreateTween());
            });
        }
    }
}
