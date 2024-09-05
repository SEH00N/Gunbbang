using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GB.Feedbacks
{
    public class MuzzleFlashFeedback : Feedback
    {
        private ParticleSystem muzzleEffect;

        private void Awake()
        {
            muzzleEffect = GetComponent<ParticleSystem>();
        }

        protected override void OnPlay(Vector3 playPos)
        {
            muzzleEffect.Play();
        }
    }
}
