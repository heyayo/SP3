using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutfitChanger : MonoBehaviour
{
    [Header("Sprite Changer")]
    public SpriteRenderer bodyPart;
    [Header("Sprite Options")]
    public List<Sprite> options = new List<Sprite>();

    private int _currentOption = 0;
    
    public void NextOption() 
    { 
        _currentOption++;
        if (_currentOption >= options.Count) _currentOption = 0; // Check if current option index exceeds the size of the list
        bodyPart.sprite = options[_currentOption];
    }

    public void PreviousOption()
    {
        _currentOption--;
        if (_currentOption <= 0) _currentOption = options.Count - 1;
        bodyPart.sprite = options[_currentOption];
    }

    public void Randomise()
    {
        _currentOption = Random.Range(0, options.Count); // Get a random number between 0 and the number of elements in the list
        bodyPart.sprite = options[_currentOption];
    }
}
 