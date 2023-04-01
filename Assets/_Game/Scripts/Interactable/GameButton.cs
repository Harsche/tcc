using UnityEngine;
using UnityEngine.Events;

public class GameButton : MonoBehaviour{
    [SerializeField] private bool requireColor;
    [SerializeField] private MagicType magicType;
    [SerializeField] private UnityEvent onPress;


    public void Interact(MagicType projectileColor){
        if (requireColor && projectileColor == magicType){
            onPress?.Invoke();
            return;
        }
        onPress?.Invoke();
    }
}