using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    private WorldGenOptions _options;
    
    [Header("UI Elements")]
    [Header("Main Menu")]
    [SerializeField] private TMP_Text title;
    [SerializeField] private Button singleplayerButton;

    [Header("World Menu")]
    [SerializeField] private GameObject worldMenuAnchor;
    [SerializeField] private TMP_InputField seedInput;
    [SerializeField] private TMP_Dropdown worldSizeChooser;

    private Animator _worldMenuAnimator;

    private void Awake()
    {
        _options = WorldGenOptions.FetchConfig();

        _worldMenuAnimator = worldMenuAnchor.GetComponent<Animator>();
    }
    public void LoadGame()
    {
        var size = worldSizeChooser.value;
        switch (size)
        {
            case 0:
            {
                _options.worldSize = new Vector2Int(250, 250);
                break;
            }
            case 1:
            {
                _options.worldSize = new Vector2Int(750, 750);
                break;
            }
            case 2:
            {
                _options.worldSize = new Vector2Int(1500, 1500);
                break;
            }
        }
        _options.seedString = seedInput.text;
        
        LoadingScreen.LoadGameWorld();
    }

    public void OpenWorldMenu()
    {
        _worldMenuAnimator.SetTrigger("Toggle");
        singleplayerButton.onClick.RemoveListener(OpenWorldMenu);
        singleplayerButton.onClick.AddListener(CloseWorldMenu);
    }

    public void CloseWorldMenu()
    {
        _worldMenuAnimator.SetTrigger("Toggle");
        singleplayerButton.onClick.RemoveListener(CloseWorldMenu);
        singleplayerButton.onClick.AddListener(OpenWorldMenu);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
