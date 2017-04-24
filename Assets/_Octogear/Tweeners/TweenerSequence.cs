using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Events;

namespace Octogear
{
    public class TweenerSequence : MonoBehaviour
    {
        public List<Tweener> Tweeners = new List<Tweener>();

        public UnityEvent OnStart = new UnityEvent();
        public UnityEvent OnFinish = new UnityEvent();

        void Awake()
        {
            Tweeners[0].Events.OnStart.AddListener(OnFirstTweenerStart);

            for (int i = 0; i < Tweeners.Count; i++)
            {
                Tweener tweener = Tweeners[i];
                tweener.Events.OnFinish.AddListener(delegate { OnTweenerFinish(tweener); });
            }
        }

        public void Play()
        {
            Tweeners[0].Play();
        }

        public void JumpToEnd()
        {
            for (int i = 0; i < Tweeners.Count; i++)
            {
                Tweeners[i].JumpToEnd();
            }
        }

        public void JumpToStart()
        {
            for (int i = 0; i < Tweeners.Count; i++)
            {
                Tweeners[i].JumpToStart();
            }
        }

        void OnFirstTweenerStart()
        {
            OnStart.Invoke();
        }

        void OnTweenerFinish(Tweener tweener)
        {
            int tweenerIndex = Tweeners.IndexOf(tweener);

            if (tweenerIndex == Tweeners.Count - 1)
            {
                OnFinish.Invoke();
            }
            else
            {
                Tweeners[tweenerIndex + 1].Play();
            }
        }
    }
}