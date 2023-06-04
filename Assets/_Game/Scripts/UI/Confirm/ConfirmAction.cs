using UnityEngine;
using UnityEngine.Events;

public class ConfirmAction : MonoBehaviour{
    [field: SerializeField] public UnityEvent OnConfirm{ get; private set; }
    [field: SerializeField] public UnityEvent OnCancel{ get; private set; }

    public void Cancel(){
        OnCancel?.Invoke();
    }

    public void Confirm(){
        OnConfirm?.Invoke();
    }
}