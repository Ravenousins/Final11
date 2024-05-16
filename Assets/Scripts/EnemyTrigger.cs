using UnityEngine;

public class EnemyTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            Turret[] turrets = FindObjectsOfType<Turret>();
            foreach (Turret turret in turrets)
            {
                turret.FindTarget();
            }
        }
    }
}