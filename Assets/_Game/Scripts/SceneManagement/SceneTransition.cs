using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class SceneTransition : MonoBehaviour{
    [SerializeField, Range(0, 20)] private int thisGateNumber;
    [FormerlySerializedAs("destination"),SerializeField] private Vector2 thisGatePosition;
    [FormerlySerializedAs("sceneName"),SerializeField] private string destinationSceneName;
    [SerializeField, Range(0, 20)] private int destinationGateNumber;

    private static string DestinationGate;

    private void Awake(){
        if (DestinationGate == $"{SceneManager.GetActiveScene().name}.{thisGateNumber}"){
            Player.Instance.transform.position = thisGatePosition;
        }
    }

    private void OnTriggerEnter2D(Collider2D col){
        if (!col.CompareTag("Player")){ return; }
        DestinationGate = $"{destinationSceneName}.{destinationGateNumber}";
        SceneLoader.Instance.LoadScene(destinationSceneName);
        Player.Instance.gameObject.SetActive(false);
        Player.Instance.transform.position = thisGatePosition;
    }

#if UNITY_EDITOR
    [SerializeField] private TextMeshProUGUI gateText;

    private void OnValidate(){
        gateText.text = $"-> {destinationSceneName}.{destinationGateNumber}";
        if (Destination == Vector2.zero){ Destination = transform.position + Vector3.right; }
    }
    
    public Vector2 Destination{ get => thisGatePosition; set => thisGatePosition = value; }
#endif
}