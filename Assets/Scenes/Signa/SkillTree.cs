using UnityEngine;
using UnityEngine.UI;

public class SkillTree : MonoBehaviour
{
    public Button button1;
    public Button button2;
    public Button button3;

    private bool isButton1Clicked = false;
    private bool Unlocked1 = false;

    

    [SerializeField] GameObject player;



    private void Start()
    {
      
        // Add click listeners to the buttons
        button1.onClick.AddListener(OnClickButton1);
        button2.onClick.AddListener(OnClickButton2);
        button3.onClick.AddListener(OnClickButton3);
    }


    public void OnClickButton1()
    {
        if (!isButton1Clicked)
        {
            // Enable buttons 2 and 3
            button2.gameObject.SetActive(true);
            button3.gameObject.SetActive(true);

            // Update the state
            isButton1Clicked = true;

            Unlocked1 =  true;
            // Deactivate button 1
            button1.gameObject.SetActive(false);
        }
    }

    public void OnClickButton2()
    {
        if (Unlocked1 == true)
        {
           
            // Deactivate button 2
            button2.gameObject.SetActive(false);
        }
    }

        public void OnClickButton3()
    {

        if (Unlocked1 == true)
        {
            // Deactivate button 3
            button3.gameObject.SetActive(false);
        }
        }
    }