using UnityEngine;

public class Rotator : MonoBehaviour
{
    public float RotationSpeed = 0.0f;
    public Rotator OtherRotator;
    public bool UseUnscaledTime = false;

    public float SetSpeed { set { RotationSpeed = value; } }

    void Update()
    {
        if (OtherRotator != null)
        {
            RotationSpeed = OtherRotator.RotationSpeed;
        }

        float timeScale = UseUnscaledTime ? Time.unscaledDeltaTime : Time.deltaTime;
        transform.Rotate(0.0f, 0.0f, RotationSpeed * timeScale);
    }
}