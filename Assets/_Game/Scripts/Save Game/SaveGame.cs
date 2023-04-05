using System;
using BayatGames.SaveGameFree.Encoders;
using UnityEngine;
using UnityEngine.SceneManagement;
using SG = BayatGames.SaveGameFree;

public class SaveGame : MonoBehaviour{
    private static SaveData GameData = new();

    [SerializeField] private float rotateSpeed = 5f;

    public static event Action<SaveData> OnLoadSave;

    private void Start(){
        LoadFromFile();
    }

    private void Update(){
        transform.Rotate(transform.forward, rotateSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D col){
        if (!col.gameObject.CompareTag("Player")){ return; }
        SaveGameState(col.gameObject);
    }

    private void SaveGameState(GameObject player){
        GameData.playerPosition = player.transform.position;
        GameData.loadScene = SceneManager.GetActiveScene().name;
        SaveToFile();
    }

    private void SaveToFile(){
        // SG.SaveGame.Encode = true;
        SG.SaveGame.Save("data", GameData, new SaveGameSimpleEncoder());
    }

    private void LoadFromFile(){
        
        if (SG.SaveGame.Exists("data")){ GameData = SG.SaveGame.Load<SaveData>("data"); }
        OnLoadSave?.Invoke(GameData);
    }
}