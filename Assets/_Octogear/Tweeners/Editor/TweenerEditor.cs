using UnityEngine;
using UnityEditor;

namespace Octogear
{
    [CustomEditor(typeof(Tweener))]
    [CanEditMultipleObjects]
    public class TweenerEditor : EditorScript<Tweener>
    {
        protected override void DrawCustomGUI()
        {
            EditorUtils.StartDebugArea();

            GUILayout.Space(4.0f);

            GUILayout.HorizontalSlider(target.CurrentTimeProgress, 0.0f, 1.0f);
            GUILayout.HorizontalSlider(target.CurrentProgress, 0.0f, 1.0f);

            Repaint();
        }
    }
}