using UnityEngine;

namespace GB.Feedbacks
{
    public abstract class Feedback : MonoBehaviour
    {
        public abstract void Play(Vector3 playPos);
        public virtual void Stop() {}
    }
}
