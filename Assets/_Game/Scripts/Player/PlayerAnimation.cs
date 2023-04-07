using System;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour{
    private static readonly int Speed = Animator.StringToHash("Speed");
    private static readonly int Grounded = Animator.StringToHash("Grounded");

    [SerializeField] private Rigidbody2D playerRigidbody2D;
    [SerializeField] private PlayerMovement playerMovement;

    private Animator animator;

    public event Action OnStep;

    private void Awake(){
        animator = GetComponent<Animator>();
    }

    private void Update(){
        animator.SetFloat(Speed, playerRigidbody2D.velocity.sqrMagnitude);
        animator.SetBool(Grounded, playerMovement.Grounded);
    }

    public void OnStepEvent(){
        OnStep?.Invoke();
    }
}