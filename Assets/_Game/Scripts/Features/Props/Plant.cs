using System.Collections.Generic;
using UnityEngine;

public class Plant : MonoBehaviour{
    [SerializeField] private bool animate = true;
    [SerializeField] private bool randomStart = true;
    [SerializeField] private AnimationClip animationClip;

    private Animator animator;
    private AnimationClipOverrides clipOverrides;

    private void Awake(){
        animator = GetComponent<Animator>();
        if (!animate){
            animator.enabled = false;
            return;
        }
        var overrideController = new AnimatorOverrideController(animator.runtimeAnimatorController);
        animator.runtimeAnimatorController = overrideController;

        clipOverrides = new AnimationClipOverrides(overrideController.overridesCount);
        overrideController.GetOverrides(clipOverrides);

        clipOverrides["Idle"] = animationClip;
        overrideController.ApplyOverrides(clipOverrides);
        if (randomStart){ animator.Play("Idle", 0, Random.Range(0f, 1f)); }
    }

    public class AnimationClipOverrides : List<KeyValuePair<AnimationClip, AnimationClip>>{
        public AnimationClipOverrides(int capacity) : base(capacity){ }

        public AnimationClip this[string name]{
            get{ return this.Find(x => x.Key.name.Contains(name)).Value; }
            set{
                int index = this.FindIndex(x => x.Key.name.Contains(name));
                if (index != -1)
                    this[index] = new KeyValuePair<AnimationClip, AnimationClip>(this[index].Key, value);
            }
        }
    }
}