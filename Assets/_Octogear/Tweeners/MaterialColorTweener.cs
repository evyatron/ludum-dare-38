using UnityEngine;

namespace Octogear
{
    public class MaterialColorTweener : MonoBehaviour
    {
        public string PropertyName = "_Tint";
        public Tweener Tweener;
        public MeshRenderer Renderer;
        public Color From = Color.clear;
        public Color To = Color.clear;
        public bool AlphaOnly = false;

        void Awake()
        {
            if (Tweener == null)
            {
                Tweener = GetComponent<Tweener>();
            }

            Tweener.Events.OnProgressUpdate.AddListener(OnProgressUpdate);

            if (AlphaOnly)
            {
                float fromAlpha = From.a;
                float targetAlpha = To.a;

                From = Renderer.sharedMaterial.GetColor(PropertyName);
                To = From;

                From.a = fromAlpha;
                To.a = targetAlpha;
            }
        }

        void OnProgressUpdate(float progress)
        {
            Renderer.material.SetColor(PropertyName, Color.LerpUnclamped(From, To, progress));
        }
    }
}