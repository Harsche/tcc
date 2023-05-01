using UnityEngine;

public class Character : Interactable{
    [SerializeField] private string characterName;
    [SerializeField] private string dialog;

    public override void Interact(){
        string knotString = characterName;
        if (!string.IsNullOrEmpty(dialog)){ knotString += $".{dialog}";}
        DialogManager.Instance.GetCharacterDialog(knotString);
    }
}