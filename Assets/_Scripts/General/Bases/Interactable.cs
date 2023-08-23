using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Interactable : MonoBehaviour
{
    public abstract void OnInteract();
    public abstract void OnMouseEnter();
    public abstract void OnMouseExit();

    private Configuration _config;
    private Transform _player;

    private void Awake()
    {
        _config = Configuration.FetchConfig();
    }

    private void Start()
    {
        _player = PlayerManager.Instance.transform;
    }
    
    public void OnMouseOver()
    {
        if (Input.GetKeyDown(_config.interact))
        {
            Vector2 d2p = _player.position - transform.position;
            float d2pf = d2p.magnitude;
            if (d2pf <= 6)
                OnInteract();
        }
    }
}
