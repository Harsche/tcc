using System;
using DG.Tweening;
using UnityEngine;

public class PauseMenu : MonoBehaviour{
    [SerializeField] private Canvas canvas;
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private CanvasGroup canvasGroup2;
    [SerializeField] private float fadeTime = 0.25f;

    public bool IsToggling{ get; private set; }


    private void Awake(){
        canvas.enabled = false;
        canvasGroup.alpha = 0f;
        canvasGroup2.alpha = 0f;
        canvasGroup2.interactable = false;
    }

    public void ToggleMenu(bool active){
        IsToggling = true;
        if (active){
            canvas.enabled = true;
            canvasGroup.DOFade(1f, fadeTime)
                .SetUpdate(true)
                .OnComplete(() => AnimateMenu(true));
            IsToggling = false;
            return;
        }

        AnimateMenu(false);
        canvasGroup.DOFade(1f, fadeTime)
            .SetUpdate(true)
            .OnComplete(() => {
                canvas.enabled = false;
                IsToggling = false;
            });
    }

    private void AnimateMenu(bool inOrOut){
        float alpha = inOrOut ? 1f : 0f;
        canvasGroup2.DOFade(alpha, fadeTime)
            .SetUpdate(true)
            .OnComplete(() => canvasGroup2.interactable = inOrOut);
    }
}