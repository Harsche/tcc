using System.Collections;
using DG.Tweening;
using UnityEngine;

public class GreenMushroom : MonoBehaviour{
    [SerializeField] private float duration = 6f;
    [SerializeField] private float jumpInputTimeWindow = 0.2f;
    [SerializeField] private float normalJumpHeight = 3f;
    [SerializeField] private float impulsedJumpHeight = 5f;
    [SerializeField] private float impulseVerticalOffset = 0.2f;
    [SerializeField] private float getImpulseDuration = 0.5f;
    [SerializeField] private Ease getImpulseEase = Ease.Linear;
    [SerializeField] private float pushDuration = 0.2f;
    [SerializeField] private Ease pushEase = Ease.OutBack;
    [SerializeField] private Transform bodyTransform;
    [SerializeField] private Vector2 squashScale;
    [SerializeField] private Vector2 stretchScale;
    private BoxCollider2D boxCollider2D;

    private bool impulseJump, isPlayerOn;
    private Coroutine jumpCoroutine;
    private Sequence pushSequence, bodySequence;
    private Vector2 startPosition;
    public Vector2 startScale;

    private void Awake(){
        startPosition = transform.position;
        boxCollider2D = GetComponent<BoxCollider2D>();
        // Destroy(transform.parent.gameObject, duration);
    }

    private void Update(){
        float topY = transform.position.y + boxCollider2D.bounds.extents.y;
        boxCollider2D.enabled = Player.Instance.PlayerMovement.FootPosition.y >= topY;
    }

    private void OnDestroy(){
        if (!isPlayerOn){ return; }
        Player.Instance.transform.SetParent(null);
        DontDestroyOnLoad(Player.Instance.gameObject);
    }

    private void OnCollisionEnter2D(Collision2D other){
        if (!other.gameObject.CompareTag("Player")){ return; }
        if (Player.Instance.PlayerMovement.Velocity.y > 0f){ return; }
        if (Mathf.Abs(other.GetContact(0).normal.y) <= 0.95f){ return; }
        // PlayerMovement.canMove = false;
        PlayerMovement.onPlatform = true;
        isPlayerOn = true;
        Player.Instance.transform.SetParent(transform);
        if (jumpCoroutine != null){
            StopCoroutine(jumpCoroutine);
            jumpCoroutine = null;
        }
        JumpAnimation();
    }

    private void OnCollisionExit2D(Collision2D other){
        if (!other.gameObject.CompareTag("Player")){ return; }
        PlayerMovement.canMove = true;
        PlayerMovement.onPlatform = false;
        isPlayerOn = false;
        Player.Instance.transform.SetParent(null);
        DontDestroyOnLoad(Player.Instance.gameObject);
    }

    private void JumpAnimation(){
        jumpCoroutine = StartCoroutine(GetJumpInput());
        pushSequence = DOTween.Sequence();
        bodySequence = DOTween.Sequence();
        pushSequence.SetLink(gameObject);
        bodySequence.SetLink(gameObject);

        // Anticipation
        pushSequence.Append(
            transform.DOLocalMoveY((startPosition.y  - impulseVerticalOffset) * startScale.y, getImpulseDuration).SetEase(getImpulseEase)
        );

        bodySequence.Append(
            bodyTransform.DOScaleX(squashScale.x * startScale.x, getImpulseDuration).SetEase(getImpulseEase)
        );
        bodySequence.Join(
            bodyTransform.DOScaleY(squashScale.y * startScale.y, getImpulseDuration).SetEase(getImpulseEase)
        );

        // Impulse
        pushSequence.Append(
            transform.DOLocalMoveY((startPosition.y + impulseVerticalOffset) * startScale.y, pushDuration)
                .SetEase(pushEase)
        );

        bodySequence.Append(
            bodyTransform.DOScaleX(stretchScale.x * startScale.x, pushDuration).SetEase(pushEase)
        );
        bodySequence.Join(
            bodyTransform.DOScaleY(stretchScale.y * startScale.y, pushDuration).SetEase(pushEase)
        );

        // Recover
        pushSequence.Append(
            transform.DOLocalMoveY(startPosition.y * startScale.y, getImpulseDuration).SetEase(getImpulseEase)
        );

        bodySequence.Append(
            bodyTransform.DOScaleX(1f * startScale.x, getImpulseDuration).SetEase(getImpulseEase)
        );
        bodySequence.Join(
            bodyTransform.DOScaleY(1f * startScale.y, getImpulseDuration).SetEase(getImpulseEase)
        );
    }

    private void GetJumpInput(bool value){
        if (value){ impulseJump = true; }
    }

    private IEnumerator GetJumpInput(){
        float startInputDelay = getImpulseDuration - jumpInputTimeWindow * 0.5f;
        yield return new WaitForSeconds(startInputDelay);
        PlayerMovement.canJump = false;
        PlayerInput.OnJumpInput += GetJumpInput;
        yield return new WaitForSeconds(jumpInputTimeWindow);
        PlayerInput.OnJumpInput -= GetJumpInput;
        if (!isPlayerOn){
            PlayerMovement.canJump = true;
            yield break;
        }
        float desiredHeight = impulseJump ? impulsedJumpHeight : normalJumpHeight;
        impulseJump = false;
        Player.Instance.PlayerMovement.ForceJump(desiredHeight);
        PlayerMovement.canJump = true;
    }
}