using UnityEngine;
using UnityEngine.Events;

#if UNITY_EDITOR
using UnityEditor;
namespace Octogear
{
    public static class EditorUtils
    {
        public static void StartDebugArea(string title = "DEBUG")
        {
            GUILayout.Space(10.0f);
            DrawDebugTitle(title);
        }

        public static void DrawDebugTitle(string title = "DEBUG")
        {
            GUIStyle headerStyle = GUIStyle.none;
            headerStyle.fontStyle = FontStyle.Bold;
            headerStyle.alignment = TextAnchor.MiddleCenter;
            GUILayout.Label(string.Format("=== {0} ===", title), headerStyle);
        }

        public static void DrawHorizontalButtons(ButtonSettings[] buttons)
        {
            DrawButtons(buttons, true);
        }

        public static void DrawButtons(ButtonSettings[] buttons, bool isHorizontal = false)
        {
            GUILayout.Space(4.0f);

            if (isHorizontal)
            {
                GUILayout.BeginHorizontal();
            }

            for (int i = 0; i < buttons.Length; i++)
            {
                DrawButton(buttons[i]);
            }

            if (isHorizontal)
            {
                GUILayout.EndHorizontal();
            }
        }

        public static void DrawButton(ButtonSettings buttonSettings, GUIStyle style = null)
        {
            DrawButton(buttonSettings.Text, buttonSettings.Action, buttonSettings.Style);
        }

        public static void DrawButton(string buttonText, UnityAction action, GUIStyle style = null)
        {
            if (style == null)
            {
                if (GUILayout.Button(buttonText))
                {
                    action.Invoke();
                }
            }
            else
            {
                if (GUILayout.Button(buttonText, style))
                {
                    action.Invoke();
                }
            }
        }

        public static void DrawDisabledButton(string buttonText)
        {
            GUIStyle buttonStyle = new GUIStyle(GUI.skin.button);
            buttonStyle.normal.textColor = Color.grey;
            GUILayout.Label(buttonText, buttonStyle);
        }

        public struct ButtonSettings
        {
            public string Text;
            public UnityAction Action;
            public GUIStyle Style;
        }

        public static class Styles
        {
            public static GUIStyle WarningButton
            {
                get
                {
                    GUIStyle style = new GUIStyle(GUI.skin.button);
                    style.normal.textColor = Color.red;
                    style.fontStyle = FontStyle.Bold;
                    return style;
                }
            }
        }
    }

    /*
        Make it easier to create custom editor scripts, usage:
        
        [CustomEditor(typeof(YOUR_CLASS))]
        public class YOUR_CLASS_Editor : EditorScript<YOUR_CLASS>
        {
            protected override void DrawCustomGUI()
            {
                ...
            }
        }
    */
    public class EditorScript<T> : Editor
    {
        new protected T target;

        void OnEnable()
        {
            target = (T)System.Convert.ChangeType(base.target, typeof(T));
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            DrawCustomGUI();
        }

        protected virtual void DrawCustomGUI() { }
    }
}
#endif