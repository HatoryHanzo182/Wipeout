using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.UI;

public class GhostScript : MonoBehaviour
{
    private Animator _animator;
    private CharacterController _ctr;
    private Vector3 _move_direction = Vector3.zero;
    private bool _is_jumping = false;
    private float _jump_force = 8.0f;
    private float _gravity = 30.0f;
    private static readonly int _idle_state = Animator.StringToHash("Base Layer.idle");
    private static readonly int _move_state = Animator.StringToHash("Base Layer.move");
    private static readonly int _surprised_state = Animator.StringToHash("Base Layer.surprised");
    private static readonly int _attack_state = Animator.StringToHash("Base Layer.attack_shift");
    [SerializeField] private float _speed;

    void Start()
    {
        _animator = this.GetComponent<Animator>();
        _ctr = GetComponent<CharacterController>();
        GameState.Stamina = 1f;
        GameState.ChackPointCoord = this.transform.position;
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.LeftShift) && GameState.Stamina > 0)
            GameState.Stamina -= 0.1f * Time.deltaTime;
        else if (GameState.Stamina < 1)
        {
            GameState.Stamina += 0.1f * Time.deltaTime;
            _speed = 15;
        }

        GRAVITY();

        if(!PlayerStatus.ContainsValue( true ))
        {
            MOVE();
            PlayerAttack();
            CheckJumpLand();
        }
        else if(PlayerStatus.ContainsValue( true ))
        {
            int status_name = 0;
            
            foreach(var i in PlayerStatus)
            {
                if(i.Value == true)
                {
                    status_name = i.Key;
                    break;
                }
            }
        }
    }

    //---------------------------------------------------------------------
    // character status
    //---------------------------------------------------------------------
    private const int Dissolve = 1;
    private const int Attack = 2;
    private const int Surprised = 3;
    private Dictionary<int, bool> PlayerStatus = new Dictionary<int, bool>
    {
        {Dissolve, false },
        {Attack, false },
        {Surprised, false },
    };

    // play a animation of Attack
    private void PlayerAttack()
    {
        if (Input.GetMouseButtonDown(0))
            _animator.CrossFade(_attack_state, 0.1f, 0, 0);
    }

    //---------------------------------------------------------------------
    // gravity for fall of this character
    //---------------------------------------------------------------------
    private void GRAVITY()
    {
        if (_ctr.enabled)
        {
            if (!_is_jumping && CheckGrounded())
            {
                if (_move_direction.y < -0.1f)
                    _move_direction.y = -0.1f;

                _is_jumping = false;
            }

            if (!_is_jumping)
                _move_direction.y -= _gravity * Time.deltaTime;

            _ctr.Move(_move_direction * Time.deltaTime);
        }
    }

    //---------------------------------------------------------------------
    // for slime moving
    //---------------------------------------------------------------------
    private void MOVE()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        Vector3 cameraForward = Camera.main.transform.forward;
        Vector3 cameraRight = Camera.main.transform.right;

        cameraForward.y = 0f;
        cameraRight.y = 0f;
        cameraForward.Normalize();
        cameraRight.Normalize();

        Vector3 moveDirection = (horizontalInput * cameraRight + verticalInput * cameraForward).normalized;

        if (moveDirection != Vector3.zero)
        {
            Quaternion toRotation = Quaternion.LookRotation(moveDirection, Vector3.up);
            transform.rotation = Quaternion.Lerp(transform.rotation, toRotation, Time.deltaTime * 10f);
        }

        MOVE_Velocity(_speed * moveDirection, moveDirection);
        KEY_DOWN();
        KEY_UP();
    }


    //---------------------------------------------------------------------
    // value for moving
    //---------------------------------------------------------------------
    private void MOVE_Velocity(Vector3 velocity, Vector3 _move_direction)
    {
        _move_direction = new Vector3(velocity.x, _move_direction.y, velocity.z);
        
        if (_ctr.enabled)
            _ctr.Move(_move_direction * Time.deltaTime);
        
        _move_direction.x = 0;
        _move_direction.z = 0;
    }

    //---------------------------------------------------------------------
    // whether arrow key is key down
    //---------------------------------------------------------------------
    private void KEY_DOWN ()
    {
        if (Input.GetKeyDown(KeyCode.W))
            _animator.CrossFade(_move_state, 0.1f, 0, 0);
        else if (Input.GetKeyDown(KeyCode.S))
            _animator.CrossFade(_move_state, 0.1f, 0, 0);
        else if (Input.GetKeyDown(KeyCode.A))
            _animator.CrossFade(_move_state, 0.1f, 0, 0);
        else if (Input.GetKeyDown(KeyCode.D))
            _animator.CrossFade(_move_state, 0.1f, 0, 0);
        else if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            _speed += 20f;
            _animator.CrossFade(_move_state, 0.1f, 0, 0);
        }
        else if (Input.GetButtonDown("Jump") && CheckGrounded())
        {
            _is_jumping = true;
            _move_direction.y = Mathf.Sqrt(2.0f * _jump_force * _gravity);

            _animator.CrossFade(_surprised_state, 0.1f, 0, 0);
        }
        else if(Input.GetKeyDown(KeyCode.R))
        {
            this.transform.position = GameState.ChackPointCoord;
        }
    }

    //---------------------------------------------------------------------
    // whether arrow key is key up
    //---------------------------------------------------------------------
    private void KEY_UP ()
    {
        if (Input.GetKeyUp(KeyCode.W))
        {
            if (!Input.GetKey(KeyCode.DownArrow) && !Input.GetKey(KeyCode.LeftArrow) && !Input.GetKey(KeyCode.RightArrow))
                _animator.CrossFade(_idle_state, 0.1f, 0, 0);
        }
        else if (Input.GetKeyUp(KeyCode.S))
        {
            if (!Input.GetKey(KeyCode.UpArrow) && !Input.GetKey(KeyCode.LeftArrow) && !Input.GetKey(KeyCode.RightArrow))
            {
                _animator.CrossFade(_idle_state, 0.1f, 0, 0);
            }
        }
        else if (Input.GetKeyUp(KeyCode.A))
        {
            if (!Input.GetKey(KeyCode.UpArrow) && !Input.GetKey(KeyCode.DownArrow) && !Input.GetKey(KeyCode.RightArrow))
            {
                _animator.CrossFade(_idle_state, 0.1f, 0, 0);
            }
        }
        else if (Input.GetKeyUp(KeyCode.D))
        {
            if (!Input.GetKey(KeyCode.UpArrow) && !Input.GetKey(KeyCode.DownArrow) && !Input.GetKey(KeyCode.LeftArrow))
            {
                _animator.CrossFade(_idle_state, 0.1f, 0, 0);
            }
        }
        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            if (!Input.GetKey(KeyCode.UpArrow) && !Input.GetKey(KeyCode.DownArrow) && !Input.GetKey(KeyCode.LeftArrow))
            {
                _speed -= 20f;
                _animator.CrossFade(_idle_state, 0.1f, 0, 0);
            }
        }
    }

    private bool CheckGrounded()
    {
        if (_ctr.isGrounded && _ctr.enabled)
            return true;

        Ray ray = new Ray(this.transform.position + Vector3.up * 0.1f, Vector3.down);
        float range = 0.2f;

        return Physics.Raycast(ray, range);
    }


    private void CheckJumpLand()
    {
        if (_is_jumping && CheckGrounded())
            _is_jumping = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Respawn")
        {
            GameState.ChackPointCoord = other.transform.position;
        }
        else if (other.gameObject.tag == "Floor")
            KEY_DOWN();
    }
}