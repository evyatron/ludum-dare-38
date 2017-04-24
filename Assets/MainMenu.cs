using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public string ByLineURL = "";
    public Toggle TutorialToggle;

    void Awake()
    {

    }

    void Update()
    {

    }

    public void OnClickNew()
    {
        PlayerPrefs.Instance.ShowTutorials = TutorialToggle.isOn;
        SceneManager.LoadScene("default");
    }

    public void OnClickQuit()
    {
        Application.Quit();
    }

    public void OnClickBackToMenu()
    {
        SceneManager.LoadScene("MENU");
    }

    public void OnClickByLine()
    {
        Application.OpenURL(ByLineURL);
    }
}