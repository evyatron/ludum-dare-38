using UnityEngine;
using UnityEditor;

namespace Octogear
{
    [CustomPropertyDrawer(typeof(RandomRangeInt))]
    public class RandomRangeIntDrawer : PropertyDrawer
    {
        float curveHeight = 20.0f;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);

            position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);

            GUIStyle style = new GUIStyle(GUI.skin.label);
            style.alignment = TextAnchor.MiddleCenter;

            EditorGUI.PropertyField(new Rect(position.x, position.y, position.width / 2.0f - 10.0f, position.height - curveHeight), property.FindPropertyRelative("Minimum"), GUIContent.none);
            EditorGUI.LabelField(new Rect(position.x + position.width / 2.0f - 10.0f, position.y, 20.0f, position.height - curveHeight), "-", style);
            EditorGUI.PropertyField(new Rect(position.x + position.width / 2.0f + 10.0f, position.y, position.width / 2.0f, position.height - curveHeight), property.FindPropertyRelative("Maximum"), GUIContent.none);

            Rect curveRect = new Rect(position.x, position.y + position.height - curveHeight, position.width, curveHeight);
            EditorGUI.CurveField(curveRect, property.FindPropertyRelative("Distribution"), Color.green, new Rect(0.0f, 0.0f, 1.0f, 1.0f), GUIContent.none);

            EditorGUI.EndProperty();
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return base.GetPropertyHeight(property, label) + curveHeight + 4.0f;
        }
    }
}