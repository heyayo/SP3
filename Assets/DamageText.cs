using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DamageText : MonoBehaviour
{
    [SerializeField]
    private GameObject damageTextPrefab;

    private void Start()
    {
        GetComponent<Damagable>().onHit.AddListener(DmgText);
    }

    private void DmgText()
    {
        GameObject text = Instantiate(damageTextPrefab, transform.position, Quaternion.identity);
        text.GetComponent<TMP_Text>().text = "POW";
    }
}
