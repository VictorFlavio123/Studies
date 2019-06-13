using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneController : MonoBehaviour
{

    [SerializeField] public GameObject pausePanel;

    public Slider Timer;

    void Start()
    {
        pausePanel.SetActive(false);
        Debug.Log("Active Scene : " + SceneManager.GetActiveScene().name);
    }
    /*void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            if (!pausePanel.activeInHierarchy)
            {
                PauseGame();
            }
            if (pausePanel.activeInHierarchy)
            {
                ContinueGame();
            }
        }
    }*/
    public void PauseGame()
    {
        Time.timeScale = 0;
        pausePanel.SetActive(true);
        //Disable scripts that still work while timescale is set to 0
    }
    public void ContinueGame()
    {
        Time.timeScale = 1;
        pausePanel.SetActive(false);
        //enable the scripts again
    }
   /* IEnumerator PauseCoroutine()
    {
        Debug.Log("Waiting...");
        yield return new WaitUntil(() => frame >= 10);
        Debug.Log("Princess was rescued!");
    }*/
}
