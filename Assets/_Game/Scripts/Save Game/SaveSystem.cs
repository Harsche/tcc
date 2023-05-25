using System;
using BayatGames.SaveGameFree;
using BayatGames.SaveGameFree.Encoders;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game.SaveSystem{
    public class SaveSystem : MonoBehaviour{
        private static int SaveSlot;
    
        [SerializeField] private float rotateSpeed = 5f;
        public static SaveData SaveData{ get; private set; } = new();
    
        private static float LastSaveTime;
    
        private void Update(){
            transform.Rotate(transform.forward, rotateSpeed * Time.deltaTime);
        }
    
        private void OnTriggerEnter2D(Collider2D col){
            if (!col.gameObject.CompareTag("Player")){ return; }
            SaveGameState(col.gameObject);
        }
    
        private void SaveGameState(GameObject player){
            SaveData.playerPosition = player.transform.position;
            SaveData.loadScene = SceneManager.GetActiveScene().name;
            SaveData.playTime += TimeSpan.FromSeconds(Time.unscaledTime - LastSaveTime);
            SetLastSaveTime();
            SaveToFile();
        }
    
        private void SaveToFile(){
            // SG.SaveGame.Encode = true;
            SaveData.isNewGame = false;
            SaveGame.Save($"data{SaveSlot}", SaveData, new SaveGameSimpleEncoder());
        }
    
        public static void LoadFromFile(int slotIndex){
            SaveSlot = slotIndex;
            if (!SaveGame.Exists($"data{SaveSlot}")){ return; }
            SaveData = SaveGame.Load<SaveData>($"data{SaveSlot}");
        }
    
        public static SaveData GetSaveData(int slotIndex){
            return !SaveGame.Exists($"data{slotIndex}")
                ? null
                : SaveGame.Load<SaveData>($"data{slotIndex}");
        }
    
        public static void DeleteSaveFile(){
            for (int i = 0; i < 3; i++)
                if (SaveGame.Exists($"data{i}")){ SaveGame.Delete($"data{i}"); }
        }
    
        public static bool SaveFileExists(int slotIndex){
            return SaveGame.Exists($"data{slotIndex}");
        }
        
        public static void SetLastSaveTime(){
            LastSaveTime = Time.unscaledTime;
        }
    }
}
