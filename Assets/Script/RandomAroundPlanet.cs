using UnityEngine;

public class RandomAroundPlanet : MonoBehaviour
{
    public bool PositionOnAwake = false;
    public float MinAngle = 0.0f;
    public float MaxAngle = 360.0f;

    void Awake()
    {
        if (PositionOnAwake)
        {
            Position();
        }
    }

    [ContextMenu("Position")]
    public void Position()
    {
        transform.eulerAngles = Vector3.forward * Random.Range(MinAngle, MaxAngle);
    }
}