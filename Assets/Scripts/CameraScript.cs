using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    [SerializeField]
    private GameObject _camera_anchor;
    private Vector3 _camera_OFF_set;
    private Vector3 _initial_OFF_set;
    private Vector3 _camera_angles;
    private Vector3 _initial_angles;

    void Start()
    {
        _camera_OFF_set = _camera_OFF_set = this.transform.position - _camera_anchor.transform.position;
        _initial_angles = _camera_angles = this.transform.eulerAngles;
    }

    void Update()
    {
        float mx = Input.GetAxis("Mouse X");
        float my = Input.GetAxis("Mouse Y");

        _camera_angles.y += mx;
        _camera_angles.x -= my;

        if (Input.GetKey(KeyCode.V))
            _camera_OFF_set = _camera_OFF_set == Vector3.zero ? _initial_OFF_set : Vector3.zero;
    }

    void LateUpdate()
    {
        this.transform.position = _camera_anchor.transform.position + Quaternion.Euler(0, _camera_angles.y - _initial_angles.y, 0) * _camera_OFF_set;
        this.transform.eulerAngles = _camera_angles;
    }
}
