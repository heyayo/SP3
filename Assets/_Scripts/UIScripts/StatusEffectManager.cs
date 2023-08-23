using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class StatusEffectManager : MonoBehaviour
{
    private Mortality _mortality;
    private ObjectPool<StatusEffect> _uiIcons;
    private List<StatusEffect> _activeIcons;
    [SerializeField] private StatusEffect uiPrefab;

    private const float _startPosX = -150f;
    private const float _startPosY = 50f;
    private const float _endPosX = 150f;
    private const float _endPosY = -30f;

    private void Awake()
    {
        _uiIcons = new ObjectPool<StatusEffect>(
            () => { return Instantiate(uiPrefab); },
            icon => { icon.gameObject.SetActive(true); },
            icon => { icon.gameObject.SetActive(false); },
            icon => { Destroy(icon.gameObject); }
        );
        _activeIcons = new List<StatusEffect>();
    }
    
    // Start is called before the first frame update
    void Start()
    {
        _mortality = PlayerManager.Instance.MortalityScript;
        
        _mortality.onAfflictionAdd.AddListener(RespawnIcons);
        _mortality.onAfflictionExpire.AddListener(RespawnIcons);
    }

    private void RespawnIcons()
    {
        Debug.Log("RESPAWNING ICONS");
        for (int i = 0; i < _activeIcons.Count; ++i)
        {
            _uiIcons.Release(_activeIcons[i]);
        }

        _activeIcons.Clear();

        int count = 0;
        var afflictions = _mortality.GetAfflictions();
        foreach (var af in afflictions)
        {
            var icon = _uiIcons.Get();
            icon.affliction = af;
            icon.transform.parent = gameObject.transform;
            icon.transform.localPosition = new Vector2(_startPosX + (50 * count),_startPosY + ((count / 6) * -40));
            _activeIcons.Add(icon);
            ++count;
        }
    }
}
