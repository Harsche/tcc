using UnityEngine;

public class Character : Interactable{
    [SerializeField] private string characterName;
    [SerializeField] private string dialog;

    public override void Interact(){
        if (!string.IsNullOrEmpty(dialog)){ DialogManager.Instance.GetCharacterDialog(characterName, dialog); }
        else{ DialogManager.Instance.GetCharacterDialog(characterName); }
    }
}