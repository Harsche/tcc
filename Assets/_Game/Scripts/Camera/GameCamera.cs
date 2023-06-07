using System;
using System.Collections;
using Cinemachine;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace Scripts.Camera{
    public class GameCamera : MonoBehaviour{
        [SerializeField] private CinemachineBrain cinemachineBrain;
        [SerializeField] private CinemachineVirtualCamera virtualCamera;
        [SerializeField] private CinemachineTargetGroup targetGroup;
        [SerializeField] private Volume volume;
        [SerializeField] private float backgroundBlurAmount = 5f;
        [SerializeField] private float fadeTime = 0.25f;
        [SerializeField] private float normalCameraDistance = 18.5f;
        [SerializeField] private float dialogCameraDistance = 7f;
        [SerializeField] private float tweenCameraDistanceDuration = 1f;
        [SerializeField] private float enemyTargetWeight = 0.5f;
        [SerializeField] private float addTargetSmooth = 1f;
        [SerializeField] private float removeTargetSmooth = 1f;
        private bool isFocusing;
        private float startBlurAmount;

        // private readonly HashSet<Transform> surroundingEnemies = new();

        public static GameCamera Instance{ get; private set; }

        private void Awake(){
            if (Instance != null){
                Destroy(gameObject);
                return;
            }
            Instance = this;
            if (volume.profile.TryGet(out DepthOfField depthOfField)){ startBlurAmount = depthOfField.aperture.value; }
        }

        public void AddTarget(Transform target){
            // surroundingEnemies.Add(target);
            targetGroup.AddMember(target, 0f, 0f);
            int index = Array.FindIndex(targetGroup.m_Targets, t => t.target == target);
            DOTween.To(
                () => targetGroup.m_Targets[index].weight,
                x => targetGroup.m_Targets[index].weight = x,
                enemyTargetWeight,
                addTargetSmooth
            );
        }

        public void RemoveTarget(Transform target){
            // surroundingEnemies.Remove(target);
            int index = Array.FindIndex(targetGroup.m_Targets, t => t.target == target);
            DOTween.To(
                    () => targetGroup.m_Targets[index].weight,
                    x => targetGroup.m_Targets[index].weight = x,
                    0f,
                    removeTargetSmooth
                )
                .OnComplete(() => targetGroup.RemoveMember(target));
        }

        public void ToggleBackgroundBlur(bool value){
            float blurValue = value ? backgroundBlurAmount : startBlurAmount;
            if (volume.profile.TryGet(out DepthOfField depthOfField)){
                StartCoroutine(FadeBlurCoroutine(blurValue, fadeTime, depthOfField));
            }
        }

        public void ToggleFocus(){
            var framingTransposer = virtualCamera.GetCinemachineComponent<CinemachineFramingTransposer>();
            isFocusing = !isFocusing;
            DOTween.To(
                () => framingTransposer.m_CameraDistance,
                desiredDistance => framingTransposer.m_CameraDistance = desiredDistance,
                isFocusing ? dialogCameraDistance : normalCameraDistance,
                tweenCameraDistanceDuration
            );
        }

        private IEnumerator FadeBlurCoroutine(float value, float time, DepthOfField depthOfField){
            float startTime = Time.unscaledTime;
            float startValue = depthOfField.aperture.value;
            while (Time.unscaledTime <= startTime + time){
                float lerpValue = (Time.unscaledTime - startTime) / time;
                depthOfField.aperture.value = Mathf.Lerp(startValue, value, lerpValue);
                yield return null;
            }
            depthOfField.aperture.value = value;
        }

        // private void UpdateTargetGroup(){
        //     HashSet<CinemachineTargetGroup.Target> targets = targetGroup.m_Targets.ToHashSet();
        //     Stack<CinemachineTargetGroup.Target> targetsToRemove = 
        //     foreach (Transform enemy in surroundingEnemies){
        //         if()
        //     }
        //     
        //     targeting.IntersectWith(surroundingEnemies);
        // }
    }
}