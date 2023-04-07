using System;
using System.Collections;
using System.Linq;
using UnityEngine;

public class PlayerMovement : MonoBehaviour{
    [SerializeField] private float speed = 5f;
    [SerializeField] private float jumpForce = 3f;
    [SerializeField] private float dashDistance = 3f;
    [SerializeField] private float dashSpeed = 10f;
    [SerializeField] private float dashCancelCollisionAngleThreshold = 30f;
    [SerializeField] private int maxJumps = 2;
    [SerializeField] private float wallFallSpeed = -2f;
    [SerializeField] private float wallJumpTime = 1f;
    [SerializeField] private float wallJumpAngle = 30f;
    [SerializeField] private float wallJumpForce = 5f;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private MagicShield magicShield;
    [SerializeField] private Camera gameCamera;
    [SerializeField] private ContactFilter2D groundContactFilter;
    [SerializeField] private ContactFilter2D wallContactFilter;
    public bool onPlatform;

    private readonly Collider2D[] contacts = new Collider2D[3];
    private readonly Collider2D[] wallContacts = new Collider2D[1];
    private WallCheck checkWall;
    private Coroutine dashCoroutine;
    private Vector2 dashDirection;
    private float dashDuration;
    private bool executeDash;
    private bool executeJump;
    private float gravityScale;
    private bool isDashing;
    private bool isWallJumping;
    private Rigidbody2D myRigidbody2D;
    private Transform myTransform;

    public bool Grounded{ get; private set; }

    private void Awake(){
        myTransform = transform;
        myRigidbody2D = GetComponent<Rigidbody2D>();
        gravityScale = myRigidbody2D.gravityScale;
        dashDuration = dashDistance / dashSpeed;
    }

    private void FixedUpdate(){
        Grounded = CheckGround();
        checkWall = CheckWall();

        if (executeDash){
            dashCoroutine = StartCoroutine(DashCoroutine());
            executeDash = false;
        }

        if (isDashing){ return; }

        if (executeJump){
            Vector3 jumpDirection = Vector2.up;
            float rotateJumpDirection = 0;
            switch (checkWall){
                case WallCheck.None:
                    break;
                case WallCheck.Left:
                    OnWallJump();
                    rotateJumpDirection = wallJumpAngle;
                    break;
                case WallCheck.Right:
                    OnWallJump();
                    rotateJumpDirection = -wallJumpAngle;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            float jumpFinalForce = checkWall != WallCheck.None ? wallJumpForce : jumpForce;
            jumpDirection = Quaternion.AngleAxis(rotateJumpDirection, Vector3.back) * jumpDirection;
            myRigidbody2D.AddForce(jumpDirection * jumpFinalForce, ForceMode2D.Impulse);
            executeJump = false;
            return;
        }

        if (checkWall == WallCheck.None && !isWallJumping){
            Vector2 velocity = myRigidbody2D.velocity;
            velocity.x = PlayerInput.MoveInput.x * speed;
            if (Math.Abs(velocity.x) > 0){ spriteRenderer.flipX = velocity.x < 0; }
            myRigidbody2D.velocity = velocity;
            return;
        }

        if (!Grounded && checkWall != WallCheck.None){
            myRigidbody2D.gravityScale = 0;
            Vector2 velocity = myRigidbody2D.velocity;
            velocity.x = 0;
            velocity.y = wallFallSpeed;
            myRigidbody2D.velocity = velocity;
        }
    }

    private void OnEnable(){
        // Subscribe to input events
        PlayerInput.OnJumpInput += OnJump;
        PlayerInput.OnDashInput += OnDash;
    }

    private void OnCollisionEnter2D(Collision2D col){
        if (!isDashing){ return; }
        if (col.contacts.Any(contact =>
                Vector2.Angle(contact.normal, dashDirection) > 180f - dashCancelCollisionAngleThreshold)){
            CancelDash();
        }
    }

    private IEnumerator DashCoroutine(){
        isDashing = true;
        if (dashDirection.sqrMagnitude == 0){ dashDirection.x = spriteRenderer.flipX ? -1 : 1; }
        if (Grounded && Vector2.Angle(dashDirection, Vector2.down) < 180f - dashCancelCollisionAngleThreshold){
            dashDirection.x = spriteRenderer.flipX ? -1 : 1;
        }
        myRigidbody2D.gravityScale = 0f;
        myRigidbody2D.velocity = dashDirection * dashSpeed;
        yield return new WaitForSeconds(dashDuration);
        CancelDash();
    }

    private void OnJump(){
        if (!Grounded && !onPlatform && checkWall == WallCheck.None){ return; }
        executeJump = true;
    }

    private void OnWallJump(){
        myRigidbody2D.gravityScale = gravityScale;
        StartCoroutine(WallJumpCoroutine());
    }

    public void OnDash(){
        if (isDashing){ return; }
        executeDash = true;
        dashDirection = PlayerInput.MoveInput;
    }

    private bool CheckGround(){
        return myRigidbody2D.GetContacts(groundContactFilter, contacts) > 0;
    }

    private WallCheck CheckWall(){
        if (isWallJumping){ return WallCheck.None; }
        if (PlayerInput.MoveInput.x < 0 && myRigidbody2D.GetContacts(wallContactFilter, wallContacts) > 0){
            if (isDashing){ CancelDash(); }
            return WallCheck.Left;
        }
        ContactFilter2D leftSideFilter = wallContactFilter;
        leftSideFilter.minNormalAngle = 180f + wallContactFilter.minNormalAngle;
        leftSideFilter.maxNormalAngle = 180f + wallContactFilter.maxNormalAngle;
        if (PlayerInput.MoveInput.x > 0 && myRigidbody2D.GetContacts(leftSideFilter, wallContacts) > 0){
            if (isDashing){ CancelDash(); }
            return WallCheck.Right;
        }
        if (!isDashing){ myRigidbody2D.gravityScale = gravityScale; }
        return WallCheck.None;
    }

    private IEnumerator WallJumpCoroutine(){
        isWallJumping = true;
        yield return new WaitForSeconds(wallJumpTime);
        isWallJumping = false;
    }


    private void CancelDash(){
        myRigidbody2D.gravityScale = gravityScale;
        isDashing = false;
        if (dashCoroutine != null){ StopCoroutine(dashCoroutine); }
    }

    public void ForceDash(Vector2 direction){
        CancelDash();
        dashDirection = direction;
        executeDash = true;
    }
}