using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;
using Game.SaveSystem.CustomConverters;

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

    [JsonProperty]
    private Dictionary<string, Dictionary<string, object>> uniqueObjectsData = new();

    public void AddUniqueObjectData(string key, Dictionary<string, object> value){
        if (uniqueObjectsData.ContainsKey(key)){
            uniqueObjectsData[key] = value;
            return;
        }
        uniqueObjectsData.Add(key, value);
    }

    public bool TryGetUniqueObjectData(string key, out Dictionary<string, object> data){
        if (uniqueObjectsData.TryGetValue(key, out Dictionary<string, object> value)){
            data = value;
            return true;
        }
        data = null;
        return false;
    }
}