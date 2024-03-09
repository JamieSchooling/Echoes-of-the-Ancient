using System;
using UnityEngine;
using UnityEngine.InputSystem;

[CreateAssetMenu(menuName = "Scriptable Objects/Input/Input Reader")]
public class InputReader : ScriptableObject, GameInput.IPlayerActions
{
    public event Action<Vector2> OnInputMove;
    public event Action<Vector2> OnInputLook;
    public event Action OnJumpPressed;
    public event Action OnJumpReleased;

    private GameInput _gameInput;
    
    private void OnEnable()
    {
        _gameInput ??= new GameInput();

        _gameInput.Player.SetCallbacks(this);
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        OnInputMove?.Invoke(context.ReadValue<Vector2>());
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        OnInputLook?.Invoke(context.ReadValue<Vector2>());
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed) OnJumpPressed?.Invoke();
        if (context.phase == InputActionPhase.Canceled) OnJumpReleased?.Invoke();
    }

    public void EnableAll()
    {
        EnablePlayer();
    }

    public void DisableAll()
    {
        DisablePlayer();
    }

    public void EnablePlayer()
    {
        DisableCursor();
        _gameInput.Player.Enable();
    }
    
    public void DisablePlayer()
    {
        _gameInput.Player.Enable();
    }

    public void EnableCursor()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;
    }

    public void DisableCursor()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }
}
