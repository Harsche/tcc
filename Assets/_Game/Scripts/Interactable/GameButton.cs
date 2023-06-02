using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

public class GameButton : MonoBehaviour{
    [SerializeField] private bool requireColor;
    [FormerlySerializedAs("magicType")] [SerializeField] private Element element;
    [SerializeField] private UnityEvent onPress;


    public void Interact(Element projectileColor){
        if (requireColor && projectileColor == element){
            onPress?.Invoke();
            return;
        }
        onPress?.Invoke();
    }
}