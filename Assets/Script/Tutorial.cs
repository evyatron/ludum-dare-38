using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using Octogear;

public class Tutorial : MonoBehaviour
{
    public const string kTutorialIntro = "intro";
    public const string kTutorialProjectiles = "projectile";
    public const string kTutorialEnd = "tutorial-over";

    public static Tutorial Instance;

    public Text Text;
    public List<TutorialStep> Steps = new List<TutorialStep>();
    public Tweener TweenerShow;
    public Tweener TweenerHide;
    public float TimeToShow = 5.0f;

    public bool IsActive { get; protected set; }

    private float timer = 0.0f;

    void Awake()
    {
        Instance = this;
        TweenerHide.JumpToEnd();
        TweenerHide.Events.OnFinish.AddListener(OnHideFinish);
    }

    void LateUpdate()
    {
        if (IsActive)
        {
            timer += Time.unscaledDeltaTime;

            if (timer >= TimeToShow || Input.GetMouseButton(0) || !string.IsNullOrEmpty(Input.inputString))
            {
                if (!TweenerHide.IsRunning)
                {
                    TweenerShow.Stop();
                    TweenerHide.Play();
                }
            }
        }
    }

    public void DoStep(string id)
    {
        if (!PlayerPrefs.Instance.ShowTutorials)
        {
            return;
        }

        TutorialStep step = GetStep(id);

        if (step == null || step.IsDone)
        {
            return;
        }

        TweenerShow.Stop();
        TweenerHide.Stop();

        Text.text = step.Text;
        TweenerShow.Play();

        step.IsDone = true;
        Time.timeScale = 0.0f;

        timer = 0.0f;
        IsActive = true;
    }

    public TutorialStep GetStep(string id)
    {
        foreach (TutorialStep step in Steps)
        {
            if (step.Id == id)
            {
                return step;
            }
        }

        return null;
    }

    private void OnHideFinish()
    {
        IsActive = false;
        Time.timeScale = 1.0f;
    }

    [System.Serializable]
    public class TutorialStep
    {
        public string Id = "";
        public string Text = "";
        public bool IsDone = false;
    }
}