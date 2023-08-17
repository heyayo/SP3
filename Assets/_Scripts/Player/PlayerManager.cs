using System.Collections;
using System.Collections.Generic;
using System.IO;
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

    [Header("Body Parts")]
    [SerializeField] private SpriteRenderer _hood;
    [SerializeField] private SpriteRenderer _face;
    [SerializeField] private SpriteRenderer _torso;
    [SerializeField] private SpriteRenderer _pelvis;
    [SerializeField] private SpriteRenderer _leftShoulder;
    [SerializeField] private SpriteRenderer _leftHand;
    [SerializeField] private SpriteRenderer _leftBoot;
    [SerializeField] private SpriteRenderer _rightShoulder;
    [SerializeField] private SpriteRenderer _rightHand;
    [SerializeField] private SpriteRenderer _rightBoot;
    
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

    private Sprite LoadSprite(string name)
    {
        string path = Path.Combine("Sprites/Rogue", name);
        return Resources.Load<Sprite>(path);
    }

    private void UpdateSprites()
    {
        _hood.sprite = hoodSprite != null ? hoodSprite : LoadSprite("Rogue_hood_01");
        _face.sprite = faceSprite != null ? faceSprite : LoadSprite("Rogue_face_01");
        _torso.sprite = torsoSprite != null ? torsoSprite : LoadSprite("Rogue_torso_01");
        _pelvis.sprite = pelvisSprite != null ? pelvisSprite : LoadSprite("Rogue_pelvis_01");

        _leftShoulder.sprite = leftShoulderSprite != null ? leftShoulderSprite : LoadSprite("Rogue_shoulder_l_01");
        _leftHand.sprite = leftHandSprite != null ? leftHandSprite : LoadSprite("Rogue_elbow_l_01");
        _leftBoot.sprite = leftBootSprite != null ? leftBootSprite : LoadSprite("Rogue_boot_l_01");
        
        _rightShoulder.sprite = rightShoulderSprite != null ? rightShoulderSprite : LoadSprite("Rogue_shoulder_r_01");
        _rightHand.sprite = rightHandSprite != null ? rightHandSprite : LoadSprite("Rogue_elbow_r_01");
        _rightBoot.sprite = rightBootSprite != null ? rightBootSprite : LoadSprite("Rogue_boot_r_01");
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
