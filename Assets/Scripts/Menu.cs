using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    // Start is called before the first frame update
    public void PlayGame()
    {
        SceneManager.LoadScene ("SampleScene");
    }
    public void ExitGame()
    {
        Application.Quit();
    }

    public void RestartGame()
    {
        Health.totalHealth = 1f;   // сброс здоровья
        Scoring.totalScore = 0;    // сброс очков
        SceneManager.LoadScene("SampleScene");
    }

}

