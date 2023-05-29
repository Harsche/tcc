using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;

public class SaveData{
    // Important variables

    public bool isNewGame = true;
    public Vector3 playerPosition;
    public string loadScene;
    public TimeSpan playTime;
    public string storyJson;

    // Skills

    public bool unlockParry;
    public bool unlockDash;
    public bool unlockWallJump;

    // Unique objects 

    [JsonProperty] private Dictionary<string, object> uniqueObjectsData = new();

    public void AddUniqueObjectData(string key, object value){
        if (uniqueObjectsData.ContainsKey(key)){
            uniqueObjectsData[key] = value;
            return;
        }
        uniqueObjectsData.Add(key, value);
    }

    public bool TryGetUniqueObjectData<T>(string key, out T data){
        if (uniqueObjectsData.TryGetValue(key, out object value)){
            data = (T) value;
            return true;
        }
        data = default;
        return false;
    }
}