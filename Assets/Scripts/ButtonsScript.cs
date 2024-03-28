using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonsScript : MonoBehaviour
{
    public GameObject _pause_panel;

    #region Buttons
    public void ContinueGame()
    {
        _pause_panel.SetActive(false);
        Time.timeScale = 1;
    }

    public void ExitGame()
    {
        Debug.Log("EXIT");
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
                Application.Quit();
        #endif
    }
    #endregion
}