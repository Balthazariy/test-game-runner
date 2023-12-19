using DG.Tweening;
using System;

namespace TestGame.Utilities
{
    public static class InternalTools
    {
        public static Sequence DoActionDelayed(TweenCallback action, float delay = 0f)
        {
            if (action == null)
            {
                return null;
            }

            Sequence sequence = DOTween.Sequence();
            sequence.PrependInterval(delay);
            sequence.AppendCallback(action);

            return sequence;
        }

        public static T EnumFromString<T>(string value) where T : Enum
        {
            return (T)Enum.Parse(typeof(T), value);
        }
    }
}