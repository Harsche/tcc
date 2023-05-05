using TMPro;
using UnityEngine;

public class SceneTransition : MonoBehaviour{
    [SerializeField] private string sceneName;
    [SerializeField, Range(0, 20)] private int gateNumber;
    [SerializeField] private Vector2 destination;

    public static string DestinationGate{ get; private set; }

    private void OnTriggerEnter2D(Collider2D col){
        if (!col.CompareTag("Player")){ return; }
        DestinationGate = $"{sceneName}.{gateNumber}";
        SceneLoader.Instance.LoadScene(sceneName);
        Player.Instance.gameObject.SetActive(false);
        Player.Instance.transform.position = destination;
    }

#if UNITY_EDITOR
    [SerializeField] private TextMeshProUGUI gateText;
    [SerializeField] private SceneGate gate;

    private void OnValidate(){
        gateText.text = $"-> {sceneName}.{gateNumber}";
    }
#endif
}