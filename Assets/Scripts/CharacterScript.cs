using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterScript : MonoBehaviour
{
    [SerializeField]
    private float _speed;
    private CharacterController _character_controller;

    void Start()
    {
        _character_controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        float dx = Input.GetAxis("Horizontal");
        float dz = Input.GetAxis("Vertical");

        if (Mathf.Abs(dx) > 0f || Mathf.Abs(dz) > 0f)
        {
            dx /= Mathf.Sqrt(2f);
            dz /= Mathf.Sqrt(2f);

            Vector3 move_direction = (dz * Camera.main.transform.forward + dx * Camera.main.transform.right).normalized;

            if (move_direction != Vector3.zero)
            {
                Quaternion to_rotation = Quaternion.LookRotation(move_direction, Vector3.up);

                transform.rotation = Quaternion.Lerp(transform.rotation, to_rotation, Time.deltaTime * 10f);
            }

            _character_controller.SimpleMove(Time.deltaTime * _speed * move_direction);
        }
    }
}