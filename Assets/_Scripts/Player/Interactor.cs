using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * This script handles interacting with the world
 * Actions Include:
 * Pickup
 * Use Ability (probably)
 * Take Environmental Damage
 */

public class Interactor : MonoBehaviour
{
    private Camera _cam;
    private Configuration _config;

    [SerializeField] private LayerMask interactLayer;
    [field:SerializeField] public float InteractDistance { get; private set; }

    private void Awake()
    {
        _cam = Camera.main;
        _config = Configuration.FetchConfig();
    }
    
    void Update()
    {
        if (Input.GetKeyDown(_config.attack))
            InteractRay();
    }

    public float GetFacingDirection()
    {
        return transform.localScale.x;
    }

    private void InteractRay()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, MousePosDelta(), InteractDistance,interactLayer);
        if (hit)
            hit.collider.GetComponent<Interactable>().OnInteract();
    }

    private Vector3 MousePosDelta()
    {
        var mousepos = Input.mousePosition;
        mousepos = _cam.ScreenToWorldPoint(mousepos);
        return mousepos - transform.position;
    }
}
