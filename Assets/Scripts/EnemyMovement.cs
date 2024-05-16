using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{

    [Header("References")]
    [SerializeField] private Rigidbody2D rb;

    [Header("Attributes")]
    [SerializeField] private float moveSpeed = 2f;


    private Transform target;
    private int pathIndex = 0;
    private bool isMoving = true;

    // Start is called before the first frame update
    void Start()
    {
        target = LevelManager.main.path[pathIndex];
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector2.Distance(target.position, transform.position) <= 0.1f) {

            pathIndex++;

            if(pathIndex == LevelManager.main.path.Length)
            {
                EnemySpawner.onEnemyDestroy.Invoke();
                LevelManager.main.DecreaseDefenseHealth(1);
                Destroy(gameObject);
                return;
            } else {
                target = LevelManager.main.path[pathIndex];
            }

        }
    }
        
    public void StopMoving()
    {
        isMoving = false;
    }

    public void StartMoving()
    {
        isMoving = true;
    }


    private void FixedUpdate()
    {
        if (isMoving && !LevelManager.isGameOver)
        {
            Vector2 direction = (target.position - transform.position);
            rb.velocity = direction.normalized * moveSpeed;

        } else {
            rb.velocity = Vector2.zero;

        }
    }

    public int GetPathIndex()
    {
        return pathIndex;
    }


}
