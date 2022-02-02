using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Main_Menu_Script : MonoBehaviour
{
    public int last_Save = 1;

    public void Play_Game()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + last_Save);
    }

    public void Exit_Game()
    {
        Application.Quit();
    }
}
