using TMPro;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private TMP_Text title;

    public void LoadGame()
    {
        LoadingScreen.LoadGameWorld();
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
