using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Octogear;

public class InGameMenu : MonoBehaviour
{
    public Tweener TweenerShow;
    public Tweener TweenerHide;

    public bool IsActive
    {
        get
        {
            return isActive;
        }
        set
        {
            if (isActive != value)
            {
                if (value)
                {
                    Show();
                }
                else
                {
                    Hide();
                }
            }
        }
    }

    private bool isActive = false;

    void Start()
    {
        Hide();
        TweenerHide.JumpToEnd();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            IsActive = !IsActive;
        }
    }

    public void Show()
    {
        isActive = true;
        TweenerShow.Play();
        Time.timeScale = 0.0f;
    }

    public void Hide()
    {
        isActive = false;
        TweenerHide.Play();
        Time.timeScale = 1.0f;
    }
}