using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    [Header("Input")]
    [SerializeField] private InputReader _inputReader;
    [Header("Camera")]
    [SerializeField] private Transform _camera;
    [SerializeField, Range(1f, 100f)] private float _sensitivity = 60f;
    [SerializeField] private float _maxLookUpAngle = 75f;
    [SerializeField] private float _maxLookDownAngle = 85f;

    private CharacterController _characterController;

    private Vector2 _lookInputVector = Vector2.zero;
    private float _rotationX = 0;
    private float _rotationY = 0;

    public float CameraSensitivity
    {
        get => _sensitivity;
        set => _sensitivity = value;
    }

    private void Awake()
    {
        _characterController = GetComponent<CharacterController>();
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
    }

    private void Look()
    {
        float lookSpeed = _sensitivity / 200f;
        _rotationY += _lookInputVector.x * lookSpeed;
        _rotationX += -_lookInputVector.y * lookSpeed;
        _rotationX = Mathf.Clamp(_rotationX, -_maxLookUpAngle, _maxLookDownAngle);
        _camera.transform.rotation = Quaternion.Euler(_rotationX, _rotationY, 0);
    }
}
