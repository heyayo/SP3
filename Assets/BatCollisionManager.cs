using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatCollisionManager : MonoBehaviour
{
    [HideInInspector] public bool isStuck = false;
    [HideInInspector] public Vector2 prev;
    public UnicornEnemy unicorn;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        prev = collision.transform.position;
        isStuck = true;
    }

    private void Start()
    {
        GetComponent<Mortality>().onHealthZero.AddListener(() =>
        {
            GameObject.Destroy(unicorn.bubblee);
            GameObject.Destroy(unicorn.gameObject);
            unicorn.stuckText.SetActive(false);
            StartCoroutine(WaitThenExplode());
        });
    }

    IEnumerator WaitThenExplode()
    {
        GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        GetComponent<Animator>().SetTrigger("isExplode");
        yield return new WaitForSeconds(0.5f);
        InventoryManager.Instance.stuckTexta.SetActive(false);
        Destroy(gameObject);
    }
}
