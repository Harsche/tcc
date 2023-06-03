using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class ButtonDoor : MonoBehaviour{
    private static readonly int Open = Animator.StringToHash("Open");
    private static readonly int GetHitAnimation = Animator.StringToHash("Button_Plant_Body_GetHit");
    private static readonly int HitTime = Shader.PropertyToID("_Hit_Time");

    [SerializeField] private float startDelay = 1f;
    [SerializeField] private Animator animator;
    [SerializeField] private SpriteRenderer spriteRenderer;

    // TODO - Save opened state if player exits scene before animation ends
    private bool opened;

#if UNITY_EDITOR
    private void OnValidate(){
        if (!animator){ animator = GetComponent<Animator>(); }
        if (!spriteRenderer){ spriteRenderer = GetComponent<SpriteRenderer>(); }
    }
#endif

    public void OpenDoor(){
        spriteRenderer.material.SetFloat(HitTime, Time.time);
        StartCoroutine(OpenDoorCoroutine());
    }

    private IEnumerator OpenDoorCoroutine(){
        yield return new WaitForSeconds(startDelay);
        animator.SetTrigger(Open);
        yield return new WaitUntil(() => animator.GetCurrentAnimatorStateInfo(0).shortNameHash == GetHitAnimation);
        AnimatorStateInfo animationState = animator.GetCurrentAnimatorStateInfo(0);
        yield return new WaitForSeconds(animationState.length);
        gameObject.SetActive(false);
    }
}