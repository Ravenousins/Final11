using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Magic : MonoBehaviour
{

    [Header("References")]
    [SerializeField] private Rigidbody2D rb;

    [Header("Attributes")]
    [SerializeField] private float projectileSpeed = 5f;

    private Transform target;


    private int magicDamage;

    public void SetDamage(int damage)
    {
        magicDamage = damage;
    }


    public void SetTarget(Transform _target)
    {
        target = _target;

        Destroy(gameObject, 5f);

    }


    private void FixedUpdate()
    {
        if (!target) return;

        Vector2 direction = (target.position - transform.position).normalized;
        rb.velocity = direction * projectileSpeed;


        float angle = Mathf.Atan2(direction.y, direction.x);

        // Convert the angle to degrees
        angle = angle * Mathf.Rad2Deg;

        // Rotate the magic to face the target
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        Health health = other.gameObject.GetComponent<Health>();

        if (other.gameObject.layer == LayerMask.NameToLayer("Enemy") && health != null)
        {


            health.TakeDamage(magicDamage);


                Destroy(gameObject);

        }
    }
}
