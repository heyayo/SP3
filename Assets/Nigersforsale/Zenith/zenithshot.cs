//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class ZenithShot : MonoBehaviour
//{
//    public float upwardForce = 10f;
//    public float downwardForce = 5f; // New variable for downward force
//    public float moveSpeed = 5f;

//    private Rigidbody2D rb;
//    private bool touchedCursor = false;
//    private Vector3 playerPosition;

//    private bool boom = false;
//    private GameObject player;

//    private void Start()
//    {
//        player = PlayerManager.Instance.gameObject;
//        rb = GetComponent<Rigidbody2D>();
//        rb.AddForce(Vector2.up * upwardForce, ForceMode2D.Impulse);
//    }

//    private void Update()
//    {
//        if (!touchedCursor)
//        {
//            if (boom == false)
//            {
//                rb.AddForce(Vector2.down * -upwardForce, ForceMode2D.Impulse);
//                boom = true;
//            }
//            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
//            mousePosition.z = 0f;
//            Vector2 direction = (mousePosition - transform.position).normalized;

//            rb.velocity = direction * moveSpeed;

//            if (Vector2.Distance(transform.position, mousePosition) < 1.0f)
//            {
//                boom = true;
//                touchedCursor = true;
//                //rb.velocity = Vector2.zero;

//                playerPosition = player.transform.position;

//            }
//        }

//        if (touchedCursor == true)
//        {

//            if (boom == false)
//            {
//                rb.AddForce(Vector2.down * upwardForce, ForceMode2D.Impulse);
//                boom = true;
//            }
//            Vector2 direction = (playerPosition - transform.position).normalized;
//            rb.velocity = direction * moveSpeed;

//            if (Vector2.Distance(transform.position, playerPosition) < 1.0f)
//            {
//                boom = false;
//                touchedCursor = false; // Switch back to following the cursor


//            }
//        }
//    }
//}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZenithShot : MonoBehaviour
{
    public float upwardForce = 10f;
    public float downwardForce = 5f;
    public float moveSpeed = 5f;
    public float curveStrength = 2f; // Adjust this value to control the curve strength

    private Rigidbody2D rb;
    private bool touchedCursor = false;
    private Vector3 playerPosition;

    private bool boom = false;
    private GameObject player;

    private void Start()
    {
        player = PlayerManager.Instance.gameObject;
        rb = GetComponent<Rigidbody2D>();
        rb.AddForce(Vector2.up * upwardForce, ForceMode2D.Impulse);
    }

    private void Update()
    {
        if (!touchedCursor)
        {
            if (!boom)
            {
                rb.AddForce(Vector2.down * -upwardForce, ForceMode2D.Impulse);
                boom = true;
            }

            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePosition.z = 0f;
            Vector2 direction = (mousePosition - transform.position).normalized;

            Vector2 perpendicular = new Vector2(-direction.y, direction.x) * curveStrength; // Calculate perpendicular force
            rb.velocity = (direction * moveSpeed) + perpendicular; // Add perpendicular force

            if (Vector2.Distance(transform.position, mousePosition) < 1.0f)
            {
                boom = true;
                touchedCursor = true;
                playerPosition = player.transform.position;
            }
        }

        if (touchedCursor)
        {
            if (!boom)
            {
                rb.AddForce(Vector2.down * upwardForce, ForceMode2D.Impulse);
                boom = true;
            }

            Vector2 direction = (playerPosition - transform.position).normalized;
            Vector2 perpendicular = new Vector2(-direction.y, direction.x) * curveStrength; // Calculate perpendicular force
            rb.velocity = (direction * moveSpeed) + perpendicular; // Add perpendicular force

            if (Vector2.Distance(transform.position, playerPosition) < 1.0f)
            {
                boom = false;
                touchedCursor = false;
            }
        }
    }
}