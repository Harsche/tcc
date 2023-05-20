using System.Collections;
using UnityEngine;
using Utils;

public class Enemy02 : EnemyBase{
    [SerializeField] private float desiredAttackDistance = 4f;
    [SerializeField] private float attackRecoverTime = 2f;
    
    private Coroutine checkPlayerCoroutine;

    private void Patrol_Enter(){
        checkPlayerCoroutine = StartCoroutine(OnCheckPlayerCoroutine());
    }

    private void Patrol_OnCheckPlayer(bool isPlayerInSight){
        if (isPlayerInSight){ ChangeState(State.Attack); }
    }

    private IEnumerator Attack_Enter(){
        isAttacking = true;
        animator.SetTrigger(Attack);
        myRigidbody2D.velocity = Vector2.zero;
        yield return new WaitUntil(() => !isAttacking);
        ChangeState(State.Chase);
    }

    private void Chase_Update(){
        Vector2 playerDistance = GameUtils.GetPlayerDistance(transform.position);

        if (!GetFloorAhead(playerDistance.x)){
            myRigidbody2D.velocity = Vector2.zero;
            return;
        }

        Vector2 velocity = myRigidbody2D.velocity;
        velocity.x = maxSpeed.x * Mathf.Sign(playerDistance.x);
        myRigidbody2D.velocity = velocity;
    }

    private void Chase_OnCheckPlayer(bool isPlayerInSight){
        if (GameUtils.GetPlayerDistance(transform.position).magnitude <= desiredAttackDistance){
            ChangeState(State.Attack);
        }
    }

    private IEnumerator BehaviourCoroutine(){
        while (gameObject){
            Vector2 playerDistance = GameUtils.GetPlayerDistance(transform.position);
            bool attack = Mathf.Abs(playerDistance.x) <= desiredAttackDistance;

            while (attack){
                animator.SetTrigger(Attack);
                myRigidbody2D.velocity = Vector2.zero;
                yield return new WaitForSeconds(attackRecoverTime);
                playerDistance = GameUtils.GetPlayerDistance(transform.position);
                attack = Mathf.Abs(playerDistance.x) <= desiredAttackDistance;
            }

            Vector2 followPlayerSpeed = maxSpeed;
            followPlayerSpeed.x *= Mathf.Sign(playerDistance.x);
            // myRigidbody2D.velocity = Utils.GameUtils.OrientVelocityToGround(followPlayerSpeed, );
        }
    }

    private IEnumerator OnCheckPlayerCoroutine(){
        WaitForSeconds waitTime = new(0.25f);
        while (gameObject){
            yield return waitTime;
            StateMachineDriver.OnCheckPlayer.Invoke(CheckPlayerInRange());
        }
    }

    // private IEnumerator Patrol(){
    //     
    // }
}