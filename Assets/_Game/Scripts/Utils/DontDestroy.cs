using System.Collections.Generic;
using UnityEngine;

public class DontDestroy : MonoBehaviour{
    [SerializeField] private GameObject[] dontDestroy;
    public static List<GameObject> PersistantObjects{ get; } = new();

    private void Awake(){
        foreach (GameObject go in dontDestroy){
            DontDestroyOnLoad(go);
            PersistantObjects.Add(go);
        }
    }
}