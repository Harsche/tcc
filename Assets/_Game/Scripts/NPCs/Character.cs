using UnityEngine;

public class Character : Interactable{
    [SerializeField] private string characterName;

    public override void Interact(){
        DialogManager.Instance.GetCharacterDialog(characterName);
    }
}