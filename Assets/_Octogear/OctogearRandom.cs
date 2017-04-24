using UnityEngine;

namespace Octogear
{
    [System.Serializable]
    public class RandomRangeInt : RandomRange<int>
    {
        public RandomRangeInt() : base() { }
        public RandomRangeInt(RandomRangeInt toCopy) : base(toCopy) { }
        public RandomRangeInt(int min, int max) : base(min, max) { }
    }

    [System.Serializable]
    public class RandomRangeFloat : RandomRange<float>
    {
        public RandomRangeFloat() : base() { }
        public RandomRangeFloat(RandomRangeFloat toCopy) : base(toCopy) { }
        public RandomRangeFloat(float min, float max) : base(min, max) { }
    }

    public class RandomRange<T>
    {
        public T Minimum = default(T);
        public T Maximum = default(T);
        public AnimationCurve Distribution = AnimationCurve.Linear(0.0f, 0.0f, 1.0f, 1.0f);
        
        public T Value { get; private set; }

        public RandomRange()
        {
            
        }

        public RandomRange(RandomRange<T> toCopy)
        {
            Minimum = toCopy.Minimum;
            Maximum = toCopy.Maximum;
        }

        public RandomRange(T minValue, T maxValue)
        {
            Minimum = minValue;
            Maximum = maxValue;
        }

        public void SetValue(T value)
        {
            Value = value;
        }

        public T GetSpecific(float value)
        {
            T returnValue = default(T);

            if (typeof(T) == typeof(float))
            {
                float min = (float)(object)Minimum;
                float max = (float)(object)Maximum;
                returnValue = (T)(object)(min + Distribution.Evaluate(value) * (max - min));
            }
            else if (typeof(T) == typeof(int))
            {
                int min = (int)(object)Minimum;
                int max = (int)(object)Maximum;
                returnValue = (T)(object)(min + Distribution.Evaluate(value) * (max - min));
            }

            return returnValue;
        }

        public T Generate()
        {
            if (typeof(T) == typeof(float))
            {
                float min = (float)(object)Minimum;
                float max = (float)(object)Maximum;
                float value = min + Distribution.Evaluate(Random.Range(0.0f, 1.0f)) * (max - min);
                Value = (T)(object)value;
            }
            else if (typeof(T) == typeof(int))
            {
                int min = (int)(object)Minimum;
                int max = (int)(object)Maximum;
                int value = min + Mathf.RoundToInt(Distribution.Evaluate(Random.Range(0.0f, 1.0f)) * (max - min));
                Value = (T)(object)value;
            }
            else
            {
                Debug.LogWarning("Invalid Random Range type: " + typeof(T).FullName);
            }

            return Value;
        }
    }
}