using UnityEngine;
using System.Collections.Generic;
using System.Xml;
using UnityEngine.Events;

namespace Octogear
{
    public static class Utils
    {
        public static T RandomFromArray<T>(IList<T> array)
        {
            return array == null || array.Count == 0 ? default(T) : array[Random.Range(0, array.Count)];
        }

        public static T GetRandomEnum<T>()
        {
            return RandomFromArray((T[])System.Enum.GetValues(typeof(T)));
        }

        public static void Shuffle<T>(this List<T> list)
        {
            list.Sort((a, b) => { return Random.Range(0.0f, 1.0f) > 0.5f ? 1 : -1; });
        }

        public static Vector3 GetRandomPointInBounds(Bounds bounds)
        {
            Vector3 min = bounds.min;
            Vector3 max = bounds.max;
            return new Vector3(Random.Range(min.x, max.x), Random.Range(min.y, max.y), Random.Range(min.z, max.z));
        }

        public static void ClearChildren(Transform parent)
        {
            int childCount = parent.childCount;
            while (childCount-- > 0)
            {
                GameObject.DestroyImmediate(parent.GetChild(0).gameObject);
            }
        }

        public static void SetLayerRecursive(Transform parent)
        {
            SetLayerRecursive(parent, parent.gameObject.layer);
        }

        public static void SetLayerRecursive(Transform parent, string layer)
        {
            SetLayerRecursive(parent, LayerMask.NameToLayer(layer));
        }

        public static void SetLayerRecursive(Transform parent, int layer)
        {
            Transform[] children = parent.GetComponentsInChildren<Transform>();

            for (int i = 0; i < children.Length; i++)
            {
                if (children[i] != parent)
                {
                    children[i].gameObject.layer = layer;
                }
            }
        }
    }

    public static class XmlUtils
    {
        public static string Attribute(XmlNode xmlNode, string attributeName, string defaultValue = "")
        {
            return GetAttribute(xmlNode, attributeName, defaultValue);
        }

        public static T GetAttribute<T>(XmlNode xmlNode, string attributeName, T defaultValue = default(T))
        {
            XmlAttribute attribute = xmlNode.Attributes[attributeName];
            System.Type tType = typeof(T);

            if (attribute != null)
            {
                object value = null;

                if (tType == typeof(string))
                {
                    value = attribute.Value;
                }
                if (tType == typeof(bool))
                {
                    value = attribute.Value == "true" ? true : false;
                }
                if (tType == typeof(int))
                {
                    value = int.Parse(attribute.Value);
                }
                if (tType == typeof(float))
                {
                    value = float.Parse(attribute.Value);
                }

                return (T)System.Convert.ChangeType(value, tType);
            }

            return defaultValue;
        }
    }

    public static class DebugUtils
    {
        private static OctogearOnScreenDebugger debugger;

        public static void Log(string message)
        {
            if (debugger == null)
            {
                GameObject debuggerObject = new GameObject("Octogear On Screen Debugger");
                debugger = debuggerObject.AddComponent<OctogearOnScreenDebugger>();
            }

            debugger.AddMessage(message);
        }
    }

    public class OctogearOnScreenDebugger : MonoBehaviour
    {
        private List<string> messages = new List<string>();

        void OnGUI()
        {
            float currentY = 20.0f;
            
            for (int i = 0; i < messages.Count; i++)
            {
                GUI.Label(new Rect(10.0f, currentY, 200.0f, 20.0f), messages[i]);
                currentY += 25.0f;
            }
        }

        void LateUpdate()
        {
            messages.Clear();
        }

        public void AddMessage(string message)
        {
            messages.Add(message);
        }
    }
}