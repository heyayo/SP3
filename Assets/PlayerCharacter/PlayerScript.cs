using System;
using UnityEngine;

// Unity ScriptTags to auto add Components
[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Mortality))]
public class PlayerScript : MonoBehaviour
{
    private Configuration _config;
    private Rigidbody2D _rb;
    private SpriteRenderer _sprite;

    [SerializeField] private float movementSpeed = 1f;
    [SerializeField] private float maxSpeed = 5f;

    private float _xInput;
    private float _yInput;
    
    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _sprite = GetComponent<SpriteRenderer>();
        _config = Configuration.FetchConfig();
    }

    private void Update()
    {
        /*
         * Gather Input Values from Key Presses
         * "Convert.ToSingle" is to type cast into a float
         */
        _xInput =
            Convert.ToSingle(Input.GetKey(_config.Right)) -
            Convert.ToSingle(Input.GetKey(_config.Left));
        _yInput =
            Convert.ToSingle(Input.GetKey(_config.Up)) -
            Convert.ToSingle(Input.GetKey(_config.Down));
    }

    private void FixedUpdate()
    {
        // Apply Input Values as a Force to Move Player
        _rb.AddForce(new Vector2(_xInput * movementSpeed, _yInput * movementSpeed));

        /*
         * Clamping Velocity
         */
        Vector2 vel = _rb.velocity;
        vel.x = Math.Clamp(vel.x, -maxSpeed, maxSpeed);
        vel.y = Math.Clamp(vel.y, -maxSpeed, maxSpeed);
        _rb.velocity = vel;
    }
    
    // Not Unity Functions
}
