using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomeRunTest : MonoBehaviour
{
    public float rotationSpeed = 180f; // Adjust this value to control the rotation speed
    public float targetRotationRightClick = 150f;
    public float targetRotationLeftClick = -210f;

    private Rigidbody2D rb;
    private float initialRotation;
    private float targetRotation;
    private Quaternion firstrotate;
    private bool rotating = false;

    public Vector2 boxSize = new Vector2(1f, 1f);
    public float hitForce = 5f; // Adjust the force as needed

    GameObject player;

    private void Start()
    {
        player = PlayerManager.Instance.gameObject;

        if (player == null)
        {
            Debug.LogError("Player object is not assigned or missing.");
        }
        else
        {
            rb = GetComponent<Rigidbody2D>();
            initialRotation = rb.rotation;
            targetRotation = initialRotation;
        }
    }

    private void Update()
    {

        if (player != null)
        {
            transform.position = player.transform.position;
            // Rest of your code
        }
        if (Input.GetMouseButton(0)) // Left click
        {
            Vector3 mousePosition = Input.mousePosition;
            Vector3 playerPosition = Camera.main.WorldToScreenPoint(player.transform.position);;
            Vector3 directionToMouse = mousePosition - playerPosition;
            if (directionToMouse.x >= 0)
            {
                targetRotation = targetRotationRightClick; // Rotate left
            }
            else
            {
                targetRotation = targetRotationLeftClick;
            }
            rotating = true;

        }
      
        if (rotating)
        {
            rb.rotation = Mathf.MoveTowards(rb.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            if (Mathf.Abs(rb.rotation - targetRotation) < 0.1f)
            {

                rotating = false;
                rb.rotation = initialRotation;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Check if the colliding object is on the "baseball" layer
        if (rotating) // Check if the object is currently rotating
        {
            // Check if the colliding object is on the "baseball" layer
            if (collision.gameObject.layer == LayerMask.NameToLayer("baseball") && collision.gameObject != null)
            {
                Rigidbody2D baseballRB = collision.gameObject.GetComponent<Rigidbody2D>();

                if (baseballRB != null)
                {
                    baseballRB.gravityScale = 0;
                    Vector2 hitDirection = (collision.transform.position - transform.position).normalized;
                    baseballRB.AddForce(hitDirection * hitForce, ForceMode2D.Impulse);
                }
            }
        }
    }
    







 

    //private void CheckHit()
    //{



    //    Vector2 direction = transform.up;
    //    Collider2D[] colliders = Physics2D.OverlapBoxAll(transform.position, boxSize, transform.rotation.eulerAngles.z, LayerMask.GetMask("baseball"));

    //    foreach (Collider2D collider in colliders)
    //    {
    //        Debug.Log("Bat hit baseball!");

    //        Rigidbody2D baseballRB = collider.GetComponent<Rigidbody2D>();
    //        if (baseballRB != null)
    //        {
    //            Vector2 hitDirection = direction;
    //            baseballRB.AddForce(hitDirection * hitForce, ForceMode2D.Impulse);
    //        }
    //    }
    //}

    
}