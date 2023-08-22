using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class ReactiveBars : MonoBehaviour
{
    [Header("Player Script")]
    [SerializeField] private Mortality mortality;
    
    [Header("Value Bars")]
    [SerializeField] private RectTransform HPBar;
    [SerializeField] private RectTransform SEBar;
    [SerializeField] private RectTransform AEBar;

    [Header("Paused Value Bars")]
    [SerializeField] private RectTransform PausedHPBar;
    [SerializeField] private RectTransform PausedSEBar;
    [SerializeField] private RectTransform PausedAEBar;

    [Header("Customization")]
    [SerializeField] private float shrinkSpeed;

    private const float _fullBarLength = 250f;

    // Start is called before the first frame update
    void Start()
    {
        mortality = PlayerManager.Instance.MortalityScript;

        mortality.onHealthAdjust.AddListener(ResetHPTimer);
        mortality.onStoredEnergyAdjust.AddListener(ResetSETimer);
        mortality.onActiveEnergyAdjust.AddListener(ResetAETimer);
    }

    // Update is called once per frame
    void Update()
    {
        LerpBar(ref PausedHPBar, mortality.__HealthMax, mortality.Health);
        LerpBar(ref PausedSEBar, mortality.__StoredEnergyMax,mortality.StoredEnergy);
        LerpBar(ref PausedAEBar, mortality.__ActiveEnergyMax, mortality.ActiveEnergy);
    }

    private void ResetHPTimer()
    {
        ResizeBar(ref HPBar, mortality.__HealthMax,mortality.Health);
    }

    private void ResetSETimer()
    {
        ResizeBar(ref SEBar,mortality.__StoredEnergyMax,mortality.StoredEnergy);
    }

    private void ResetAETimer()
    {
        ResizeBar(ref AEBar, mortality.__ActiveEnergyMax,mortality.ActiveEnergy);
    }

    private void ResizeBar(ref RectTransform bar, float max, float current)
    {
        float barSize = (_fullBarLength / max) * current;
        float newPos = 25 + (barSize / 2);
        bar.sizeDelta = new Vector2(barSize, bar.rect.height);
        bar.localPosition = new Vector3(newPos, 0, 0);
    }

    private void LerpBar(ref RectTransform bar, float max, float target)
    {
        float barSize = (_fullBarLength / max) * target;
        float lerpedBarSize = Mathf.MoveTowards(bar.sizeDelta.x, barSize, shrinkSpeed * Time.deltaTime);
        if (lerpedBarSize < barSize)
            lerpedBarSize = barSize;
        float pos = 25 + (lerpedBarSize / 2);
        bar.sizeDelta = new Vector2(lerpedBarSize, 25);
        bar.localPosition = new Vector3(pos, 0, 0);
    }
}
