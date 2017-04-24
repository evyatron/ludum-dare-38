using UnityEngine;
using Octogear;
using System.Collections;
using System.Collections.Generic;

public class LightTweener : MonoBehaviour
{
    public Tweener Tweener = null;
    public Light ReferenceLight = null;

    [Header("Intensity")]
    public bool TweenIntensity = false;
    public float MinIntensity = 0.0f;
    public float MaxIntensity = 0.0f;

    [Header("Range")]
    public bool TweenRange = false;
    public float MinRange = 0.0f;
    public float MaxRange = 0.0f;

    [Header("Spot Angle")]
    public bool TweenSpotAngle = false;
    public float MinSpotAngle = 0.0f;
    public float MaxSpotAngle = 0.0f;

    void Awake()
    {
        if (Tweener == null)
        {
            Tweener = GetComponent<Tweener>();
        }

        if (ReferenceLight == null)
        {
            ReferenceLight = GetComponent<Light>();
        }

        Tweener.Events.OnProgressUpdate.AddListener(UpdateLight);
    }
    
    void UpdateLight(float progress)
    {
        if (TweenIntensity)
        {
            ReferenceLight.intensity = Mathf.LerpUnclamped(MinIntensity, MaxIntensity, progress);
        }

        if (TweenRange)
        {
            ReferenceLight.range = Mathf.LerpUnclamped(MinRange, MaxRange, progress);
        }

        if (TweenSpotAngle)
        {
            ReferenceLight.spotAngle = Mathf.LerpUnclamped(MinSpotAngle, MaxSpotAngle, progress);
        }
    }
}