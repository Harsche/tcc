using UnityEngine;

public class Map : MonoBehaviour{
    [SerializeField] private Transform mapContent;
    [SerializeField] private Vector2 minMaxMapZoom;
    
    private int mapZoom;
    private Coroutine zoomCoroutine;
    
    public void OnZoomOut(){
        // Vector2 zoomPosition  = 
        // if (zoomCoroutine != null){ }
        // zoomCoroutine = new
    }

    public void OnZoomIn(){
        // if (zoomCoroutine != null){ return; }
        // if (mapZoom > minMaxMapZoom.x && mapZoom < minMaxMapZoom.y){ }
        // zoomCoroutine = StartCoroutine(ZoomCoroutine(true));
    }
}