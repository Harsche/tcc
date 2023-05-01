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
    private BoxCollider2D boxCollider2D;

    private bool impulseJump;
    private bool isPlayerOn;
    private Coroutine jumpCoroutine;
    private Sequence pushSequence;
    private Vector2 startPosition;

    private void Awake(){
        startPosition = transform.position;
        boxCollider2D = GetComponent<BoxCollider2D>();
        Destroy(transform.parent.gameObject, duration);
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
        PlayerMovement.canMove = false;
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
        isPlayerOn = false;
        Player.Instance.transform.SetParent(null);
        DontDestroyOnLoad(Player.Instance.gameObject);
    }

    private void JumpAnimation(){
        StartCoroutine(GetJumpInput());
        pushSequence = DOTween.Sequence();
        pushSequence.Append(
            transform.DOMoveY(startPosition.y - impulseVerticalOffset, getImpulseDuration).SetEase(getImpulseEase)
        );
        pushSequence.Append(
            transform.DOMoveY(startPosition.y + impulseVerticalOffset, pushDuration)
                .SetEase(pushEase)
        );
        pushSequence.Append(
            transform.DOMoveY(startPosition.y, getImpulseDuration).SetEase(getImpulseEase)
        );
        pushSequence.SetLink(gameObject);
    }

    private void GetJumpInput(bool value){
        if (value){ impulseJump = true; }
    }

    private IEnumerator GetJumpInput(){
        float startInputDelay = getImpulseDuration - jumpInputTimeWindow * 0.5f;
        yield return new WaitForSeconds(startInputDelay);
        PlayerInput.OnJumpInput += GetJumpInput;
        yield return new WaitForSeconds(jumpInputTimeWindow);
        PlayerInput.OnJumpInput -= GetJumpInput;
        float desiredHeight = impulseJump ? impulsedJumpHeight : normalJumpHeight;
        impulseJump = false;
        Player.Instance.PlayerMovement.ForceJump(desiredHeight);
    }
}