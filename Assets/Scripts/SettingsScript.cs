using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SettingsScript : MonoBehaviour
{
    public TMP_Dropdown _resolution_dropdown;
    private Resolution[] _resolutions;
    private GameObject _fps;

    void Start()
    {
        _fps = GameObject.Find("BasicCounter");

        InitializationResolutions();
        SetQuality(2);
    }

    private void InitializationResolutions()
    {
        _resolution_dropdown.ClearOptions();

        List<string> options = new List<string>();

        _resolutions = Screen.resolutions;

        for (int i = 0; i < _resolutions.Length; i++)
        {
            string option = _resolutions[i].width + "x" + _resolutions[i].height;

            options.Add(option);
        }

        _resolution_dropdown.AddOptions(options);
        _resolution_dropdown.RefreshShownValue();
    }

    #region Graphics Settings
    public void SetResolutions(int res_index)
    {
        Resolution resolution = _resolutions[res_index];

        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }

    public void SetQuality(int quality_index) => QualitySettings.SetQualityLevel(quality_index);

    public void SetFullScreen(bool is_full) => Screen.fullScreen = is_full;

    public void SetFps(bool is_fps)
    {
        if (is_fps)
            _fps.SetActive(true);
        else
            _fps.SetActive(false);
    }
    #endregion
}