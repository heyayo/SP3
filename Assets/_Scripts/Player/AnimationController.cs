using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
public class AnimationController : MonoBehaviour
{
    private Rigidbody2D _rb;
    private Animator _animator;
    private Vector3 _scale;

    private int _hashXVelocity;
    private int _hashYVelocity;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();

        _hashXVelocity = Animator.StringToHash("xVelocity");
        _hashYVelocity = Animator.StringToHash("yVelocity");

        _scale = transform.localScale;
    }

    private void Update()
    {
        Vector2 velocity = _rb.velocity;
        // Walking animation and idle animation and runnning aniamtion
        _animator.SetFloat(_hashXVelocity, velocity.x);
        _animator.SetFloat(_hashYVelocity, velocity.y);

        Vector3 dir = _scale;
        if (velocity.x > 0)
            dir.x = _scale.x;
        else if (velocity.x < 0)
            dir.x = -_scale.x;

        transform.localScale = dir;
    }
    // Not Unity Functions
}
