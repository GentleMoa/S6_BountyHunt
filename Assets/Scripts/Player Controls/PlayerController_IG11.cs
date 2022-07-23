using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController_IG11 : MonoBehaviour
{
    //Private Variables
    private Rigidbody _rigidbody;
    private FixedJoystick _joystick;
    private Animator _animator;
    private Camera _arCamera;
    private Vector3 _lastRotation;
    private Vector3 _playerMovementVector;
    private float _playerMovementSpeed;

    // Start is called before the first frame update
    void Start()
    {
        _joystick = FixedJoystick.FindObjectOfType<FixedJoystick>();
        _rigidbody = GetComponent<Rigidbody>();
        _animator = GetComponent<Animator>();
        //This might become a problem later in multiplayer, when there are more than one MainCameras in the scene
        _arCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //Walking
        if (_joystick.Horizontal != 0 || _joystick.Vertical != 0)
        {
            _playerMovementSpeed = 0.25f;

            //If using animator bools, set them here!
            _animator.SetBool("isWalking", true);
            _animator.SetBool("Idling", false);

            //saving the last rotation, while moving
            _lastRotation = _rigidbody.velocity;

            //Running
            if (_joystick.Horizontal > 0.7 || _joystick.Vertical > 0.7)
            {
                //Setting the walking speed
                _playerMovementSpeed = 0.35f;

                //If using animator bools, set them here!
            }

            _playerMovementVector = new Vector3(_joystick.Horizontal * _playerMovementSpeed, 0.0f, _joystick.Vertical * _playerMovementSpeed);

            Vector3 camForward = _arCamera.transform.forward;
            camForward.y = 0.0f;
            Quaternion camRotationFlattened = Quaternion.LookRotation(camForward);
            _playerMovementVector = camRotationFlattened * _playerMovementVector;

            //moving the character in the direction of the joystick input
            _rigidbody.velocity = _playerMovementVector;

            //rotating the character in the direction of the joystick input
            transform.rotation = Quaternion.LookRotation(_rigidbody.velocity);
        }
        //Idling
        else if (_joystick.Horizontal == 0 || _joystick.Vertical == 0)
        {
            //If using animator bools, set them here!
            _animator.SetBool("Idling", true);
            _animator.SetBool("isWalking", false);

            //copying the last rotation, while standing still
            transform.rotation = Quaternion.LookRotation(_lastRotation);
        }
    }
}
