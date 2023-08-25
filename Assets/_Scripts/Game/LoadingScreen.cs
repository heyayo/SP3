using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadingScreen : MonoBehaviour
{
    public static string ToLoad;

    private AsyncOperation _sceneLoading;
    
    [Header("UI Elements")]
    [SerializeField] private TMP_Text percentage;
    
    void Start()
    {
        if (ToLoad == null)
        {
            Debug.LogError("NULL SCENE");
        }
        _sceneLoading = SceneManager.LoadSceneAsync(ToLoad);
    }

    private void Update()
    {
        if (_sceneLoading.isDone)
        {
            ToLoad = null;
            Debug.Log("Load End");
        }
        else
        {
            int progressPercentage = (int)(_sceneLoading.progress * 100);
            percentage.text = progressPercentage.ToString() + "%";
        }
    }
    
    public static void LoadScene(string scene)
    {
        LoadingScreen.ToLoad = scene;
        SceneManager.LoadScene("LoadingScreen");
    }

    public static void LoadGameWorld()
    {
        LoadScene("TheRealGame");
    }

    public static void LoadPlayerCustomizer()
    {
        LoadScene("Customizer");
    }
}
