using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController_IG11_V2 : MonoBehaviour
{
    //Private Variables
    private Rigidbody _rigidbody;
    private FixedJoystick _joystick;
    private Animator _animator;
    private Camera _arCamera;
    private Vector3 _lastRotation;
    private Vector3 _playerMovementVector;

    //Serialized Variables
    [SerializeField] private float playerMovementSpeed;

    void Start()
    {
        //All of these might become a problem later in multiplayer, if they exist twice (once for each player)!
        _joystick = FixedJoystick.FindObjectOfType<FixedJoystick>();
        _rigidbody = GetComponent<Rigidbody>();
        _animator = GetComponent<Animator>();
        _arCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();

        if (!_joystick || !_rigidbody || !_animator || !_arCamera)
        {
            Debug.Log("A 'PlayerController_IG11_V2' reference could not be established!");
        }
    }

    void FixedUpdate()
    {
        //Joystick Input Movement
        if (_joystick.Horizontal != 0 || _joystick.Vertical != 0)
        {
            //For Debugging
            //Debug.Log("Joystick Horizontal: " + _joystick.Horizontal);
            //Debug.Log("Joystick Vertical: " + _joystick.Vertical);

            //Animation blend-tree handling (using contextual joystick input axis value to influence animation blend-tree)
            //If both axis values are positive ( > 0 )
            if (_joystick.Horizontal > 0 && _joystick.Vertical > 0)
            {
                //If Horizontal is greater than Vertical
                if (_joystick.Horizontal > _joystick.Vertical)
                {
                    //Use Horizontal as JoystickInput
                    _animator.SetFloat("JoystickInput", _joystick.Horizontal);
                }
                //If Vertical is greater than Horizontal
                else if (_joystick.Vertical > _joystick.Horizontal)
                {
                    //Use Vertical as JoystickInput
                    _animator.SetFloat("JoystickInput", _joystick.Vertical);
                }
            }
            //If both axis values are negative ( < 0 )
            else if (_joystick.Horizontal < 0 && _joystick.Vertical < 0)
            {
                //If Horizontal is smaller than Vertical
                if (_joystick.Horizontal < _joystick.Vertical)
                {
                    //Use Horizontal as JoystickInput
                    _animator.SetFloat("JoystickInput", _joystick.Horizontal);
                }
                //If Vertical is smaller than Horizontal
                else if (_joystick.Vertical < _joystick.Horizontal)
                {
                    //Use Vertical as JoystickInput
                    _animator.SetFloat("JoystickInput", _joystick.Vertical);
                }
            }
            //If the Horizontal is positive ( > 0 ) and the Vertical is negative ( < 0 )
            else if (_joystick.Horizontal > 0 && _joystick.Vertical < 0)
            {
                //If Horizontal is greater than Vertical (without taking the algebraic sign into account)
                if (_joystick.Horizontal > (_joystick.Vertical * -1))
                {
                    //Use Horizontal as JoystickInput
                    _animator.SetFloat("JoystickInput", _joystick.Horizontal);
                }
                //If Horizontal is smaller than Vertical (without taking the algebraic sign into account)
                else if (_joystick.Vertical < (_joystick.Vertical * -1))
                {
                    //Use Vertical as JoystickInput
                    _animator.SetFloat("JoystickInput", _joystick.Vertical);
                }
            }
            //If the Horizontal is negative ( < 0 ) and the Vertical is positive ( > 0 )
            else if (_joystick.Horizontal < 0 && _joystick.Vertical > 0)
            {
                //If Horizontal is greater than Vertical (without taking the algebraic sign into account)
                if ((_joystick.Horizontal * -1) > _joystick.Vertical)
                {
                    //Use Horizontal as JoystickInput
                    _animator.SetFloat("JoystickInput", _joystick.Horizontal);
                }
                //If Horizontal is smaller than Vertical (without taking the algebraic sign into account)
                else if ((_joystick.Vertical * -1) < _joystick.Vertical)
                {
                    //Use Vertical as JoystickInput
                    _animator.SetFloat("JoystickInput", _joystick.Vertical);
                }
            }

            //saving the last rotation, while moving
            _lastRotation = _rigidbody.velocity;

            _playerMovementVector = new Vector3(_joystick.Horizontal * playerMovementSpeed, 0.0f, _joystick.Vertical * playerMovementSpeed);

            Vector3 camForward = _arCamera.transform.forward;
            camForward.y = 0.0f;
            Quaternion camRotationFlattened = Quaternion.LookRotation(camForward);
            _playerMovementVector = camRotationFlattened * _playerMovementVector;

            //moving the character in the direction of the joystick input
            _rigidbody.velocity = _playerMovementVector;

            //rotating the character in the direction of the joystick input
            transform.rotation = Quaternion.LookRotation(_rigidbody.velocity);
        }
        else if (_joystick.Horizontal == 0 || _joystick.Vertical == 0) // ---> Idling
        {
            //Animation blend-tree handling (using any joystick input axis value - they are both 0 - to achieve a rest pose in the blend-tree)
            _animator.SetFloat("JoystickInput", _joystick.Horizontal);

            //copying the last rotation, while standing still
            transform.rotation = Quaternion.LookRotation(_lastRotation);
        }
    }
}
