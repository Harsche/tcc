using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

public class GameButton : MonoBehaviour{
    private static readonly int HitTime = Shader.PropertyToID("_Hit_Time");
    
    [SerializeField] private bool requireColor;
    [FormerlySerializedAs("magicType")] [SerializeField] private Element element;
    [SerializeField] private UnityEvent onPress;
    [SerializeField] private SpriteRenderer spriteRenderer;

#if UNITY_EDITOR
    private void OnValidate(){
        if (!spriteRenderer){ spriteRenderer = GetComponent<SpriteRenderer>(); }
    }
#endif


    public void Interact(Element projectileColor){
        if (requireColor && projectileColor == element){
            onPress?.Invoke();
            return;
        }
        onPress?.Invoke();
        spriteRenderer.material.SetFloat(HitTime, Time.time);
    }
}