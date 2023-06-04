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
    public static event Action OnSceneTransition;

    private void Awake(){
        if (DestinationGate == $"{SceneManager.GetActiveScene().name}.{thisGateNumber}"){
            Player.Instance.transform.position = thisGatePosition;
            DestinationGate = "";
        }
    }

    private void OnTriggerEnter2D(Collider2D col){
        if (!col.CompareTag("Player")){ return; }
        OnSceneTransition?.Invoke();
        OnSceneTransition = null;
        DestinationGate = $"{destinationSceneName}.{destinationGateNumber}";
        SceneLoader.Instance.LoadScene(destinationSceneName);
        Player.Instance.gameObject.SetActive(false);
        Player.Instance.PlayerMovement.CancelDash();
        Player.Instance.transform.position = thisGatePosition;
    }

#if UNITY_EDITOR
    [SerializeField] private TextMeshProUGUI gateText;

    private void OnValidate(){
        string destinationText = $"-> {destinationSceneName}.{destinationGateNumber}";
        gateText.text = destinationText;
        name = $"Scene Transition {thisGateNumber} {destinationText}";
        if (Destination == Vector2.zero){ Destination = transform.position + Vector3.right; }
    }
    
    public Vector2 Destination{ get => thisGatePosition; set => thisGatePosition = value; }
#endif
}