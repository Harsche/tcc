using UnityEngine;
using UnityEngine.Events;

public class CollisionEvent : MonoBehaviour{
    [SerializeField] private UnityEvent onEnterEvent;
    [SerializeField] private UnityEvent onExitEvent;
    [SerializeField] private bool unlockParry;
    [SerializeField] private bool unlockDash;

    public void OnTriggerEnter2D(Collider2D other){
        if (!other.CompareTag("Player")){ return; }
        onEnterEvent?.Invoke();
        if (unlockParry) Player.Instance.PlayerParry.EnableParry = true;
        if (unlockDash) Player.Instance.PlayerMovement.enableDash = true;
    }

    public void OnTriggerExit2D(Collider2D other){
        if (other.CompareTag("Player")){ onExitEvent?.Invoke(); }
    }
}