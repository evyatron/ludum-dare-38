using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerPrefs : MonoBehaviour
{
    public bool ShowTutorials = true;

    public static PlayerPrefs Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<PlayerPrefs>();

                if (instance == null)
                {
                    instance = new GameObject("Player Prefs").AddComponent<PlayerPrefs>();
                }

                DontDestroyOnLoad(instance.gameObject);
            }

            return instance;
        }
    }

    private static PlayerPrefs instance;
}