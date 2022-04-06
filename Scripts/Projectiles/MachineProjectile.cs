using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MachineProjectile : Projectile
{ 
    public Vector2 Direction{get;set;}

    protected override void Update() 
    {
        if(_enemyTarget != null)
        {
            MoveProjectile();

        }
    }
    //when this machine projectile colides with an enemy
    protected override void MoveProjectile()
    {
        Vector2 movement = Direction.normalized * moveSpeed * Time.deltaTime;
        transform.Translate(movement);
    }

    private void OnTriggerEnter2D(Collider2D other) 
    {
        if(other.CompareTag("Enemy"))
        {
            //we deal damage to our enemy return this new projectile back to the pool
            Enemy enemy = other.GetComponent<Enemy>();
            if(enemy.EnemyHealth.CurrentHealth > 0f)
            {
                enemy.EnemyHealth.DealDamage(damage);
            }

            ObjectPooler.ReturnToPool(gameObject);

        }
    }
}
