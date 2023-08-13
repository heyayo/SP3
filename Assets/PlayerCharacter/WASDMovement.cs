using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Rigidbody2D))]
public class WASDMovement : MonoBehaviour
{
    private Rigidbody2D _rb;
    private SpriteRenderer _sprite;

    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _sprite = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        
    }
}
