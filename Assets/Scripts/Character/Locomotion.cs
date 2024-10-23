using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Scripting.APIUpdating;

[RequireComponent(typeof(CharacterController), typeof(PlayerInput), typeof(Animator))]
public class Locomotion : MonoBehaviour
{
    private CharacterController _characterController;

    public float Speed = 5.0f;
    public float AngularSensitivity = 50f;
    public float PanSensitivity = 30f;
    public Vector2 m_Rotation;
    public Camera MainCamera;
    private float _cameraToCharacterDistance;

    private PlayerInput _inputAction;
    private Animator _playerAnimator;
    private float _moveXDuration = 0f;
    private float _moveYDuration = 0f;

    private void Awake()
    {
        _characterController = GetComponent<CharacterController>();
        _inputAction = GetComponent<PlayerInput>();
        _playerAnimator = GetComponent<Animator>();
        if(MainCamera == null)
            MainCamera = Camera.main;
        _cameraToCharacterDistance = Vector3.Distance(transform.position, MainCamera.transform.position);
    }

    // Update is called once per frame
    void Update()
    {
        if (_inputAction == null)
            return;
        Move(_inputAction.actions["Move"].ReadValue<Vector2>());
        Look(_inputAction.actions["Look"].ReadValue<Vector2>());
    }
    private void Move(Vector2 dir)
    {
        if (dir.sqrMagnitude < 0.1f)
        {
            _moveYDuration = 0;
            _playerAnimator.SetBool("is Moving", false);
            return;
        }
        else
            _playerAnimator.SetBool("is Moving", true);
        _playerAnimator.SetFloat("Move X", dir.x);
        if (dir.y < 0)
        {
            _playerAnimator.SetFloat("Move Y", -1f);
        }
        else if(dir.y > 0)
        {
            _moveYDuration += Time.deltaTime;
            if(_moveXDuration < 1f)
                _playerAnimator.SetFloat("Move Y", 0.5f);
            else if (_moveXDuration < 2f)
                _playerAnimator.SetFloat("Move Y", 1f);
            else
                _playerAnimator.SetFloat("Move Y", 2f);
        }
        else
        {
            _moveYDuration = 0;
        }
        float scaledMoveSpeed = Speed * Time.deltaTime;
        // For simplicity's sake, we just keep movement in a single plane here. Rotate
        // direction according to world Y rotation of player.
        Vector3 move = Quaternion.Euler(0, MainCamera.transform.eulerAngles.y, 0) * new Vector3(dir.x, 0, dir.y);
        transform.position += move * scaledMoveSpeed;
    }
    private void Look(Vector2 rotate)
    {
        if (rotate.sqrMagnitude < 0.01)
            return;
        m_Rotation.x += rotate.y * PanSensitivity * Time.deltaTime;
        m_Rotation.y += rotate.x * AngularSensitivity * Time.deltaTime;
        m_Rotation.x = Mathf.Clamp(m_Rotation.x, 25, 80);
        m_Rotation.y = Mathf.Clamp(m_Rotation.y, 0, 360);
        MainCamera.transform.position = transform.position + _cameraToCharacterDistance * (Quaternion.Euler(m_Rotation.x, 0f, 0f) * Vector3.up);
        MainCamera.transform.RotateAround(transform.position, Vector3.up, m_Rotation.y);
        MainCamera.transform.LookAt(transform.position + new Vector3(0, 1f, 0f));

        //m_Rotation.x = Mathf.Clamp(m_Rotation.x - rotate.y * scaledRotateSpeed, -89, 89);
        transform.localEulerAngles = new Vector3(0f, m_Rotation.y + 180, 0f);
    }
}
