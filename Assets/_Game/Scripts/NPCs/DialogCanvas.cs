using System;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class DialogCanvas : MonoBehaviour{
    [SerializeField] private RectTransform dialogBox;
    [SerializeField] private TextMeshProUGUI dialogText;
    [SerializeField] private float scaleTime = 0.25f;
    [SerializeField] private float fadeTime = 0.25f;

    private Canvas canvas;
    private Sequence toggleAnimation;

    public void ShowDialog(string text){ }

    private void Awake(){
        canvas = GetComponent<Canvas>();
        canvas.enabled = false;
        dialogText.alpha = 0f;
        dialogBox.localScale = new Vector3(0f, 1f, 1f);
        PlayerInput.OnShieldToggle += ToggleDialogCanvas;
    }

    public void ToggleDialogCanvas(bool value){
        toggleAnimation = DOTween.Sequence();

        if (value){
            canvas.enabled = true;
            toggleAnimation.Append(dialogBox.DOScaleX(1f, scaleTime))
                .Append(dialogText.DOFade(1f, fadeTime));
            return;
        }
        toggleAnimation.Append(dialogText.DOFade(0f, fadeTime))
            .Append(dialogBox.DOScaleX(0f, scaleTime))
            .OnComplete(() => canvas.enabled = false);
    }
}