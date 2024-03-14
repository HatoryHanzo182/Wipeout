using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class DaySystem : MonoBehaviour
{
    [SerializeField]
    Gradient directionalLightGradient;
    [SerializeField]
    Gradient ambientLightGradient;
    [SerializeField, Range(1, 3600)]
    float timeDayInSeconds = 60;
    [SerializeField, Range(0f, 1f)]
    float timeProgress;
    [SerializeField]
    Light dirlight;
    Vector3 defaultAngles;

    void Start()
    {
        defaultAngles = dirlight.transform.localEulerAngles;
    }

    void Update()
    {
        if (Application.isPlaying)
            timeProgress += Time.deltaTime / timeDayInSeconds;

        if (timeProgress > 1f)
            timeProgress = 0f;

        dirlight.color = directionalLightGradient.Evaluate(timeProgress);
        
        RenderSettings.ambientLight = ambientLightGradient.Evaluate(timeProgress);
        
        dirlight.transform.localEulerAngles = new Vector3(x: 360f * timeProgress - 90, defaultAngles.x, defaultAngles.z);
    }
}