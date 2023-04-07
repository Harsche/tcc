using DG.Tweening;
using UnityEngine;

public class MenuOptions : MonoBehaviour{
    [SerializeField] private float fadeTime = 1f;
    [SerializeField] private Ease ease = Ease.Linear;
    private CanvasGroup canvasGroup;

    private void Awake(){
        canvasGroup = GetComponent<CanvasGroup>();
    }

    public void ChangeMenu(MenuOptions menuOptions){
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
        canvasGroup.DOFade(0f, fadeTime * 0.5f)
            .SetEase(ease)
            .OnComplete(() => ActivateNextMenu(menuOptions));
    }

    private void ActivateNextMenu(MenuOptions menuOptions){
        CanvasGroup nextCanvasGroup = menuOptions.canvasGroup;
        nextCanvasGroup.DOFade(1f, fadeTime * 0.5f)
            .SetEase(ease)
            .OnComplete(() => {
                nextCanvasGroup.interactable = true;
                nextCanvasGroup.blocksRaycasts = true;
            });
    }
}