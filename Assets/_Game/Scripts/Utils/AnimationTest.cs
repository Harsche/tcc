using UnityEngine;

namespace Utils{
    public class AnimationTest : StateMachineBehaviour{
        [SerializeField] private bool toggleAttackColor;
        private Material material;
        private static readonly int UseAttackColor = Shader.PropertyToID("_Use_Attack_Color");

        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo,
            int layerIndex){
            if (!material){ material = animator.gameObject.GetComponent<SpriteRenderer>().material; }
            material.SetInt(UseAttackColor, toggleAttackColor ? 1 : 0);
        }

        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo,
            int layerIndex){ }

        public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo,
            int layerIndex){ }

        public override void OnStateMove(Animator animator, AnimatorStateInfo stateInfo,
            int layerIndex){ }

        public override void OnStateIK(Animator animator, AnimatorStateInfo stateInfo,
            int layerIndex){ }
    }
}