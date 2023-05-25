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
    }

    public void ToggleMenu(bool active){
        IsToggling = true;
        if (active){
            canvas.enabled = true;
            canvasGroup.DOFade(1f, fadeTime)
                .OnComplete(() => AnimateMenu(true));
            IsToggling = false;
            return;
        }

        AnimateMenu(false);
        canvasGroup.DOFade(1f, fadeTime)
            .OnComplete(() => {
                canvas.enabled = false;
                IsToggling = true;
            });
    }

    private void AnimateMenu(bool inOrOut){
        canvasGroup2.alpha = inOrOut ? 1f : 0f;
    }
}