using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[ExecuteInEditMode]
public class SpriteShadow : MonoBehaviour
{
    public SpriteRenderer ReferenceSprite;
    public Transform ReferenceTransform;

    private SpriteRenderer OwnSprite = null;
    private PlanetLight planetLight = null;

    void Update()
    {
        if (ReferenceTransform == null)
        {
            ReferenceTransform = transform;
        }

        if (ReferenceSprite == null)
        {
            ReferenceSprite = GetComponent<SpriteRenderer>();
        }
        if (OwnSprite == null)
        {
            OwnSprite = GetComponent<SpriteRenderer>();
        }

        if (ReferenceSprite != null)
        {
            ReferenceTransform = ReferenceSprite.transform;

            if (OwnSprite != null)
            {
                OwnSprite.sprite = ReferenceSprite.sprite;
            }
        }

        if (planetLight == null)
        {
            planetLight = FindObjectOfType<PlanetLight>();
        }

        if (planetLight != null)
        {
            Planet planet = planetLight.GetComponentInParent<Planet>();

            if (planet != null)
            {
                Vector3 offset = planetLight.transform.position - planet.transform.position;

                if (ReferenceTransform == transform.parent)
                {
                    transform.localScale = new Vector3(1.0f, 1.0f, -offset.z);
                }
                else
                {
                    transform.localScale = ReferenceTransform.localScale + new Vector3(0.0f, 0.0f, -offset.z);
                }

                transform.position = ReferenceTransform.position - new Vector3(offset.x, offset.y, offset.z * 0.1f);
            }
        }
    }
}