using UnityEngine;

public class DontDestroy : MonoBehaviour{
    [SerializeField] private GameObject[] dontDestroy;

    private void Awake(){
        foreach (GameObject go in dontDestroy){ DontDestroyOnLoad(go); }
    }
}