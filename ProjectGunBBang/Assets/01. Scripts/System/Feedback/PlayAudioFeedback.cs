using UnityEngine;

namespace GB.Feedbacks
{
    public class PlayAudioFeedback : Feedback
    {
        [SerializeField] AudioClip clip;
        [SerializeField] AudioSource source;

        protected override void OnPlay(Vector3 playPos)
        {
            source.PlayOneShot(clip);
        }
    }
}
