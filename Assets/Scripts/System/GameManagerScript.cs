using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[DisallowMultipleComponent]
public class GameManagerScript : MonoBehaviour
{
    public static GameManagerScript instance;

    [SerializeField] GameObject pausePanel; 

    bool isPause = false;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private void Start()
    {
        pausePanel.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown("escape"))
        {
            if (isPause)
            {
                this.isPause = false;
                resume();
            } else
            {
                this.isPause = true;
                pause();
            }
        }   
    }

    public void pause()
    {
        Time.timeScale = 0;
        pausePanel?.SetActive(true);
    }

    public void resume()
    {
        Time.timeScale = 1;
        pausePanel?.SetActive(false);
    }
    public void quit()
    {
        Application.Quit();
    }
}