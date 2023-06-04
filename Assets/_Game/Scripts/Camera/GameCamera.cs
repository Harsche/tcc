using System.Collections;
using Cinemachine;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace Scripts.Camera{
    public class GameCamera : MonoBehaviour{
        [SerializeField] private CinemachineBrain cinemachineBrain;
        [SerializeField] private Volume volume;
        [SerializeField] private float backgroundBlurAmount = 5f;
        [SerializeField] private float fadeTime = 0.25f;
        [SerializeField] private float normalCameraDistance = 18.5f;
        [SerializeField] private float dialogCameraDistance = 7f;

        private float startBlurAmount;
        private bool isFocusing;

        private void Awake(){
            if (volume.profile.TryGet(out DepthOfField depthOfField)){ startBlurAmount = depthOfField.aperture.value; }
        }

        public void ToggleBackgroundBlur(bool value){
            float blurValue = value ? backgroundBlurAmount : startBlurAmount;
            if (volume.profile.TryGet(out DepthOfField depthOfField)){
                StartCoroutine(FadeBlurCoroutine(blurValue, fadeTime, depthOfField));
            }
        }

        public void ToggleFocus(){
            CinemachineVirtualCamera virtualCamera = Player.Instance.PlayerVirtualCamera;
            var framingTransposer = virtualCamera.GetCinemachineComponent<CinemachineFramingTransposer>();
            isFocusing = !isFocusing;
            framingTransposer.m_CameraDistance = isFocusing ? dialogCameraDistance : normalCameraDistance;
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
    }
}