using UnityEngine;

public class SceneTransition : MonoBehaviour{
    [SerializeField] private string sceneName;
    [SerializeField] private Vector2 destination;

    private void OnTriggerEnter2D(Collider2D col){
        if (!col.CompareTag("Player")){ return; }
        SceneLoader.Instance.LoadScene(sceneName);
        Player.Instance.gameObject.SetActive(false);
        Player.Instance.transform.position = destination;
    }
}