using UnityEngine;

namespace Utils{
    public class ToggleParryColor : StateMachineBehaviour{
        private Material material;
        private static readonly int ParryColor = Shader.PropertyToID("_Use_Parry_Color");


        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo,
            int layerIndex){
            if (!material){ material = animator.gameObject.GetComponent<SpriteRenderer>().material; }
            material.SetInt(ParryColor, 1);
        }

        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo,
            int layerIndex){
            material.SetInt(ParryColor, 0);
        } 
    }
}