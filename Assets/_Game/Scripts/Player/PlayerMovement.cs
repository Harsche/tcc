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
    public new Rigidbody2D rigidbody2D;

    private readonly Collider2D[] contacts = new Collider2D[3];
    private readonly Collider2D[] wallContacts = new Collider2D[1];
    private bool checkGround;
    private WallCheck checkWall;
    private Coroutine dashCoroutine;
    private Vector2 dashDirection;
    private bool executeDash;
    private bool executeJump;
    private float gravityScale;
    private bool isDashing;
    private bool isWallJumping;
    private Transform myTransform;

    private void Awake(){
        myTransform = transform;
        rigidbody2D = GetComponent<Rigidbody2D>();
        gravityScale = rigidbody2D.gravityScale;
    }

    private void FixedUpdate(){
        checkGround = CheckGround();
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
            rigidbody2D.AddForce(jumpDirection * jumpFinalForce, ForceMode2D.Impulse);
            executeJump = false;
            return;
        }

        if (checkWall == WallCheck.None && !isWallJumping){
            Vector2 velocity = rigidbody2D.velocity;
            velocity.x = PlayerInput.MoveInput.x * speed;
            if (Math.Abs(velocity.x) > 0){ spriteRenderer.flipX = velocity.x < 0; }
            rigidbody2D.velocity = velocity;
            return;
        }

        if (!checkGround && checkWall != WallCheck.None){
            rigidbody2D.gravityScale = 0;
            Vector2 velocity = rigidbody2D.velocity;
            velocity.x = 0;
            velocity.y = wallFallSpeed;
            rigidbody2D.velocity = velocity;
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
        Vector2 dashStartPosition = myTransform.position;
        if (dashDirection.sqrMagnitude == 0){ dashDirection.x = spriteRenderer.flipX ? -1 : 1; }
        if (checkGround && Vector2.Angle(dashDirection, Vector2.down) < 180f - dashCancelCollisionAngleThreshold){
            dashDirection.x = spriteRenderer.flipX ? -1 : 1;
        }
        while (Vector2.Distance(dashStartPosition, myTransform.position) < dashDistance){
            rigidbody2D.velocity = dashDirection * dashSpeed;
            yield return null;
        }
        isDashing = false;
    }

    private void OnJump(){
        if (!checkGround && !onPlatform && checkWall == WallCheck.None){ return; }
        executeJump = true;
    }

    private void OnWallJump(){
        rigidbody2D.gravityScale = gravityScale;
        StartCoroutine(WallJumpCoroutine());
    }

    public void OnDash(){
        if (isDashing){ return; }
        executeDash = true;
        dashDirection = PlayerInput.MoveInput;
    }

    private bool CheckGround(){
        return rigidbody2D.GetContacts(groundContactFilter, contacts) > 0;
    }

    private WallCheck CheckWall(){
        if (isWallJumping){ return WallCheck.None; }
        if (PlayerInput.MoveInput.x < 0 && rigidbody2D.GetContacts(wallContactFilter, wallContacts) > 0){
            return WallCheck.Left;
        }
        ContactFilter2D leftSideFilter = wallContactFilter;
        leftSideFilter.minNormalAngle = 180f + wallContactFilter.minNormalAngle;
        leftSideFilter.maxNormalAngle = 180f + wallContactFilter.maxNormalAngle;
        if (PlayerInput.MoveInput.x > 0 && rigidbody2D.GetContacts(leftSideFilter, wallContacts) > 0){
            return WallCheck.Right;
        }
        rigidbody2D.gravityScale = gravityScale;
        return WallCheck.None;
    }

    private IEnumerator WallJumpCoroutine(){
        isWallJumping = true;
        yield return new WaitForSeconds(wallJumpTime);
        isWallJumping = false;
    }


    private void CancelDash(){
        if (dashCoroutine != null){ StopCoroutine(dashCoroutine); }
        isDashing = false;
    }

    public void ForceDash(Vector2 direction){
        CancelDash();
        dashDirection = direction;
        executeDash = true;
    }
}