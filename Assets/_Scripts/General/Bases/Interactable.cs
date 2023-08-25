using UnityEngine;

public abstract class Interactable : MonoBehaviour
{
    public abstract void OnInteract();
    public abstract void OnMouseEnter();
    public abstract void OnMouseExit();

    private Configuration _config;
    protected Mortality _mortality;

    private void Awake()
    {
        _config = Configuration.FetchConfig();
        _mortality = GetComponent<Mortality>();
    }
    
    public void OnMouseOver()
    {
        if (Input.GetKeyDown(_config.interact))
        {
            Vector2 d2p = PlayerManager.Instance.transform.position - transform.position;
            float d2pf = d2p.magnitude;
            if (d2pf <= 6)
                OnInteract();
        }
    }
}
