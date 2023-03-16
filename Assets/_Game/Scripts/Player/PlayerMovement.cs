using System;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour, GameInput.IPlayerActions{
    [SerializeField] private float speed = 5f;
    [SerializeField] private float jumpForce = 3f;
    [SerializeField] private float dashDistance = 3f;
    [SerializeField] private float dashSpeed = 10f;
    [SerializeField] private float dashCancelCollisionAngleThreshold = 30f;
    [SerializeField] private int maxJumps = 2;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private MagicShield magicShield;
    [SerializeField] private Camera gameCamera;
    [SerializeField] private ContactFilter2D groundContactFilter;

    private readonly Collider2D[] contacts = new Collider2D[3];
    private bool checkGround;
    private Coroutine dashCoroutine;
    private Vector2 dashDirection;
    private bool dashInput;
    private GameInput gameInput;
    private bool isDashing;
    private bool jumpInput;
    private Vector2 moveInput;
    private Transform myTransform;
    private new Rigidbody2D rigidbody2D;
    public static float LookAngle{ get; private set; }

    // UNITY METHODS

    private void Awake(){
        myTransform = transform;
        rigidbody2D = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate(){
        checkGround = CheckGround();

        if (dashInput){
            dashCoroutine = StartCoroutine(DashCoroutine());
            dashInput = false;
        }

        if (isDashing){ return; }

        if (jumpInput){
            rigidbody2D.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            jumpInput = false;
        }

        Vector2 velocity = rigidbody2D.velocity;
        velocity.x = moveInput.x * speed;
        if (Math.Abs(velocity.x) > 0){ spriteRenderer.flipX = velocity.x < 0; }
        rigidbody2D.velocity = velocity;
    }

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

    private void OnCollisionEnter2D(Collision2D col){
        if (!isDashing){ return; }
        if (col.contacts.Any(contact =>
                Vector2.Angle(contact.normal, dashDirection) > 180f - dashCancelCollisionAngleThreshold)){
            CancelDash();
        }
    }

    // INPUT METHODS

    public void OnMove(InputAction.CallbackContext context){
        moveInput = context.ReadValue<Vector2>().normalized;
    }

    public void OnLook(InputAction.CallbackContext context){
        Vector3 myPosition = transform.position;
        Vector3 targetDirection = context.ReadValue<Vector2>();;
        targetDirection.z = Mathf.Abs(gameCamera.transform.position.z - myPosition.z);
        targetDirection = gameCamera.ScreenToWorldPoint(targetDirection);
        LookAngle = Vector2.SignedAngle(Vector2.right, targetDirection - myPosition);
        magicShield.RotateShield(LookAngle);
    }

    public void OnFire(InputAction.CallbackContext context){
        if (context.action.WasPressedThisFrame()){
            magicShield.ToggleShield(true);
            return;
        }
        if (context.action.WasReleasedThisFrame()){ magicShield.ToggleShield(false); }
    }

    public void OnJump(InputAction.CallbackContext context){
        jumpInput = context.performed && checkGround;
    }

    public void OnDash(InputAction.CallbackContext context){
        dashInput = !isDashing && context.performed;
    }

    public void OnSetColor1(InputAction.CallbackContext context){
        magicShield.ChangeShieldColor(0);
    }

    public void OnSetColor2(InputAction.CallbackContext context){
        magicShield.ChangeShieldColor(1);
    }

    public void OnSetColor3(InputAction.CallbackContext context){
        magicShield.ChangeShieldColor(2);
    }

    // CUSTOM METHODS

    private IEnumerator DashCoroutine(){
        isDashing = true;
        Vector2 dashStartPosition = myTransform.position;
        dashDirection = moveInput;
        if (dashDirection.sqrMagnitude == 0 ||
            (checkGround && Vector2.Angle(moveInput, Vector2.down) < 180f - dashCancelCollisionAngleThreshold)){
            dashDirection.x = spriteRenderer.flipX ? -1 : 1;
        }
        while (Vector2.Distance(dashStartPosition, myTransform.position) < dashDistance){
            rigidbody2D.velocity = dashDirection * dashSpeed;
            yield return null;
        }
        isDashing = false;
    }

    private bool CheckGround(){
        return rigidbody2D.GetContacts(groundContactFilter, contacts) > 0;
    }

    private void CancelDash(){
        if (dashCoroutine != null){ StopCoroutine(dashCoroutine); }
        isDashing = false;
    }
}