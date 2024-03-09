using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    [Header("Input")]
    [SerializeField] private InputReader _inputReader;
    [Header("Movement")]
    [SerializeField] private float _walkSpeed = 2f;
    [SerializeField] private float _gravityValue = -9.81f;
    [Header("Camera")]
    [SerializeField] private Transform _camera;
    [SerializeField, Range(1f, 100f)] private float _sensitivity = 60f;
    [SerializeField] private float _maxLookUpAngle = 75f;
    [SerializeField] private float _maxLookDownAngle = 85f;

    private CharacterController _characterController;

    private Vector3 _playerVelocity;
    private Vector2 _moveInputVector = Vector2.zero;

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
        Move();
        Gravity();
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

    private void Move()
    {
        if (_characterController.isGrounded && _playerVelocity.y < 0f)
            _playerVelocity.y = 0f;

        Vector3 forward = transform.TransformDirection(Vector3.forward);
        Vector3 right = transform.TransformDirection(Vector3.right);

        float currentVelocityX = _walkSpeed * _moveInputVector.x;
        float currentVelocityZ = _walkSpeed * _moveInputVector.y;

        Vector3 moveVector = new Vector3(currentVelocityX, 0f, currentVelocityZ);

        if (moveVector.magnitude > 0f) moveVector = (forward * currentVelocityZ) + (right * currentVelocityX);

        _characterController.Move(Time.deltaTime * moveVector);
    }

    private void Gravity()
    {
        _playerVelocity.y += (_gravityValue) * Time.deltaTime;
        _characterController.Move(_playerVelocity * Time.deltaTime);
    }
}
