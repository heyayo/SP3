using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.Serialization;

[RequireComponent(typeof(Mortality))]
[RequireComponent(typeof(Interactor))]
[RequireComponent(typeof(Movement))]
public class PlayerManager : MonoBehaviour
{
    public static PlayerManager Instance { get; private set; }
    
    [field:SerializeField] public Mortality MortalityScript { get; private set; }
    [field:SerializeField] public Movement MovementScript { get; private set; }
    [field:SerializeField] public AnimationController AnimationScript { get; private set; }

    [Header("Body Parts")]
    [SerializeField] private SpriteRenderer hood;
    [SerializeField] private SpriteRenderer face;
    [SerializeField] private SpriteRenderer torso;
    [SerializeField] private SpriteRenderer pelvis;
    [SerializeField] private SpriteRenderer leftShoulder;
    [SerializeField] private SpriteRenderer leftHand;
    [SerializeField] private SpriteRenderer leftBoot;
    [SerializeField] private SpriteRenderer rightShoulder;
    [SerializeField] private SpriteRenderer rightHand;
    [SerializeField] private SpriteRenderer rightBoot;
    
    public static Sprite HoodSprite;
    public static Sprite FaceSprite;
    public static Sprite TorsoSprite;
    public static Sprite PelvisSprite;
    public static Sprite LeftShoulderSprite;
    public static Sprite LeftHandSprite;
    public static Sprite LeftBootSprite;
    public static Sprite RightShoulderSprite;
    public static Sprite RightHandSprite;
    public static Sprite RightBootSprite;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("Multiple Players Detected, MULTIPLAYER NOT ACTIVE");
            Debug.Break();
        }
        Instance = this;
        
        MortalityScript = GetComponent<Mortality>();
        MovementScript = GetComponent<Movement>();
        AnimationScript = GetComponent<AnimationController>();
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
        hood.sprite = HoodSprite != null ? HoodSprite : LoadSprite("Rogue_hood_01");
        face.sprite = FaceSprite != null ? FaceSprite : LoadSprite("Rogue_face_01");
        torso.sprite = TorsoSprite != null ? TorsoSprite : LoadSprite("Rogue_torso_01");
        pelvis.sprite = PelvisSprite != null ? PelvisSprite : LoadSprite("Rogue_pelvis_01");

        leftShoulder.sprite = LeftShoulderSprite != null ? LeftShoulderSprite : LoadSprite("Rogue_shoulder_l_01");
        leftHand.sprite = LeftHandSprite != null ? LeftHandSprite : LoadSprite("Rogue_elbow_l_01");
        leftBoot.sprite = LeftBootSprite != null ? LeftBootSprite : LoadSprite("Rogue_boot_l_01");
        
        rightShoulder.sprite = RightShoulderSprite != null ? RightShoulderSprite : LoadSprite("Rogue_shoulder_r_01");
        rightHand.sprite = RightHandSprite != null ? RightHandSprite : LoadSprite("Rogue_elbow_r_01");
        rightBoot.sprite = RightBootSprite != null ? RightBootSprite : LoadSprite("Rogue_boot_r_01");
    }

    public void FreezePlayer()
    {
        MortalityScript.enabled = false;
        MovementScript.enabled = false;
    }

    public void UnFreezePlayer()
    {
        MortalityScript.enabled = true;
        MovementScript.enabled = true;
    }
}
