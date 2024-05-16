using System.Collections;
using System;
using UnityEngine;
using UnityEditor;
using TMPro;

public enum ProjectileType
{
    Arrow,
    Magic
}

public class Turret : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private LayerMask enemyMask;
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private Transform firingPoint;
    [SerializeField] private float xOffset = 0f;
    [SerializeField] private float yOffset = 0f;
    [SerializeField] private ProjectileType projectileType;
    public int level = 1;

    [Header("Attribute")]
    [SerializeField] private float targetingRange = 4f;
    [SerializeField] private int damage = 1;
    [SerializeField] private float aps = 1f; // Attacks Per Second
    public int cost = 100;

    [Header("Werewolf Attributes")]
    [SerializeField] private bool isWerewolf = false;
   // [SerializeField] private float respawnTime = 5f;
    [SerializeField] private int speed = 3;
    [SerializeField] private float rallyRange = 3f;
    [SerializeField] private Transform siblingObject;

    //private Animator animator;



    private Transform target;
    private float timeUntilFire;

    private bool isSettingRallyPoint = false;
    private bool hasAttacked = false;

    private Vector3 rallyPoint;

    public TextMeshProUGUI targetingRangeText;
    public TextMeshProUGUI damageText;
    public TextMeshProUGUI apsText;
    public TextMeshProUGUI healthText;


    CircleCollider2D cc;

    private void OnDrawGizmosSelected()
    {
       // Handles.color = Color.cyan;
        Vector3 newPosition = new Vector3(transform.position.x + xOffset, transform.position.y + yOffset, transform.position.z);
       // Handles.DrawWireDisc(newPosition, transform.forward, targetingRange);
    }


    private void FaceTarget ()
    {
        if (target != null)
        {
            // Get the direction to the target
            Vector3 directionToTarget = target.position - transform.position;

            // Get the SpriteRenderer component
            SpriteRenderer sr = GetComponent<SpriteRenderer>();

            // Flip the sprite based on the direction of the target
            sr.flipX = directionToTarget.x < 0;
        }

    }

     void Start()
    {
       // animator = GetComponent<Animator>();

        CircleCollider2D cc = GetComponent<CircleCollider2D>();
      if (!isWerewolf)
        {
            cc.radius = targetingRange;
        }
        rallyPoint = siblingObject.position;
    }

    // Update is called once per frame
    private void Update()
    {

        FaceTarget();
        if (target != null) {


            timeUntilFire += Time.deltaTime;

           // animator.SetBool("isAttacking", true);
           // Debug.Log("isAttacking set to true");

            if (timeUntilFire >= 1f / aps)
            {

                if (isWerewolf) {
                    DealDamage();
                } else {
                    Shoot();
                }
           //     hasAttacked = true;
           //    Debug.Log("Hasattacked set to true");
                timeUntilFire = 0f;
            } else if (hasAttacked) {

           //   animator.SetBool("isAttacking", false);
           //   Debug.Log("isAttacking set to false");
           //   hasAttacked = false;
            }




                 
        }


        if (isSettingRallyPoint && Input.GetMouseButtonDown(0))
        {
            Vector3 clickPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            // Create a new Vector3 with the x and y values from the click position and the z value from the current transform
            Vector3 newPosition = new Vector3(clickPosition.x, clickPosition.y, transform.position.z);
            float distance = Vector3.Distance(newPosition, transform.position);

            // If the click is within the allowed radius, update the rally point
            if (distance <= rallyRange)
            {
                SetRallyPoint(newPosition);
                isSettingRallyPoint = false;
            }
        }

        // If the werewolf is not at the rally point, move towards it
        if (isWerewolf && Vector3.Distance(transform.position, rallyPoint) > 0.1f)
        {
            Vector3 direction = (rallyPoint - transform.position).normalized;

            transform.position = Vector3.MoveTowards(transform.position, rallyPoint, speed * Time.deltaTime);

            FaceDirection(direction);


        }
    }


    public void Sell()
    {
        int refund = Mathf.RoundToInt(cost * 0.6f);

        LevelManager.main.IncreaseSouls(refund);

        Transform root = transform;
        while (root.parent != null)
        {
            root = root.parent;
        }

        Destroy(root.gameObject);
    }

    public void LevelUp()
    {
  
        int levelUpCost = cost * level;


        if (LevelManager.main.SpendSouls(levelUpCost))
        {

            level++;



            if (!isWerewolf) {
                targetingRange = (float)Math.Ceiling(targetingRange * 1.1);
                cc.radius = targetingRange;
            }

            damage = (int)Math.Ceiling(damage * 1.1);

            aps = (float)Math.Ceiling(aps * 1.1);

            UpdateUI();



            Debug.Log("Leveled up tower to level " + level);
        } else {
            // If SpendSouls returned false, there were not enough souls
            // You can show a message to the user
            Debug.Log("Not enough souls to level up this tower.");
        }
    }


private void FaceDirection(Vector3 direction)
    {
        SpriteRenderer sr = GetComponent<SpriteRenderer>();

        // Flip the sprite based on the direction of movement
        sr.flipX = direction.x < 0;
    }

    public void UpdateUI()
    {
        Debug.Log("UpdateUI called");

        if (!isWerewolf)
        {
            if (targetingRangeText != null)
            {
                targetingRangeText.text = "Targeting Range: " + targetingRange;
            }
              
       
        } else {
            
            if (healthText != null)
            {
                healthText.text = "Health: " + GetComponent<Health>().currentHitPoints;
            }

        }

        if (damageText != null)
        {
            damageText.text = "Damage: " + damage;

        }


        if (apsText != null)
        {
            apsText.text = "Attack Speed: " + aps;
        }

    }


    public void StartSettingRallyPoint()
    {
  
        Debug.Log("Button Was Clicked");
         StartCoroutine(DelayedSetRallyPoint());
       

    }

    private IEnumerator DelayedSetRallyPoint()
    {
        // Wait for a short delay
        yield return new WaitForSeconds(0.3f);
           isSettingRallyPoint = true;

    }


    public void SetRallyPoint(Vector3 newRallyPoint)
    {
        rallyPoint = newRallyPoint;
    }

    private void Shoot()
    {
        GameObject projectileObj = Instantiate(projectilePrefab, firingPoint.position, Quaternion.identity);

        switch (projectileType)
        {
            case ProjectileType.Arrow:
                Arrow arrowScript = projectileObj.GetComponent<Arrow>();
                arrowScript.SetTarget(target);
                arrowScript.SetDamage(damage);
                break;
            case ProjectileType.Magic:
                Magic magicScript = projectileObj.GetComponent<Magic>();
                magicScript.SetTarget(target);
                magicScript.SetDamage(damage);
                break;
        }
    }


    private void DealDamage()
    {
        FaceTarget();
        Health health = target.GetComponent<Health>();
        if (health != null)
        {
            health.TakeDamage(damage);
        }
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        // An enemy entered the turret's range, so find a new target
        if (other.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            FindTarget();
        }

        if (isWerewolf && Vector3.Distance(transform.position, rallyPoint) <= 0.1f)
        {
            if (other.gameObject.layer == LayerMask.NameToLayer("Enemy"))
            {
                // Stop the enemy's movement
                other.gameObject.GetComponent<EnemyMovement>().StopMoving();
            }
            target = other.transform;
            DealDamage();
        }

    }

    private void OnTriggerExit2D(Collider2D other)
    {
        // An enemy left the turret's range, so find a new target
        if (other.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            FindTarget();
        }
    }

    public void FindTarget()
    {
        RaycastHit2D[] hits = Physics2D.CircleCastAll(transform.position, targetingRange, (Vector2)transform.position, 0f, enemyMask);

        if (hits.Length > 0)
        {
            // Initialize the target to the first hit
            target = hits[0].transform;
            int maxPathIndex = target.GetComponent<EnemyMovement>().GetPathIndex();

            // Loop through the rest of the hits
            for (int i = 1; i < hits.Length; i++)
            {
                int pathIndex = hits[i].transform.GetComponent<EnemyMovement>().GetPathIndex();
                // If this hit is further along the path than the current target, update the target
                if (pathIndex > maxPathIndex)
                {
                    target = hits[i].transform;
                    maxPathIndex = pathIndex;
                }
            }
        } else {
            // If no targets are in range, set target to null
            target = null;
        }
    }

}
