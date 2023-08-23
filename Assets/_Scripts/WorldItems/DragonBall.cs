using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DragonBall : Interactable
{
    public override void OnInteract()
    {
        List<GameObject> dragonBalls = new List<GameObject>();
        int count = 0;
        Collider2D[] otherDragonBalls = Physics2D.OverlapCircleAll(transform.position, 8);
        foreach (var ball in otherDragonBalls)
        {
            if (ball.CompareTag("DragonBall"))
            {
                ++count;
                dragonBalls.Add(ball.gameObject);
            }
        }
        if (count >= 7)
        {
            foreach (var ball in dragonBalls)
            {
                Destroy(ball);
            }

            ShenronSummoner.Instance.EnableScreen();
        }
    }

    public override void OnMouseEnter()
    {
    }

    public override void OnMouseExit()
    {
    }
}
