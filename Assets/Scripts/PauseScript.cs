using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseScript : MonoBehaviour
{
    public GameObject _pause_panel;

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
            TogglePausePanel();
    }

    void TogglePausePanel()
    {
        if (_pause_panel.activeSelf == false)
        {
            _pause_panel.SetActive(true);
            Time.timeScale = 0;
        }
        else
        {
            _pause_panel.SetActive(false);
            Time.timeScale = 1;
        }
    }
}
