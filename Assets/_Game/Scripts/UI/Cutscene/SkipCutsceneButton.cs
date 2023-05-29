using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;
using UnityEngine.Serialization;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class SkipCutsceneButton : MonoBehaviour{
    [FormerlySerializedAs("button")] [SerializeField] private Button skipButton;
    [SerializeField] private float timeToDisable = 3f;
    private IDisposable inputListener;

    private void Start(){
        inputListener = InputSystem.onAnyButtonPress.Call(ActivateButton);
        skipButton.interactable = false;
        gameObject.SetActive(false);
    }

    private void OnDestroy(){
        inputListener.Dispose();
    }

#if UNITY_EDITOR
    private void OnValidate(){
        if (!skipButton){ skipButton = GetComponent<Button>(); }
    }
#endif

    private void ActivateButton(InputControl button){
        StopAllCoroutines();
        skipButton.interactable = true;
        gameObject.SetActive(true);
        StartCoroutine(DeactivateButton());
    }

    private IEnumerator DeactivateButton(){
        yield return new WaitForSeconds(timeToDisable);
        skipButton.interactable = false;
        gameObject.SetActive(false);
    }
}