using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Mainmenu : MonoBehaviour
{
    public void PlayGame()
    {
        PlayerPrefs.SetInt("MaxHP",2);
        PlayerPrefs.SetInt("HP", 2);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
      }
    public void QuizGame()
    {
        Application.Quit();
    }

}
