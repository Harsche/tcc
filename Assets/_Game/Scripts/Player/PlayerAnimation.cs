using System;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour{
    private static readonly int SpeedX = Animator.StringToHash("SpeedX");
    private static readonly int SpeedY = Animator.StringToHash("SpeedY");
    private static readonly int Grounded = Animator.StringToHash("Grounded");
    private static readonly int Parry = Animator.StringToHash("Parry");
    private static readonly int ParryDirectionY = Animator.StringToHash("ParryDirectionY");
    private static readonly int Jump = Animator.StringToHash("Jump");

    private static readonly int ParryColor = Shader.PropertyToID("_ParryColor");
    
    [SerializeField] private Rigidbody2D playerRigidbody2D;
    [SerializeField] private PlayerMovement playerMovement;

    private Animator animator;
    private SpriteRenderer spriteRenderer;

    public event Action OnStep;
    public event Action OnParry;

    private void Awake(){
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start(){
        Player.Instance.PlayerParry.OnParry += () => animator.SetTrigger(Parry);
        Player.Instance.PlayerMovement.OnJump += () => animator.SetTrigger(Jump);
    }

    private void Update(){
        animator.SetFloat(SpeedX, Mathf.Abs(playerRigidbody2D.velocity.x));
        animator.SetFloat(SpeedY, playerRigidbody2D.velocity.y);
        animator.SetFloat(ParryDirectionY, PlayerInput.LookDirection.y);
        animator.SetBool(Grounded, PlayerMovement.Grounded);
    }

    public void OnStepEvent(){
        OnStep?.Invoke();
    }
    
    public void OnParryEvent(){
        OnParry?.Invoke();
    }

    public void ChangeParryColor(Color color){
        spriteRenderer.material.SetColor(ParryColor, color);
    }
}