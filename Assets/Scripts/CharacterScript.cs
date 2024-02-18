using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterScript : MonoBehaviour
{
    [SerializeField]
    private float _speed;
    private CharacterController _character_controller;
    private Vector3 _move_step;

    void Start()
    {
        _character_controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        float dx = Input.GetAxis("Horizontal");
        float dz = Input.GetAxis("Vertical");

        if (Mathf.Abs(dx) > 0f && Mathf.Abs(dz) > 0f)
        {
            dx /= Mathf.Sqrt(2f);
            dz /= Mathf.Sqrt(2f);
        }

        _move_step = (dx * Camera.main.transform.right + dz * Camera.main.transform.forward);

        _character_controller.SimpleMove(Time.deltaTime * _speed * _move_step);
    }
}