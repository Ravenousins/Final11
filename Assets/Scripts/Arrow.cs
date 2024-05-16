using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Rigidbody2D rb;

    [Header("Attributes")]
    [SerializeField] private float projectileSpeed = 20f;
    [SerializeField] private int pierceCount = 1;

    private Transform target;

    private Vector2 direction;

    private int arrowDamage;

    public void SetDamage(int damage)
    {
        arrowDamage = damage;
    }

    public void SetTarget(Transform _target)
    {
        target = _target;

        direction = (_target.position - transform.position).normalized;

        float angle = Mathf.Atan2(direction.y, direction.x);

        // Convert the angle to degrees
        angle = angle * Mathf.Rad2Deg;

        // Rotate the arrow to face the target
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));

    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        if(!target) return;

        rb.velocity = direction * projectileSpeed;

    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        Health health = other.gameObject.GetComponent<Health>();

        if (other.gameObject.layer == LayerMask.NameToLayer("Enemy") && health != null)
        {
           
            
            health.TakeDamage(arrowDamage);
             
            pierceCount--;
           
            if (pierceCount <= 0)
            {
                Destroy(gameObject);
            }
        }
    }

    void OnBecameInvisible()
    {
        Destroy(gameObject);
    }

}
