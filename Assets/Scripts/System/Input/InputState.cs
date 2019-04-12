using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.System.Input
{
    public class InputState
    {
        public Vector2 LeftStick { get; set; }
        public Vector2 RightStick { get; set; }
        public List<GameInputButton> Buttons { get; set; }

    }
}