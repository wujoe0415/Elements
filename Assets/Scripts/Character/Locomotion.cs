using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Scripting.APIUpdating;

[RequireComponent(typeof(CharacterController), typeof(PlayerInput), typeof(Animator))]
public class Locomotion : MonoBehaviour
{
    private CharacterController _characterController;

    public float Speed = 5.0f;
    public float RunSpeed = 8.0f;
    public float AngularSensitivity = 50f;
    public float JumpHeight = 3f;
    private float _gravityValue = -9.81f;
    private Vector3 _velocity = Vector3.zero;

    private PlayerInput _inputAction;
    private Animator _playerAnimator;
    private float _moveXDuration = 0f;
    [SerializeField]
    private float _moveYDuration = 0f;

    [Header("Player Camera")]
    public float PanSensitivity = 30f;
    public Vector2 m_Rotation = new Vector2(50f, 180f);
    public Camera MainCamera;
    public Transform TargetPosition;
    public LayerMask CollisionLayer;
    public LayerMask IgnoreLayer;
    private float _cameraToCharacterDistance;

    private void Awake()
    {
        _characterController = GetComponent<CharacterController>();
        _inputAction = GetComponent<PlayerInput>();
        _playerAnimator = GetComponent<Animator>();
        if (MainCamera == null)
            MainCamera = Camera.main;
        _cameraToCharacterDistance = Vector3.Distance(transform.position, MainCamera.transform.position);
        _inputAction.actions["Jump"].performed += ctx => Jump();
    }

    // Update is called once per frame
    void Update()
    {
        if (_inputAction == null)
            return;
        if (_characterController.isGrounded && _velocity.y < 0)
            _velocity.y = 0f;
        _velocity.y += _gravityValue * Time.deltaTime;
        Move(_inputAction.actions["Move"].ReadValue<Vector2>());
        Look(_inputAction.actions["Look"].ReadValue<Vector2>());
        _characterController.Move(_velocity * Time.deltaTime);
        if (Input.GetKeyDown(KeyCode.K))
            _playerAnimator.SetTrigger("Turn Right");
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
        float speed = Speed;
        if (dir.y < 0)
        {
            _playerAnimator.SetFloat("Move Y", -1f);
        }
        else if (dir.y > 0)
        {
            _moveYDuration += Time.deltaTime;
            if (_moveYDuration > 2f)
                speed = RunSpeed;

            _playerAnimator.SetFloat("Move Y", Mathf.Clamp(_moveYDuration, 0f, 2f));
        }
        else
        {
            _moveYDuration = 0;
        }
        float scaledMoveSpeed = speed * Time.deltaTime;
        Vector3 move = Quaternion.Euler(0, MainCamera.transform.eulerAngles.y, 0) * new Vector3(dir.x, 0, dir.y);

        _characterController.Move(move * scaledMoveSpeed);
    }
    private void Look(Vector2 rotate)
    {
        if (rotate.sqrMagnitude < 0.01)
            return;
        m_Rotation.x += rotate.y * PanSensitivity * Time.deltaTime;
        m_Rotation.y += rotate.x * AngularSensitivity * Time.deltaTime;
        m_Rotation.x = Mathf.Clamp(m_Rotation.x, 25, 110);
        //m_Rotation.y = Mathf.Clamp(m_Rotation.y, 0, 360);
        RaycastHit hit;
        float distance = _cameraToCharacterDistance;
        if (Physics.Linecast(transform.position, transform.position + _cameraToCharacterDistance * (Quaternion.Euler(m_Rotation.x, 0f, 0f) * Vector3.up), out hit, CollisionLayer))
            distance = Mathf.Min(distance, hit.distance);

        MainCamera.transform.position = transform.position + distance * (Quaternion.Euler(m_Rotation.x, 0f, 0f) * Vector3.up);
        MainCamera.transform.RotateAround(transform.position, Vector3.up, m_Rotation.y);
        MainCamera.transform.LookAt(TargetPosition.position + new Vector3(0, 1f, 0f));

        //m_Rotation.x = Mathf.Clamp(m_Rotation.x - rotate.y * scaledRotateSpeed, -89, 89);
        transform.localEulerAngles = new Vector3(0f, m_Rotation.y + 180, 0f);

        bool isTurnRight = rotate.x > 0;
        //Debug.Log(rotate.x * AngularSensitivity * Time.deltaTime);
        if (Mathf.Abs(rotate.x) * AngularSensitivity * Time.deltaTime > 2.2f)
        {
            _playerAnimator.SetBool(isTurnRight ? "Turn Right" : "Turn Left", true);
            _playerAnimator.SetBool(!isTurnRight ? "Turn Right" : "Turn Left", false);
        }
        else
        {
            _playerAnimator.SetBool("Turn Right", false);
            _playerAnimator.SetBool("Turn Left", false);
        }


        // Clamp Rotation y
        if (m_Rotation.y < 0)
            m_Rotation.y = 360 + m_Rotation.y;
        else if (m_Rotation.y > 360)
            m_Rotation.y = m_Rotation.y - 360;
    }
    private void Jump()
    {
        if (!isGrounded())
            return;
        _velocity.y += Mathf.Sqrt(JumpHeight * -1.0f * _gravityValue);
        _playerAnimator.SetTrigger("Jump");
    }
    public bool isGrounded()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, 0.4f, ~IgnoreLayer/*CollisionLayer*/))
            return true;
        else
            return false;
    }
    public bool isIdle
    {
        get { return _playerAnimator.GetCurrentAnimatorStateInfo(0).IsName("Idle"); }
    }
    public bool isRun
    {
        get
        {
            return _moveYDuration > 2f;
        }
    }
    public void Fly(float height)
    {
        _velocity.y += Mathf.Sqrt(height * -1.0f * _gravityValue);
        _playerAnimator.SetTrigger("Jump");
    }
}
