using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    //Script Reference
    private CharacterController _characterController;

    //Private variables
    private float _fSpeed;
    private float _fJumpSpeed;
    private float _fVerticalSpeed;
    private float _fMaxVerticalFallSpeed;
    private float _fGravityFactor;
    private float _fOriginalStepOffset;
    private float _fJumpGrace;
    private float _fRotateToMouse;
    private float _fRotationSpeed;
    //the ? is so the field can be nullable
    private float? _fGroundTime;
    private float? _fJumpButtonTime;

    //Forward and Right vectors for isometric movement
    private Vector3 _forward;
    private Vector3 _right;
    private Vector3 _movementDirection;

    //Float variables for axis input
    private float fHorizontalInput;
    private float fVerticalInput;

    //To hold the camera object
    private Camera _camera;

    //Subscribe to events when enabled and remove from events when disabled
    private void OnEnable()
    {
        PlayerManager.onPlayerDeath += DisableMovement;
    }
    private void OnDisable()
    {
        PlayerManager.onPlayerDeath -= DisableMovement;
    }

    // Start is called before the first frame update
    void Start()
    {
        //Get component references
        _characterController = gameObject.GetComponent<CharacterController>();
        _fOriginalStepOffset = _characterController.stepOffset;

        //Setting default values
        _fSpeed = 10f;
        _fJumpSpeed = 10f;
        _fJumpGrace = 0.2f;
        _fGravityFactor = 3f;
        _fRotateToMouse = 0f;
        _fRotationSpeed = 720;
        _fMaxVerticalFallSpeed = -50f;
        
        //Cache the camera
        _camera = Camera.main;

        //Vector fix for isometric movement
        _forward = _camera.transform.forward;
        _forward.y = 0f;
        _forward = Vector3.Normalize(_forward);
        _right = Quaternion.Euler(new Vector3(0, 90, 0)) * _forward;
    }

    // Update is called once per frame
    void Update()
    {
        //Reset movement direction from the last frame
        _movementDirection = Vector3.zero;
        //A+D default Horizontal, W+S default Vertical
        fHorizontalInput = Input.GetAxis("Horizontal");
        fVerticalInput = Input.GetAxis("Vertical");

        //Get right/left speed and up/down speed
        Vector3 rightMove = _right * fHorizontalInput * _fSpeed * Time.deltaTime;
        Vector3 upMove = _forward * fVerticalInput * _fSpeed * Time.deltaTime;
        //Normalise vectors so diagonals don't have greater speed
        _movementDirection = Vector3.Normalize(rightMove + upMove);

        //Allowing for delay with jumping after leaving the ground or jumping as you hit the ground
        if (_characterController.isGrounded)
            {
                //Time since game started when the player was on the ground
                _fGroundTime = Time.time;
            }
        if (Input.GetButtonDown("Jump"))
            {
                //Time since game started when the player pressed jump
                _fJumpButtonTime = Time.time;
            }

        //Check to see if the character was on the ground in the grace period
        if (Time.time - _fGroundTime <= _fJumpGrace)
            {
                //reset stepoffset back to original
                _characterController.stepOffset = _fOriginalStepOffset;
                //Jump speed set to less that 0 to stop flucuating speed when grounded set to 0
                _fVerticalSpeed = -0.5f;

                //Check to see if the player pressed the jump button within the grace peroid
                if (Time.time - _fJumpButtonTime <= _fJumpGrace)
                {
                    _fVerticalSpeed = _fJumpSpeed;

                    //reset timers so you can't jump repeatedly
                    _fJumpButtonTime = null;
                    _fGroundTime = null;
                }
            }
        else
            {
                //If character is in the air change step offset to 0, fixes "glitchyness" when jump near objects
                _characterController.stepOffset = 0;
            }

        //If used a skill recently (event), rotate player towards mouse otherwise rotate player towards move direction
        if (_fRotateToMouse > 0)
        {
            _fRotateToMouse -= Time.deltaTime;
            //Rotate player towards the mouse
            Vector3 targetLocation = Vector3.zero;
            Ray screenRay = _camera.ScreenPointToRay(Input.mousePosition);

            //Set the target location to where the ray collides with an object
            if (Physics.Raycast(screenRay, out RaycastHit rayHit, 100, ~(1 << 7)))
            {//Using layer mask to ignore layer 6 where the player is located
                targetLocation = rayHit.point - gameObject.transform.position;
                targetLocation.y = 0f; //To stop the player character from pointing up
            }
                
            Quaternion toRotation = Quaternion.LookRotation(targetLocation);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, _fRotationSpeed * Time.deltaTime); //Speed * Delta time for consistent speed
        }
        //New Vector 3 as diagonal(-1,0,1) would equal Vector3.Zero.  Mathf.Abs doesn't work on Vector3
        else if(new Vector3(Mathf.Abs(_movementDirection.x), Mathf.Abs(_movementDirection.y), Mathf.Abs(_movementDirection.z)) != Vector3.zero)
        {
            _fRotateToMouse = 0;
            //Rotate player towards direction
            Quaternion toRotation = Quaternion.LookRotation(_movementDirection, Vector3.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, _fRotationSpeed * Time.deltaTime);
        }
    
        //Gravity is -9.81 (m/s^2) this will reduce vertical speed by gravity by time
        //increased by gravity factor to make jumps less "floaty"
        _fVerticalSpeed += Physics.gravity.y * Time.deltaTime * _fGravityFactor;
            
        if (_fVerticalSpeed < _fMaxVerticalFallSpeed)
        {
            _fVerticalSpeed = _fMaxVerticalFallSpeed;
        }

        //Set speed in the y direction and add it to the vector then move using the character controller
        _movementDirection.y = _fVerticalSpeed / _fSpeed;
        _characterController.Move(_movementDirection * _fSpeed * Time.deltaTime);
    }

    //Allow outside scripts to affect movement without use of public variables
    //Allowing change of gravity (faster slower falling) as well as capping fall speed (temp slow fall)
    public void ChangeGravityFactor(float a_fNewGravityFactor)
    {
        _fGravityFactor = a_fNewGravityFactor;
    }
    public void ClampFallSpeed(float a_fMaxFallSpeed)
    {
        if (_fVerticalSpeed < a_fMaxFallSpeed)
        {
            _fVerticalSpeed = a_fMaxFallSpeed;
        }
    }
    //Modifying Speed values
    public void ModifyAddSpeed (float a_fSpeedMod)
    {
        _fSpeed *= a_fSpeedMod;
    }
    public void ModiftyRemoveSpeed (float a_fSpeedMod)
    {
        _fSpeed /= a_fSpeedMod;
    }
    public void UpdateSpeed(float a_fNewSpeed)
    {
        _fSpeed = a_fNewSpeed;
    }
    private void DisableMovement()
    {
        enabled = false;
    }
    //float to satisfy condition, not needed for function
    public void PlayerUsedSkill()
    {
        _fRotateToMouse = 3f;
    }
}
