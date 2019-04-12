using System;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts.System
{
    public static class StaticMethods
    {
        public static Vector2 NatrualValues(this Vector2 vector)
        {
            var returnValue = vector;
            if (returnValue.x < 0)
            {
                returnValue.x *= -1;
            }

            if (returnValue.y < 0)
            {
                returnValue.y *= -1;
            }
            return returnValue;
        }

        public static Vector2Int ToVector2Int(this Vector2 vector)
        {
            return new Vector2Int((int)vector.x, (int)vector.y);
        }

        public static Vector2 ToVector2(this Vector3 vector)
        {
            return new Vector2(vector.x, vector.y);
        }

        public static IEnumerator WaitForFrames(int frames, Action doAfter)
        {
            var totalFrames = Time.frameCount + frames;
            yield return new WaitUntil(() => Time.frameCount >= totalFrames);
            doAfter?.Invoke();
        }

    }

}