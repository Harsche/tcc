using System.Collections;
using UnityEngine;

public class Enemy02 : Enemy{
    [SerializeField] private float desiredAttackDistance = 4f;
    [SerializeField] private float attackRecoverTime = 2f;
    private Coroutine behaviour;

    protected override void Awake(){
        base.Awake();
        behaviour ??= StartCoroutine(BehaviourCoroutine());
    }

    protected override void OnCheckPlayer(bool isInSight){
        onBattle = isInSight;
        if (!isInSight){
            StopCoroutine(behaviour);
            behaviour = null;
            return;
        }
        
    }

    private IEnumerator BehaviourCoroutine(){
        while (gameObject){
            Vector2 playerDistance = Utils.GameUtils.GetPlayerDistance(transform.position);
            bool attack = Mathf.Abs(playerDistance.x) <= desiredAttackDistance;

            while (attack){
                animator.SetTrigger(Attack);
                myRigidbody2D.velocity = Vector2.zero;
                yield return new WaitForSeconds(attackRecoverTime);
                playerDistance = Utils.GameUtils.GetPlayerDistance(transform.position);
                attack = Mathf.Abs(playerDistance.x) <= desiredAttackDistance;
            }

            Vector2 followPlayerSpeed = maxSpeed;
            followPlayerSpeed.x *= Mathf.Sign(playerDistance.x);
            // myRigidbody2D.velocity = Utils.GameUtils.OrientVelocityToGround(followPlayerSpeed, );
        }
    }

    private IEnumerator Patrol(){
        
    }
}