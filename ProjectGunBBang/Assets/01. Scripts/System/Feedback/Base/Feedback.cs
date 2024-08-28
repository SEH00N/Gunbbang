using UnityEngine;

namespace GB.Feedbacks
{
    public abstract class Feedback : MonoBehaviour
    {
        [SerializeField] bool stopOnPlay = false;

        public void Play(Vector3 playPos)
        {
            if(stopOnPlay)
                Stop();

            OnPlay(playPos);
        }

        protected abstract void OnPlay(Vector3 playPos);
        public virtual void Stop() {}
    }
}
