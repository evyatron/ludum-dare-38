using UnityEngine;
using UnityEngine.Events;

namespace Octogear
{
    [System.Serializable]
    public class TweenerEvents
    {
        public UnityEvent OnStart = new UnityEvent();
        public UnityEvent OnFinish = new UnityEvent();
        public UnityEvent OnStartReverse = new UnityEvent();
        public UnityEvent OnFinishReverse = new UnityEvent();
        public UnityEventFloat OnProgressUpdate = new UnityEventFloat();
    }

    public class Tweener : MonoBehaviour
    {
        public AnimationCurve AnimationCurve = new AnimationCurve();
        public float AnimationDuration = 1.0f;
        public float AnimationDelay = 0.0f;
        public TweenerLoopType LoopType = TweenerLoopType.PlayOnce;
        public bool PlayOnAwake = false;
        public bool PlayReverseOnAwake = false;
        public bool UseUnscaledTime = false;
        public bool UseFixedUpdate = false;
        public float ProgressScaleMin = 0.0f;
        public float ProgressScaleMax = 1.0f;
        public float CustomTimeScale = 1.0f;

        [Space(5)]
        public TweenerEvents Events = new TweenerEvents();

        public float CurrentTimeProgress { get { return timeRunning / AnimationDuration; } }
        public float CurrentProgressUnscaled { get { return currentProgress; } }
        public float CurrentProgress
        {
            get
            {
                return Mathf.LerpUnclamped(ProgressScaleMin, ProgressScaleMax, currentProgress);
            }
        }

        public bool IsRunning { get; private set; }

        bool isPaused = false;
        float timeRunning = 0.0f;
        bool isPlayingForward = false;
        private float currentProgress = 0.0f;
        TweenerState state = TweenerState.Stopped;

        public enum TweenerState
        {
            Stopped,
            WaitingForDelay,
            Running,
            FinishOnEnable
        }

        public enum TweenerLoopType
        {
            PlayOnce,
            StartOver,
            PingPong
        }

        void Start()
        {
            if (PlayOnAwake)
            {
                Play();
            }
            else if (PlayReverseOnAwake)
            {
                PlayReverse();
            }
        }

        void Update()
        {
            if (!UseFixedUpdate)
            {
                DoUpdate();
            }
        }

        void FixedUpdate()
        {
            if (UseFixedUpdate)
            {
                DoUpdate();
            }
        }

        void DoUpdate()
        {
            if (IsRunning && !isPaused)
            {
                timeRunning += (UseUnscaledTime ? Time.unscaledDeltaTime : Time.deltaTime) * CustomTimeScale;

                if (state == TweenerState.WaitingForDelay)
                {
                    if (timeRunning >= AnimationDelay)
                    {
                        timeRunning = 0.0f;
                        state = TweenerState.Running;

                        if (isPlayingForward)
                        {
                            Events.OnStart.Invoke();
                        }
                        else
                        {
                            Events.OnStartReverse.Invoke();
                        }
                    }
                }
                else
                {
                    JumpToPoint(timeRunning / AnimationDuration);
                }
            }
        }

        void OnEnable()
        {
            if (state == TweenerState.FinishOnEnable)
            {
                JumpToEnd();
            }
        }

        public void JumpToStart()
        {
            Play();
            JumpToPoint(0.0f);
        }

        public void JumpToEnd()
        {
            Play();
            JumpToPoint(1.0f);
        }

        public void JumpToPoint(float progress)
        {
            progress = Mathf.Clamp01(progress);

            bool didEnd = progress >= 1.0f;

            if (!isPlayingForward)
            {
                progress = 1 - progress;
            }

            currentProgress = AnimationCurve.Evaluate(progress);

            Events.OnProgressUpdate.Invoke(CurrentProgress);

            if (didEnd)
            {
                if (isPlayingForward)
                {
                    Events.OnFinish.Invoke();
                }
                else
                {
                    Events.OnFinishReverse.Invoke();
                }

                if (LoopType == TweenerLoopType.StartOver)
                {
                    Play(false);
                }
                else if (LoopType == TweenerLoopType.PingPong)
                {
                    if (isPlayingForward)
                    {
                        PlayReverse(false);
                    }
                    else
                    {
                        Play(false);
                    }
                }
                else
                {
                    state = TweenerState.Stopped;
                    IsRunning = false;
                }
            }
        }

        public void Stop()
        {
            if (IsRunning)
            {
                currentProgress = 0.0f;
                Events.OnProgressUpdate.Invoke(CurrentProgress);

                state = TweenerState.Stopped;
                IsRunning = false;
                isPaused = false;

                if (isPlayingForward)
                {
                    Events.OnFinish.Invoke();
                }
                else
                {
                    Events.OnFinishReverse.Invoke();
                }
            }
        }

        public void Pause()
        {
            isPaused = true;
        }

        public void Play()
        {
            if (!isActiveAndEnabled)
            {
                state = TweenerState.FinishOnEnable;
            }
            else
            {
                if (!isPaused)
                {
                    Play(true);
                }

                isPaused = false;
            }
        }

        public void Play(bool isFirstPlay)
        {
            IsRunning = true;
            isPlayingForward = true;
            timeRunning = 0.0f;
            currentProgress = AnimationCurve.Evaluate(0.0f);

            if (isFirstPlay && AnimationDelay != 0.0f)
            {
                state = TweenerState.WaitingForDelay;
            }
            else
            {
                state = TweenerState.Running;

                if (isFirstPlay)
                {
                    Events.OnStart.Invoke();
                }
            }
        }

        public void PlayReverse()
        {
            PlayReverse(true);
        }

        public void PlayReverse(bool isFirstPlay)
        {
            IsRunning = true;
            isPlayingForward = false;
            timeRunning = 0.0f;
            currentProgress = AnimationCurve.Evaluate(1.0f);

            if (isFirstPlay && AnimationDelay != 0.0f)
            {
                state = TweenerState.WaitingForDelay;
            }
            else
            {
                state = TweenerState.Running;

                if (isFirstPlay)
                {
                    Events.OnStartReverse.Invoke();
                }
            }
        }

        public void SetDuration(float duration)
        {
            AnimationDuration = duration;
        }
    }
}