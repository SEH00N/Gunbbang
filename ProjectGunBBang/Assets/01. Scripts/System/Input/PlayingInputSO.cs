using System;
using UnityEngine;

namespace GB.Inputs
{
    [CreateAssetMenu(menuName = "SO/Input/PlayingInput")]
    public class PlayingInputSO : InputSO
    {
        public Vector3 MovementInput = Vector3.zero;
        public Action OnJumpEvent = null;
    }
}
