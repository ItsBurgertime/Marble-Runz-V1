using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuUI : MonoBehaviour
{

    public void OnPlayButton()
    {
        Debug.Log("click");
        SceneManager.LoadScene(1);

    }
    public void OnQuitButton()
    {
        Debug.Log("click");
        Application.Quit();
    }
}