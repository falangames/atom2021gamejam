using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public void OnClick_PlayButton()
    {
        SceneManager.LoadScene("MainScene");
    }

    public void OnClick_InfoButton()
    {
        SceneManager.LoadScene("InfoScene");
    }

    public void OnClick_MenuButton()
    {
        SceneManager.LoadScene("MenuScene");
    }
}
