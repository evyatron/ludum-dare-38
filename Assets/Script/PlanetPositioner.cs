using UnityEngine;

[ExecuteInEditMode]
public class PlanetPositioner : MonoBehaviour
{
    public float Offset = 0.0f;
    Ring ring = null;

    void LateUpdate()
    {
        if (ring == null)
        {
            ring = GetComponentInParent<Ring>();
        }

        if (ring != null)
        {
            Vector3 position = transform.localPosition;
            position.y = ring.Surface + Offset;
            transform.localPosition = position;
        }
    }
}