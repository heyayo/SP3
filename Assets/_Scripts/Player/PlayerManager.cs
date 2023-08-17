using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

[RequireComponent(typeof(Mortality))]
[RequireComponent(typeof(Interactor))]
[RequireComponent(typeof(Movement))]
public class PlayerManager : MonoBehaviour
{
    public static PlayerManager Instance { get; private set; }
    
    private Mortality _mortality;
    private Interactor _interactor;
    private Movement _movement;

    private SpriteRenderer _hood;
    private SpriteRenderer _face;
    private SpriteRenderer _torso;
    private SpriteRenderer _pelvis;
    private SpriteRenderer _leftShoulder;
    private SpriteRenderer _leftHand;
    private SpriteRenderer _leftBoot;
    private SpriteRenderer _rightShoulder;
    private SpriteRenderer _rightHand;
    private SpriteRenderer _rightBoot;
    
    public static Sprite hoodSprite;
    public static Sprite faceSprite;
    public static Sprite torsoSprite;
    public static Sprite pelvisSprite;
    public static Sprite leftShoulderSprite;
    public static Sprite leftHandSprite;
    public static Sprite leftBootSprite;
    public static Sprite rightShoulderSprite;
    public static Sprite rightHandSprite;
    public static Sprite rightBootSprite;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("Multiple Players Detected, MULTIPLAYER NOT ACTIVE");
            Debug.Break();
        }
        Instance = this;
        
        _mortality = GetComponent<Mortality>();
        _interactor = GetComponent<Interactor>();
        _movement = GetComponent<Movement>();

        gameObject.tag = "Player";
    }

    private void Start()
    {
        UpdateSprites();
    }

    private void UpdateSprites()
    {
        _hood.sprite = hoodSprite;
        _face.sprite = faceSprite;
        _torso.sprite = torsoSprite;
        _pelvis.sprite = pelvisSprite;

        _leftShoulder.sprite = leftShoulderSprite;
        _leftHand.sprite = leftHandSprite;
        _leftBoot.sprite = leftBootSprite;
        
        _rightShoulder.sprite = rightShoulderSprite;
        _rightHand.sprite = rightHandSprite;
        _rightBoot.sprite = rightBootSprite;
    }

    public void FreezePlayer()
    {
        _mortality.enabled = false;
        _movement.enabled = false;
        _interactor.enabled = false;
    }

    public void UnFreezePlayer()
    {
        _mortality.enabled = true;
        _movement.enabled = true;
        _interactor.enabled = true;
    }
}
