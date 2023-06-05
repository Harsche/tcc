using System;
using UnityEngine;
using UnityEngine.Animations;

public class ElementMagic : MonoBehaviour{
    [SerializeField] private Transform greenMushroomPrefab;
    public bool hasAbsorbedElement;
    public Element absorbedElement;

    private Transform greenMushroom;

    public void UseMagicElement(){
        if (!hasAbsorbedElement){ return; }
        switch (absorbedElement){
            case Element.Fire:
                break;
            case Element.Nature:
                DoNatureMagic();
                break;
            case Element.Water:
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
        hasAbsorbedElement = false;
        PlayerHUD.Instance.SetAbsorbedElement(Element.None);
        Color eyesColor = GameManager.GameData.elementsData[Element.None].SpriteColor;
        Player.Instance.PlayerAnimation.ChangeEyesColor(eyesColor);
    }

    private void DoNatureMagic(){
        // Fires a raycast down and spawns mushroom if hits ground
        if (!PlayerMovement.Grounded){ return; }
        int groundLayerMask = LayerMask.GetMask("Ground");
        Vector2 origin = transform.position;
        RaycastHit2D hit = Physics2D.Raycast(origin, Vector2.down, 5f, groundLayerMask);
        if (!hit.collider){ return; }
        if (greenMushroom){ Destroy(greenMushroom.gameObject); }
        greenMushroom = Instantiate(greenMushroomPrefab, hit.point, Quaternion.identity);
        Transform hitObject = hit.collider.transform;
        var constraintSource = new ConstraintSource(){
            sourceTransform = hitObject,
            weight = 1f
        };
        var positionConstraint = greenMushroom.GetComponent<PositionConstraint>();
        positionConstraint.translationOffset = hit.point - (Vector2) hit.transform.position;
        positionConstraint.AddSource(constraintSource);
    }
}