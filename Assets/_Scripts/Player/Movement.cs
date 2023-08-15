using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
public class Movement : MonoBehaviour
{
    private Rigidbody2D _rb;
    private Animator _animator;
    private Configuration _config;

    [field:SerializeField] public float XInput { get; private set; }
    [field:SerializeField] public float YInput { get; private set; }
    [field:SerializeField] public float MovementSpeed { get; private set; }
    [field:SerializeField] public float MaxSpeed { get; private set; }
    [field:SerializeField] public float RunModifier { get; private set; }
    
    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _config = Configuration.FetchConfig();
    }
    
    // Update is called once per frame
    void Update()
    {
        /*
        * Gather Input Values from Key Presses
        * "Convert.ToSingle" is to type cast into a float
        */
        XInput =
            Convert.ToSingle(Input.GetKey(_config.right)) -
            Convert.ToSingle(Input.GetKey(_config.left));
        YInput =
            Convert.ToSingle(Input.GetKey(_config.up)) -
            Convert.ToSingle(Input.GetKey(_config.down));
        if (Input.GetKey(_config.run))
        {
            XInput *= RunModifier;
            YInput *= RunModifier;
            _animator.speed = 2;
        }
        else _animator.speed = 1;
    }

    private void FixedUpdate()
    {
        // Apply Input Values as a Force to Move Player
        _rb.AddForce(new Vector2(XInput * MovementSpeed, YInput * MovementSpeed));

        // Clamping Velocity
        Vector2 vel = _rb.velocity;
        vel.x = Math.Clamp(vel.x, -MaxSpeed, MaxSpeed);
        vel.y = Math.Clamp(vel.y, -MaxSpeed, MaxSpeed);
        _rb.velocity = vel;
    }
}
