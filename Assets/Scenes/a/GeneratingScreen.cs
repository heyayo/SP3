using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GeneratingScreen : MonoBehaviour
{
    private WorldGenerator _worldGen;

    [Header("UI Elements")]
    [SerializeField] private TMP_Text generating;
    [SerializeField] private TMP_Text percentage;

    void Start()
    {
        _worldGen = WorldGenerator.instance;

        generating.text = "GENERATING";
        percentage.text = "0%";
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
