using System;
using UnityEngine;

public class ElementMagic : MonoBehaviour{
    [SerializeField] private Transform greenMushroomPrefab;
    public bool hasAbsorbedElement;
    public MagicType absorbedElement;

    private Transform greenMushroom;

    public void UseMagicElement(){
        if (!hasAbsorbedElement){ return; }
        switch (absorbedElement){
            case MagicType.Red:
                break;
            case MagicType.Green:
                // Fires a raycast down and spawns mushroom if hits ground
                if (!PlayerMovement.Grounded){ return; }
                int groundLayerMask = LayerMask.GetMask("Ground");
                Vector2 origin = transform.position;
                RaycastHit2D hit = Physics2D.Raycast(origin, Vector2.down, 5f, groundLayerMask);
                if (!hit.collider){ break; }
                if (greenMushroom){ Destroy(greenMushroom.gameObject); }
                greenMushroom = Instantiate(greenMushroomPrefab, hit.point, Quaternion.identity);
                greenMushroom.SetParent(hit.transform);
                break;
            case MagicType.Blue:
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
        hasAbsorbedElement = false;
        PlayerHUD.Instance.ToggleAbsorbedElement(false);
    }
}