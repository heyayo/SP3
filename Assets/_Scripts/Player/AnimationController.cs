using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
public class AnimationController : MonoBehaviour
{
    private Configuration _config;

    private Rigidbody2D _rb;
    private Animator _animator;
    private Movement _movement;
    private Vector3 _scale;
    private float _lockTime;
    private int _animState;

    private static readonly int _animIdle = Animator.StringToHash("Idle");
    private static readonly int _animWalk = Animator.StringToHash("Walk");
    private static readonly int _animAttackOne = Animator.StringToHash("AttackOne");
    private static readonly int _animAttackTwo = Animator.StringToHash("AttackTwo");
    public static readonly int animDeath = Animator.StringToHash("Death");

    private static float _durAttackOne;
    private static float _durAttackTwo;
    public static float durDeath;

    private void Awake()
    {
        _config = Configuration.FetchConfig();

        _rb = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _movement = GetComponent<Movement>();

        _scale = transform.localScale;
    }

    private void Start()
    {
        AnimationClip[] clips = _animator.runtimeAnimatorController.animationClips;
        foreach (var clip in clips)
        {
            switch (clip.name)
            {
                case "AttackSecondHalf":
                    _durAttackTwo = clip.length;
                    break;
                case "Death":
                    durDeath = clip.length;
                    break;
            }
        }
    }

    private void Update()
    {
        Vector2 velocity = _rb.velocity;
        Vector3 dir = _scale;
        if (velocity.x > 0)
            dir.x = _scale.x;
        else if (velocity.x < 0)
            dir.x = -_scale.x;

        transform.localScale = dir;

        // Handle Animation Changes
        int state = GetState();
        if (state == _animState) return;
        _animator.CrossFade(state, 0, 0);
        _animState = state;
    }
    // Not Unity Functions

    private int GetState()
    {
        if (Time.time < _lockTime) return _animState;
        if (Input.GetKey(_config.attack)) return _animAttackOne;
        if (Input.GetKeyUp(_config.attack)) return LockState(_animAttackTwo, _durAttackTwo);
        if (_movement.XInput != 0 || _movement.YInput != 0) return _animWalk;

        return _animIdle;
    }

    public int LockState(int state, float time)
    {
        _lockTime = Time.time + time;
        return state;
    }
}
