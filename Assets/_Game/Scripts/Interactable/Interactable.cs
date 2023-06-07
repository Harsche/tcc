using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

public abstract class Interactable : MonoBehaviour, IInteractable{
    [SerializeField] private UnityEvent onInteract;
    [SerializeField] private CanvasGroup interactionIndication;
    [SerializeField] private float fadeTime = 0.25f;
    [SerializeField] private Ease ease = Ease.Linear;
    [SerializeField] private bool isInteractable = true;
    private Tweener indicationFade;

    public bool IsInteractable{
        get => isInteractable;
        protected set{
            isInteractable = value;
            ToggleIndication(isInteractable);
        }
    }

    private void Awake(){
        ToggleIndication(false);
    }

    private void OnTriggerEnter2D(Collider2D col){
        if (!col.CompareTag("Player")){ return; }
        if (IsInteractable){ ToggleIndication(true); }
    }

    private void OnTriggerExit2D(Collider2D col){
        if (!col.CompareTag("Player")){ return; }
        ToggleIndication(false);
    }

    public virtual void Interact(){
        onInteract?.Invoke();
    }

    public void ToggleIndication(bool value){
        float endValue = value ? 1f : 0f;
        indicationFade?.Kill();
        indicationFade = interactionIndication.DOFade(endValue, fadeTime)
            .SetEase(ease);
    }
}