using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MachineTurretProjectile : TurretProjectile
{
    protected override void Update()
    {
        if(Time.time > _nextAttackTime)
        { //from this machine turret position toward current target position
            if(_turret.CurrentEnemyTarget !=null)
            {
                Vector3 dirToTarget = _turret.CurrentEnemyTarget.transform.position - transform.position;
                FireProjectile(dirToTarget);
            }
            _nextAttackTime = Time.time + delayBtwAttacks;
        }
    }

    protected override void LoadProjectile()
    {
        base.LoadProjectile();
    }

    private void FireProjectile(Vector3 direction)
    {
        GameObject instance = _pooler.GetInstanceFromPool();
        instance.transform.position = projectileSpawnPosition.position;

        MachineProjectile projectile = instance.GetComponent<MachineProjectile>();
        projectile.Direction = direction;
        instance.SetActive(true);
    }
}
