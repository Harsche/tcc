using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInput : MonoBehaviour,  GameInput.IPlayerActions{
    private GameInput gameInput;

    public static Vector2 MoveDirection{ get; private set; }
    public static bool DashInput{ get; private set; }
    
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
        throw new NotImplementedException();
    }

    public void OnLook(InputAction.CallbackContext context){
        throw new NotImplementedException();
    }

    public void OnFire(InputAction.CallbackContext context){
        throw new NotImplementedException();
    }

    public void OnJump(InputAction.CallbackContext context){
        throw new NotImplementedException();
    }

    public void OnDash(InputAction.CallbackContext context){
        throw new NotImplementedException();
    }

    public void OnSetColor1(InputAction.CallbackContext context){
        throw new NotImplementedException();
    }

    public void OnSetColor2(InputAction.CallbackContext context){
        throw new NotImplementedException();
    }

    public void OnSetColor3(InputAction.CallbackContext context){
        throw new NotImplementedException();
    }

    public void OnZoomOut(InputAction.CallbackContext context){
        throw new NotImplementedException();
    }

    public void OnZoomIn(InputAction.CallbackContext context){
        throw new NotImplementedException();
    }
}