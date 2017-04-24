using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class UIPlayerHealth : MonoBehaviour
{
    public Image[] hearts;

    void Awake()
    {

    }

    void Update()
    {

    }

    public void Set(float health)
    {
        health -= 1.0f;

        for (int i = 0; i < hearts.Length; i++)
        {
            if (i <= health)
            {
                hearts[i].fillAmount = 1.0f;
            }
            else
            {
                hearts[i].fillAmount = 0.0f;
            }
        }

        if (health - Mathf.Floor(health) > 0)
        {
            hearts[Mathf.CeilToInt(health)].fillAmount = health - Mathf.Floor(health);
        }
    }
}