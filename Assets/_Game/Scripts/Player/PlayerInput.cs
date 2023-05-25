using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInput : MonoBehaviour, GameInput.IPlayerActions{
    private GameInput gameInput;
    public static event Action<int> OnChangeShieldColor;
    public static event Action OnDashInput;
    public static event Action OnFireInput;
    public static event Action<bool> OnShieldToggle;
    public static event Action OnPlayerInteract;
    public static event Action<bool> OnJumpInput;

    public static Vector2 MoveInput{ get; private set; }
    public static float LookAngle{ get; private set; }
    public static Vector2 LookDirection{ get; private set; }

    private void OnEnable(){
        if (gameInput == null){
            gameInput = new GameInput();
            gameInput.Player.SetCallbacks(this);
        }
        gameInput.Enable();
    }

    private void OnDisable(){
        gameInput.Disable();
    }


    public void OnMove(InputAction.CallbackContext context){
        MoveInput = context.ReadValue<Vector2>().normalized;
    }

    public void OnLook(InputAction.CallbackContext context){
        Vector3 myPosition = transform.position;
        Vector3 targetDirection = context.ReadValue<Vector2>();
        Camera gameCamera = Player.Instance.PlayerCamera;
        targetDirection.z = Mathf.Abs(gameCamera.transform.position.z - myPosition.z);
        targetDirection = gameCamera.ScreenToWorldPoint(targetDirection);
        LookDirection = (targetDirection - myPosition).normalized;
        LookAngle = Vector2.SignedAngle(Vector2.right, LookDirection);
    }

    public void OnFire(InputAction.CallbackContext context){
        if (!PlayerMovement.canMove){ return; }
        if (context.performed){ OnFireInput?.Invoke(); }
    }

    public void OnJump(InputAction.CallbackContext context){
        if (context.action.WasPressedThisFrame()){ OnJumpInput?.Invoke(true); }
        if (context.action.WasReleasedThisFrame()){ OnJumpInput?.Invoke(false); }
    }

    public void OnDash(InputAction.CallbackContext context){
        if (!PlayerMovement.canMove){ return; }
        if (context.performed){ OnDashInput?.Invoke(); }
    }

    public void OnSetColor1(InputAction.CallbackContext context){
        if (context.performed){ OnChangeShieldColor?.Invoke(0); }
    }

    public void OnSetColor2(InputAction.CallbackContext context){
        if (context.performed){ OnChangeShieldColor?.Invoke(1); }
    }

    public void OnSetColor3(InputAction.CallbackContext context){
        if (context.performed){ OnChangeShieldColor?.Invoke(2); }
    }

    public void OnZoomOut(InputAction.CallbackContext context){
        // Vector2 zoomPosition  = 
        // if (zoomCoroutine != null){ }
        // zoomCoroutine = new
    }

    public void OnZoomIn(InputAction.CallbackContext context){
        // throw new NotImplementedException();
    }

    public void OnInteract(InputAction.CallbackContext context){
        if (context.performed){ OnPlayerInteract?.Invoke(); }
    }

    public void OnShield(InputAction.CallbackContext context){
        if (!PlayerMovement.canMove){ return; }
        if (context.action.WasPressedThisFrame()){ OnShieldToggle?.Invoke(true); }
        if (context.action.WasReleasedThisFrame()){ OnShieldToggle?.Invoke(false); }
    }

    public void OnUseElement(InputAction.CallbackContext context){
        if (context.performed){ Player.Instance.ElementMagic.UseMagicElement(); }
    }

    public void OnPause(InputAction.CallbackContext context){
        if (context.performed){ GameManager.TogglePause(); }
    }
}