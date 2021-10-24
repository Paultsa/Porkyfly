using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public TimeWarp timeWarp;
    public GameObject button;
    [SerializeField] GameObject pauseMenu;
    bool isPaused = false;

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.O))
        {
            Pause();
        }
    }

    public void Pause()
    {
        switch (isPaused)
        {
            case true:
                Camera.main.GetComponent<TimeWarp>().ToggolePause();
                AudioManager.PlaySound("menu_unpause");
                
                isPaused = false;
                Debug.Log("UNPAUSE");
                pauseMenu.SetActive(false);
                

                break;
            case false:
                AudioManager.PlaySound("menu_pause");
                Debug.Log("PAUSE");
                isPaused = true;
                
                pauseMenu.SetActive(true);
                Camera.main.GetComponent<TimeWarp>().ToggolePause();
                break;
        }
        
        
    }

}
