using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RandomTexture : MonoBehaviour
{
    public List<Sprite> Sprites;

    public Sprite Sprite { get; protected set; }

    void Awake()
    {
        SetRandom();
    }

    [ContextMenu("Set Random")]
    public void SetRandom()
    {
        SpriteRenderer spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        Renderer meshRenderer = GetComponentInChildren<Renderer>();

        Sprite = Sprites[Random.Range(0, Sprites.Count)];

        if (meshRenderer != null)
        {
            if (Application.isPlaying)
            {
                meshRenderer.material.mainTexture = Sprite.texture;
            }
        }

        if (spriteRenderer != null)
        {
            spriteRenderer.sprite = Sprite;
        }
    }
}