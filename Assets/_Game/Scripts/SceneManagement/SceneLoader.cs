using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Game.SaveSystem;

public class SceneLoader : MonoBehaviour{
    [SerializeField] private float fadeTime = 1f;
    [SerializeField] private Ease fadeEase = Ease.Linear;
    [SerializeField] private Slider progressBar;
    [SerializeField] private string setupSceneName = "SetupScene";

    private CanvasGroup canvasGroup;
    private Coroutine loadCoroutine;

    public static SceneLoader Instance{ get; private set; }

    private void Awake(){
        if (Instance != null){
            Destroy(this);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(this);
        canvasGroup = GetComponent<CanvasGroup>();
    }

    public void LoadSetupScene(){
        loadCoroutine = StartCoroutine(LoadSceneCoroutine(setupSceneName));
    }

    public void LoadScene(string sceneName){
        loadCoroutine = StartCoroutine(LoadSceneCoroutine(sceneName));
    }

    private Tweener Fade(bool inOrOut){
        float endValue = inOrOut ? 1f : 0f;
        Tweener tweener = canvasGroup.DOFade(endValue, fadeTime)
            .SetEase(fadeEase);
        return tweener;
    }

    private IEnumerator LoadSceneCoroutine(string sceneName){
        yield return Fade(true)
            .WaitForCompletion();
        AsyncOperation loadSceneAsync = SceneManager.LoadSceneAsync(sceneName);
        while (!loadSceneAsync.isDone){
            progressBar.value = loadSceneAsync.progress;
            yield return null;
        }
        progressBar.value = loadSceneAsync.progress;
        if (sceneName == setupSceneName){
            foreach (GameObject rootGameObject in SceneManager.GetActiveScene().GetRootGameObjects()){
                Player.Instance.gameObject.SetActive(false);
                DontDestroyOnLoad(rootGameObject);
            }
            string scene = SaveSystem.SaveData.loadScene;
            if (string.IsNullOrEmpty(scene)){ scene = "A1"; }
            LoadScene(scene);
            SaveSystem.SetLastSaveTime();
            yield break;
        }
        Player.Instance.gameObject.SetActive(true);
        yield return Fade(false).WaitForCompletion();
    }
}