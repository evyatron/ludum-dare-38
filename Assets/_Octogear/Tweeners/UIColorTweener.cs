using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

namespace Octogear
{
    public class UIColorTweener : MonoBehaviour
    {
        public Tweener Tweener;
        public Color From = Color.clear;
        public Color To = Color.clear;
        public bool AlphaOnly = false;
        public bool StartFromCurrent = false;
        public bool EndOnCurrent = false;

        [Header("UI")]
        public Image Image;
        public Text Text;

        [Header("World")]
        public TextMesh TextMesh;
        public SpriteRenderer Sprite;

        void Awake()
        {
            if (Tweener == null)
            {
                Tweener = GetComponent<Tweener>();
            }

            Tweener.Events.OnProgressUpdate.AddListener(OnProgressUpdate);

            if (AlphaOnly)
            {
                Color currentColor = GetColor();
                float fromAlpha = From.a;
                float toAlpha = To.a;
                float currentAlpha = currentColor.a;

                From = currentColor;
                To = currentColor;

                From.a = StartFromCurrent ? currentAlpha : fromAlpha;
                To.a = EndOnCurrent ? currentAlpha : toAlpha;

                OnProgressUpdate(0.0f);
            }
        }

        void OnProgressUpdate(float progress)
        {
            if (Image != null)
            {
                Image.color = Color.LerpUnclamped(From, To, progress);
            }
            if (Text != null)
            {
                Text.color = Color.LerpUnclamped(From, To, progress);
            }
            if (TextMesh != null)
            {
                TextMesh.color = Color.LerpUnclamped(From, To, progress);
            }
            if (Sprite != null)
            {
                Sprite.color = Color.LerpUnclamped(From, To, progress);
            }
        }

        Color GetColor()
        {
            if (Image != null)
            {
                return Image.color;
            }
            if (Text != null)
            {
                return Text.color;
            }
            if (TextMesh != null)
            {
                return TextMesh.color;
            }
            if (Sprite != null)
            {
                return Sprite.color;
            }

            return Color.clear;
        }
    }
}