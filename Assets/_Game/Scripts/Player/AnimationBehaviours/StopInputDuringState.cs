using UnityEngine;

public class StopInputDuringState : StateMachineBehaviour{
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo,
        int layerIndex){
        PlayerMovement.canMove = false;
        Player.Instance.invulnerable = true;
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo,
        int layerIndex){
        PlayerMovement.canMove = true;
        Player.Instance.invulnerable = false;
    }
}