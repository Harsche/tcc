using System;
using UnityEngine;

namespace Scripts.Enemies{
    public class EnemyAnimation : MonoBehaviour{
        [SerializeField] private EnemyBase enemyBase;
        [SerializeField] private SpriteRenderer spriteRenderer;
        [SerializeField] private Animator animator;
        [SerializeField] private bool orientByVelocity = true;
        private static readonly int Velocity = Animator.StringToHash("Velocity");
        private static readonly int Attack = Animator.StringToHash("Attack");
        private static readonly int HitTime = Shader.PropertyToID("_Hit_Time");

        private void OnEnable(){
            enemyBase.OnAttack += TriggerAttackAnimation;
        }

        private void OnDisable(){
            enemyBase.OnAttack -= TriggerAttackAnimation;
        }

        public void FacePlayerDirection(){
            Vector2 direction = Utils.GameUtils.GetPlayerDistance(transform.position);
            bool flipX = direction.x < 0;
            transform.localScale = new Vector3(flipX ? -1f : 1, 1f, 1f);
        }

        public void DoHitEffect(){
            spriteRenderer.material.SetFloat(HitTime, Time.time);
        }

        private void TriggerAttackAnimation(){
            animator.SetTrigger(Attack);
        }

        private void Update(){
            float xVelocity = enemyBase.Rigidbody2D.velocity.x;
            animator.SetFloat(Velocity, Mathf.Abs(xVelocity));
            if (orientByVelocity && xVelocity != 0){
                bool flipX = xVelocity < 0;
                transform.localScale = new Vector3(flipX ? -1f : 1, 1f, 1f);
            }
        }

#if UNITY_EDITOR
        private void OnValidate(){
            if (!enemyBase){ enemyBase = GetComponent<EnemyBase>(); }
        }
#endif
    }
}