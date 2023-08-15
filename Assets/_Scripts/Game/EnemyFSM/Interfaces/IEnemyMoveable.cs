using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEnemyMoveable
{
    Rigidbody2D _rb { get; set; }
    bool isFacingRight { get; set; }

    void MoveEnemy(Vector2 velocity);
    void CheckLeftOrRightFacing(Vector2 velocity);
}
