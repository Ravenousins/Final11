using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{

    [Header("Attributes")]
    [SerializeField] private int maxHitPoints = 2;
    [SerializeField] private bool isEnemy = true;
    public int currentHitPoints;
    //[SerializeField] private float regenRate = 1f;
    [SerializeField] private int soulsDropped = 15;

    public void TakeDamage(int dmg)
    {
        currentHitPoints -= dmg;

        if (currentHitPoints <= 0) {
            if (isEnemy && EnemySpawner.onEnemyDestroy != null)
            {
                EnemySpawner.onEnemyDestroy.Invoke();
                LevelManager.main.IncreaseSouls(soulsDropped);
            }
            Destroy(gameObject);

        }
    }


}
