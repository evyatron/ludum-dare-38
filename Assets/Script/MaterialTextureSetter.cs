using UnityEngine;

[RequireComponent(typeof(Renderer))]
public class MaterialTextureSetter : MonoBehaviour
{
    public Sprite Sprite;

    void Awake()
    {
        SetSprite(Sprite);
    }

    public void SetSprite(Sprite sprite)
    {
        if (Application.isPlaying)
        {
            Renderer renderer = GetComponent<Renderer>();

            if (renderer != null)
            {
                renderer.material.mainTexture = sprite.texture;
            }
        }
    }
}