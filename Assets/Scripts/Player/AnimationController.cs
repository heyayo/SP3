using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
public class AnimationController : MonoBehaviour
{
    private Rigidbody2D _rb;
    private Animator _animator;

    private int _hashXVelocity;
    private int _hashYVelocity;
    
    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();

        _hashXVelocity = Animator.StringToHash("xVelocity");
        _hashYVelocity = Animator.StringToHash("yVelocity");
    }

    private void Update()
    {
        // Walking animation and idle animation and runnning aniamtion
        _animator.SetFloat(_hashXVelocity, _rb.velocity.x);
        _animator.SetFloat(_hashYVelocity, _rb.velocity.y);
    }
    // Not Unity Functions
}
