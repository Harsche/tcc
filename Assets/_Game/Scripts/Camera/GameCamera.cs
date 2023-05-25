using System;
using Cinemachine;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace Scripts.Camera{
    public class GameCamera : MonoBehaviour{
        [SerializeField] private CinemachineBrain cinemachineBrain;
        [SerializeField] private Volume volume;
        [SerializeField] private float backgroundBlurAmount = 5f;

        private float startBlurAmount;

        private void Awake(){
            if (volume.profile.TryGet(out DepthOfField depthOfField)){
                startBlurAmount = depthOfField.aperture.value;
            }
        }

        public void ToggleBackgroundBlur(bool value){
            float blurValue = value ? backgroundBlurAmount : startBlurAmount;
            if (volume.profile.TryGet(out DepthOfField depthOfField)){
                startBlurAmount = depthOfField.aperture.value = blurValue;
            }
        }
    }
}