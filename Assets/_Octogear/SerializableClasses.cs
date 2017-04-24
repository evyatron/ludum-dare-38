using UnityEngine;
using UnityEngine.Events;

namespace Octogear
{
    [System.Serializable]
    public class UnityEventInt : UnityEvent<int> { }
    [System.Serializable]
    public class UnityEventFloat : UnityEvent<float> { }
    [System.Serializable]
    public class UnityEventBool : UnityEvent<bool> { }
    [System.Serializable]
    public class UnityEventString : UnityEvent<string> { }

    [System.Serializable]
    public class SerializableVector3
    {
        float x;
        float y;
        float z;

        public SerializableVector3(Vector3 vector)
        {
            x = vector.x;
            y = vector.y;
            z = vector.z;
        }

        // implicit conversion from SerializableColor to Color
        // so it can be used like texture.color = SerializableColor
        static public implicit operator Vector3(SerializableVector3 serialized)
        {
            return serialized == null ? Vector3.zero : new Vector3(serialized.x, serialized.y, serialized.z);
        }

        // implicit conversion from Color to SerializableColor
        // so it can be used like SerializableColor color = Color.red
        static public implicit operator SerializableVector3(Vector3 vector)
        {
            return new SerializableVector3(vector);
        }
    }

    [System.Serializable]
    public class SerializableVector2
    {
        public float x;
        public float y;

        public SerializableVector2(Vector2 vector)
        {
            x = vector.x;
            y = vector.y;
        }

        // implicit conversion from SerializableColor to Color
        // so it can be used like texture.color = SerializableColor
        static public implicit operator Vector2(SerializableVector2 serialized)
        {
            return serialized == null ? Vector2.zero : new Vector2(serialized.x, serialized.y);
        }

        // implicit conversion from Color to SerializableColor
        // so it can be used like SerializableColor color = Color.red
        static public implicit operator SerializableVector2(Vector2 vector)
        {
            return new SerializableVector2(vector);
        }
    }

    [System.Serializable]
    public class SerializableColor
    {
        float Red;
        float Green;
        float Blue;
        float Alpha;

        public SerializableColor(Color color)
        {
            Red = color.r;
            Green = color.g;
            Blue = color.b;
            Alpha = color.a;
        }

        // implicit conversion from SerializableColor to Color
        // so it can be used like texture.color = SerializableColor
        static public implicit operator Color(SerializableColor serialized)
        {
            return new Color(serialized.Red, serialized.Green, serialized.Blue, serialized.Alpha);
        }

        // implicit conversion from Color to SerializableColor
        // so it can be used like SerializableColor color = Color.red
        static public implicit operator SerializableColor(Color colour)
        {
            return new SerializableColor(colour);
        }
    }
}