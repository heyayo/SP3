using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterSlowTilemap : MonoBehaviour
{
    private Rigidbody2D _player;
    private float _defaultLinearDrag;

    private void Start()
    {
        _player = PlayerManager.Instance.gameObject.GetComponent<Rigidbody2D>();
        _defaultLinearDrag = _player.drag;
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Water");
        _player.drag = _defaultLinearDrag * 2;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        _player.drag = _defaultLinearDrag;
    }
}
