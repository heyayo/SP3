using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gore : MonoBehaviour
{
    private int timer;

    // Start is called before the first frame update
    void Start()
    {
        timer = Random.Range(900, 1100);   
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        timer--;
        if (timer <= 0)
        {
            if (GetComponent<SpriteRenderer>().color.a > 0)
            {
                GetComponent<SpriteRenderer>().color -= new Color(0, 0, 0, 0.05f);
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }
}
