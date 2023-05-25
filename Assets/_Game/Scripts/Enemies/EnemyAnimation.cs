using System;
using UnityEngine;

namespace Scripts.Enemies{
    public class EnemyAnimation : MonoBehaviour{
        [SerializeField] private EnemyBase enemyBase;
        [SerializeField] private SpriteRenderer spriteRenderer;
        [SerializeField] private Animator animator;
        private static readonly int Velocity = Animator.StringToHash("Velocity");
        private static readonly int Attack = Animator.StringToHash("Attack");

        private void OnEnable(){
            enemyBase.OnAttack += TriggerAttackAnimation;
        }

        private void OnDisable(){
            enemyBase.OnAttack -= TriggerAttackAnimation;
        }

        private void TriggerAttackAnimation(){
            animator.SetTrigger(Attack);
        }

        private void Update(){
            float xVelocity = enemyBase.Rigidbody2D.velocity.x;
            animator.SetFloat(Velocity, Mathf.Abs(xVelocity));
            if (xVelocity != 0){
                bool flipX = xVelocity < 0;
                transform.localScale = new Vector3(flipX ? -1f : 1, 1f, 1f);
            }
        }
    }
}