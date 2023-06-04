using System;
using BayatGames.SaveGameFree;
using BayatGames.SaveGameFree.Encoders;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game.SaveSystem{
    public class SaveSystem : MonoBehaviour{
        private static int SaveSlot;

        private static float LastSaveTime;

        [SerializeField] private float rotateSpeed = 5f;
        public static SaveData SaveData{ get; private set; } = new();


        private void Update(){
            transform.Rotate(transform.forward, rotateSpeed * Time.deltaTime);
        }

        private void OnTriggerEnter2D(Collider2D col){
            if (!col.gameObject.CompareTag("Player")){ return; }
            SaveGameState(col.gameObject);
        }

        public static void SetupSaveSystem(){
            // SG.SaveGame.Encode = true;
            SaveGame.Serializer = new GameSerializer();
        }

        public static void DeleteSaveFile(int slot){
            if (SaveGame.Exists($"data{slot}")){ SaveGame.Delete($"data{slot}"); }
        }

        public static SaveData GetSaveData(int slotIndex){
            return !SaveGame.Exists($"data{slotIndex}")
                ? null
                : SaveGame.Load<SaveData>($"data{slotIndex}");
        }

        public static void LoadFromFile(int slotIndex){
            SaveSlot = slotIndex;
            if (!SaveGame.Exists($"data{SaveSlot}")){ return; }
            SaveData = SaveGame.Load<SaveData>($"data{SaveSlot}");
        }

        public static bool SaveFileExists(int slotIndex){
            return SaveGame.Exists($"data{slotIndex}");
        }

        public static void SetLastSaveTime(){
            LastSaveTime = Time.unscaledTime;
        }

        private void SaveGameState(GameObject player){
            SaveData.playerPosition = player.transform.position;
            SaveData.loadScene = SceneManager.GetActiveScene().name;
            SaveData.playTime += TimeSpan.FromSeconds(Time.unscaledTime - LastSaveTime);
            SetLastSaveTime();
            SaveToFile();
        }

        private void SaveToFile(){
            SaveData.isNewGame = false;
            SaveGame.Save($"data{SaveSlot}", SaveData, new SaveGameSimpleEncoder());
        }
    }
}