using System.IO;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;

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

    private Animator _animator;

    [Header("Player Specific Attack Stats")]
    private float _attackStat;
    private float _nativeAttackStat;
    public float AttackDamage
    {
        get => _attackStat;
        set
        {
            _attackStat = value;
        }
    }

    public float __NativeAttackDamage
    {
        get => _nativeAttackStat;
        set
        {
            float delta = value - _nativeAttackStat;
            AttackDamage += delta;
            _nativeAttackStat = value;
        }
    }

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
        _animator = GetComponent<Animator>();
    }

    private void Start()
    {
        UpdateSprites();
        MortalityScript.onHealthZero.AddListener(KillPlayer);
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

    [ContextMenu("Kill")]
    public void KillPlayer()
    {
        FreezePlayer();
        AnimationScript.enabled = false;
        _animator.CrossFade(AnimationController.animDeath,0,0);
        WaitForDeath();
    }

    public void RespawnPlayer(Vector2 position = new Vector2())
    {
        UnFreezePlayer();
        AnimationScript.enabled = true;
        transform.position = position;
        gameObject.SetActive(true);
        MortalityScript.ResetToMax();
    }

    [ContextMenu("Respawn")]
    private void Respawn()
    {
        RespawnPlayer();
    }
    
    private async void WaitForDeath()
    {
        await Task.Delay((int)(AnimationController.durDeath * 1000f));
        gameObject.SetActive(false);
    }

    [ContextMenu("Give SLE")]
    private void SpawnEOC()
    {
        InventoryManager.Instance.Add(Resources.Load<Item>("Items/BossSummons/SuspiciousLookingEye"));
    }
    [ContextMenu("Give BS")]
    private void SpawnBS()
    {
        InventoryManager.Instance.Add(Resources.Load<Item>("Items/BossSummons/BloodySpine"));
    }
    [ContextMenu("Give Nasus E Skill")]
    private void SpawnNasus()
    {
        InventoryManager.Instance.Add(Resources.Load<Item>("Items/Weapons/NasusESkill"));
    }
}
