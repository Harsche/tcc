using System.Collections;
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

    public static bool Active{ get; private set; }
    public static bool IsToggling{ get; private set; }

    private void Awake(){
        canvas = GetComponent<Canvas>();
        canvas.enabled = false;
        dialogText.alpha = 0f;
        dialogBox.localScale = new Vector3(0f, 1f, 1f);
    }

    public void ShowDialog(string text){
        dialogText.text = text;
    }

    public void ToggleDialogCanvas(bool value){
        IsToggling = true;
        PlayerMovement.canMove = !value;
        toggleAnimation = DOTween.Sequence();

        if (value){
            Active = true;
            canvas.enabled = true;
            toggleAnimation.Append(dialogBox.DOScaleX(1f, scaleTime))
                .Append(dialogText.DOFade(1f, fadeTime))
                .OnComplete(() => IsToggling = false);
            return;
        }
        toggleAnimation.Append(dialogText.DOFade(0f, fadeTime))
            .Append(dialogBox.DOScaleX(0f, scaleTime))
            .OnComplete(() => {
                Active = false;
                canvas.enabled = false;
                IsToggling = false;
            });
    }
}