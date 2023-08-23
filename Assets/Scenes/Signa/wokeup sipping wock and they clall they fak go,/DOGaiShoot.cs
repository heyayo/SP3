using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DOGaiShoot : MonoBehaviour
{
    public GameObject prefabToShoot;
    public float minShootInterval = 3f;
    public float maxShootInterval = 5f;
    public float shootForce = 10f; // Adjust the force value as needed
    private float timeSinceLastShoot;

    // Start is called before the first frame update
    void Start()
    {
        timeSinceLastShoot = Random.Range(minShootInterval, maxShootInterval);
    }

    // Update is called once per frame
    void Update()
    {
        timeSinceLastShoot -= Time.deltaTime;
        if (timeSinceLastShoot <= 0f)
        {
            Shoot();
            timeSinceLastShoot = Random.Range(minShootInterval, maxShootInterval);
        }
    }

    void Shoot()
    {
        GameObject newProjectile = Instantiate(prefabToShoot, transform.position, Quaternion.identity);
        Rigidbody2D rb2d = newProjectile.GetComponent<Rigidbody2D>();

        if (rb2d != null)
        {
            // Get the direction the sprite is facing (upwards)
            Vector2 shootingDirection = transform.up;

            // Apply a force in the shooting direction to the projectile
            rb2d.AddForce(shootingDirection * shootForce, ForceMode2D.Impulse);
        }
    }
}