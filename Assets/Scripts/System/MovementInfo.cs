using System;

namespace Assets.Scripts.System
{
    [Serializable]
    public class MovementInfo
    {
        public float Acceleration;
        public float MaxSpeed;
        public float MaxDistance;


        public void SetValue(MovementValueType type, float value)
        {
            switch (type)
            {
                case MovementValueType.Acceleration:
                    Acceleration += value;
                    break;
                case MovementValueType.MaxSpeed:
                    MaxSpeed += value;
                    break;
                case MovementValueType.MaxDistance:
                    MaxDistance += value;
                    break;
            }
        }

        public float GetValue(MovementValueType type)
        {
            var returnValue = 0f;
            switch (type)
            {
                case MovementValueType.Acceleration:
                    returnValue = Acceleration;
                    break;
                case MovementValueType.MaxSpeed:
                    returnValue = MaxSpeed;
                    break;
                case MovementValueType.MaxDistance:
                    returnValue = MaxDistance;
                    break;
            }
            return returnValue;
        }
    }
}