using UnityEngine;
using System.Collections;
using UnityEngine.Events;

public class TaskTimer : MonoBehaviour
{
    public float Delay = 0.0f;
    public bool StartOnAwake = false;
    public UnityEvent Trigger = new UnityEvent();

    public bool IsWaiting { get; private set; }

    float timeRunning = 0.0f;

    void Awake()
    {
        if (StartOnAwake)
        {
            Run();
        }
    }

    void Update()
    {
        if (IsWaiting)
        {
            timeRunning += Time.deltaTime;

            if (timeRunning >= Delay)
            {
                RunTrigger();
            }
        }
    }

    public void Run()
    {
        if (!IsWaiting)
        {
            IsWaiting = true;
            timeRunning = 0.0f;
        }
    }

    public void RunTrigger()
    {
        IsWaiting = false;
        timeRunning = 0.0f;
        Trigger.Invoke();
    }
}
