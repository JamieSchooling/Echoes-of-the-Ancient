using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    [Header("Input")]
    [SerializeField] private InputReader _inputReader;
    [Header("Movement")]
    [SerializeField] private float _walkSpeed = 2f;
    [SerializeField] private float _gravityValue = -9.81f;
    [SerializeField] private float _jumpForce = 5f;
    [Header("Camera")]
    [SerializeField] private Transform _camera;
    [SerializeField, Range(1f, 100f)] private float _sensitivity = 60f;
    [SerializeField] private float _maxLookUpAngle = 75f;
    [SerializeField] private float _maxLookDownAngle = 85f;

    private CharacterController _characterController;

    private Vector3 _playerVelocity;
    private Vector2 _moveInputVector = Vector2.zero;
    private bool _isJumpPressed = false;

    private Vector2 _lookInputVector = Vector2.zero;
    private float _rotationX = 0f;
    private float _rotationY = 0f;

    public float CameraSensitivity
    {
        get => _sensitivity;
        set => _sensitivity = value;
    }

    private void Awake()
    {
        _characterController = GetComponent<CharacterController>();
        _inputReader.OnInputMove += (vector) => _moveInputVector = vector;
        _inputReader.OnInputLook += (vector) => _lookInputVector = vector;
        _inputReader.OnJumpPressed += () => _isJumpPressed = true;
        _inputReader.OnJumpReleased += () => _isJumpPressed = false;
    }

    private void OnEnable()
    {
        _inputReader.EnablePlayer();
    }

    private void OnDisable()
    {
        _inputReader.DisablePlayer();
    }

    private void Update()
    {
        Look();
        Gravity();
        Move();
    }

    private void Look()
    {
        float lookSpeed = _sensitivity / 200f;
        _rotationY += _lookInputVector.x * lookSpeed;
        _rotationX += -_lookInputVector.y * lookSpeed;
        _rotationX = Mathf.Clamp(_rotationX, -_maxLookUpAngle, _maxLookDownAngle);
        transform.rotation = Quaternion.Euler(0f, _rotationY, 0f);
        _camera.transform.rotation = Quaternion.Euler(_rotationX, _rotationY, 0f);
    }

    private void Gravity()
    {
        _playerVelocity.y += (_gravityValue) * Time.deltaTime;

        if (_isJumpPressed && _characterController.isGrounded) _playerVelocity.y = _jumpForce;
    }

    private void Move()
    {
        if (_characterController.isGrounded && _playerVelocity.y < 0f)
            _playerVelocity.y = 0f;

        Vector3 forward = transform.TransformDirection(Vector3.forward);
        Vector3 right = transform.TransformDirection(Vector3.right);

        _playerVelocity.x = _walkSpeed * _moveInputVector.x;
        _playerVelocity.z = _walkSpeed * _moveInputVector.y;

        Vector3 moveVector = new Vector3(_playerVelocity.x, 0f, _playerVelocity.z);

        if (moveVector.magnitude > 0f) moveVector = (forward * _playerVelocity.z) + (right * _playerVelocity.x);
        _playerVelocity.x = moveVector.x;
        _playerVelocity.z = moveVector.z;

        _characterController.Move(Time.deltaTime * _playerVelocity);
    }

    
}
