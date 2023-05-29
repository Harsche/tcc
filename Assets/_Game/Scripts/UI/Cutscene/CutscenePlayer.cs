using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Video;

public class CutscenePlayer : MonoBehaviour{
    [SerializeField] private VideoPlayer videoPlayer;
    [SerializeField] private UnityEvent onVideoEnd;

    private void Start(){
        StartCoroutine(WaitForEndOfVideo());
    }

#if UNITY_EDITOR
    private void OnValidate(){
        if (!videoPlayer){ videoPlayer = GetComponent<VideoPlayer>(); }
    }
#endif

    private IEnumerator WaitForEndOfVideo(){
        yield return new WaitForSeconds((float) videoPlayer.length);
        onVideoEnd?.Invoke();
    }
}